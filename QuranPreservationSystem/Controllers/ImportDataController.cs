using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Domain.Entities;
using ClosedXML.Excel;
using System.Data;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// استيراد البيانات - Import Data
    /// </summary>
    [Authorize]
    public class ImportDataController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ImportDataController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ImportDataController(
            IUnitOfWork unitOfWork,
            ILogger<ImportDataController> logger,
            IWebHostEnvironment webHostEnvironment,
            IServiceScopeFactory serviceScopeFactory)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // GET: ImportData
        public IActionResult Index()
        {
            return View();
        }

        // GET: ImportData/Centers
        public async Task<IActionResult> Centers()
        {
            // عرض السجلات المؤقتة
            var tempRecords = await _unitOfWork.TempCenterImports.GetAllAsync();
            ViewBag.TempRecords = tempRecords.OrderByDescending(t => t.UploadedDate).Take(50);
            
            return View();
        }

        // GET: ImportData/DownloadCentersTemplate
        public IActionResult DownloadCentersTemplate()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("المراكز");

            // Header
            worksheet.Cell(1, 1).Value = "اسم المركز *";
            worksheet.Cell(1, 2).Value = "العنوان";
            worksheet.Cell(1, 3).Value = "رقم الهاتف";
            worksheet.Cell(1, 4).Value = "الوصف";
            worksheet.Cell(1, 5).Value = "نشط (نعم/لا)";

            // Styling Header
            var headerRange = worksheet.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e7d32");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            // Sample Data
            worksheet.Cell(2, 1).Value = "مركز الاردن القرآني";
            worksheet.Cell(2, 2).Value = "الاردن - عمان";
            worksheet.Cell(2, 3).Value = "0123456789";
            worksheet.Cell(2, 4).Value = "مركز لتعليم القرآن الكريم";
            worksheet.Cell(2, 5).Value = "نعم";
            worksheet.Cell(2, 6).Value = "مثال تعليمي";
      
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"قالب_المراكز_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // POST: ImportData/UploadCenters
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCenters(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["Error"] = "الرجاء اختيار ملف Excel";
                return RedirectToAction(nameof(Centers));
            }

            if (!excelFile.FileName.EndsWith(".xlsx") && !excelFile.FileName.EndsWith(".xls"))
            {
                TempData["Error"] = "يجب أن يكون الملف بصيغة Excel (.xlsx أو .xls)";
                return RedirectToAction(nameof(Centers));
            }

            try
            {
                var batchId = Guid.NewGuid().ToString();
                var uploadedBy = User.Identity?.Name ?? "Unknown";
                int rowNumber = 0;
                int successCount = 0;
                int errorCount = 0;

                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);

                    // البدء من الصف 2 (تخطي الـ Header)
                    var rows = worksheet.RowsUsed().Skip(2);

                    foreach (var row in rows)
                    {
                        rowNumber++;

                        try
                        {
                            var name = row.Cell(1).GetString().Trim();
                            
                            // التحقق من الحقول المطلوبة
                            if (string.IsNullOrWhiteSpace(name))
                            {
                                continue; // تخطي الصفوف الفارغة
                            }

                            var tempCenter = new TempCenterImport
                            {
                                Name = name,
                                Address = row.Cell(2).GetString().Trim(),
                                PhoneNumber = row.Cell(3).GetString().Trim(),
                                Description = row.Cell(4).GetString().Trim(),
                                IsActive = row.Cell(5).GetString().Trim().Equals("نعم", StringComparison.OrdinalIgnoreCase),
                                Status = ImportStatus.Pending,
                                UploadedBy = uploadedBy,
                                UploadedDate = DateTime.Now,
                                BatchId = batchId,
                                RowNumber = rowNumber
                            };

                            await _unitOfWork.TempCenterImports.AddAsync(tempCenter);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "خطأ في معالجة الصف {RowNumber}", rowNumber);
                            errorCount++;
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                // بدء معالجة البيانات في Background
                _ = Task.Run(async () => await ProcessPendingImportsAsync(batchId));

                TempData["Success"] = $"تم رفع {successCount} سجل بنجاح. جاري المعالجة...";
                if (errorCount > 0)
                {
                    TempData["Warning"] = $"فشل في قراءة {errorCount} سجل";
                }

                return RedirectToAction(nameof(Centers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في رفع ملف Excel");
                TempData["Error"] = "حدث خطأ أثناء معالجة الملف";
                return RedirectToAction(nameof(Centers));
            }
        }

        // Background Processing
        private async Task ProcessPendingImportsAsync(string batchId)
        {
            // استخدام scope جديد للـ background processing
            using var scope = _serviceScopeFactory.CreateScope();
            var scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                await Task.Delay(2000); // انتظار قليلاً للتأكد من اكتمال الـ request الأصلي

                var pendingRecords = await scopedUnitOfWork.TempCenterImports.GetByBatchIdAsync(batchId);

                foreach (var tempRecord in pendingRecords.Where(r => r.Status == ImportStatus.Pending))
                {
                    // بدء Transaction لكل سجل
                    await scopedUnitOfWork.BeginTransactionAsync();
                    
                    try
                    {
                        // تحديث الحالة إلى Processing
                        tempRecord.Status = ImportStatus.Processing;
                        await scopedUnitOfWork.TempCenterImports.UpdateAsync(tempRecord);
                        await scopedUnitOfWork.SaveChangesAsync();

                        // التحقق من عدم التكرار
                        var exists = await scopedUnitOfWork.Centers.ExistsAsync(c => c.Name == tempRecord.Name);
                        
                        if (exists)
                        {
                            tempRecord.Status = ImportStatus.Duplicate;
                            tempRecord.ErrorMessage = "المركز موجود بالفعل";
                            tempRecord.ProcessedDate = DateTime.Now;
                            await scopedUnitOfWork.TempCenterImports.UpdateAsync(tempRecord);
                            await scopedUnitOfWork.SaveChangesAsync();
                            await scopedUnitOfWork.CommitTransactionAsync();
                            continue;
                        }

                        // إضافة المركز الجديد
                        var center = new Center
                        {
                            Name = tempRecord.Name,
                            Address = tempRecord.Address,
                            PhoneNumber = tempRecord.PhoneNumber,
                            Description = tempRecord.Description,
                            IsActive = tempRecord.IsActive,
                            CreatedDate = DateTime.Now
                        };

                        await scopedUnitOfWork.Centers.AddAsync(center);
                        await scopedUnitOfWork.SaveChangesAsync();

                        // تحديث الحالة إلى Completed
                        var addedCenter = await scopedUnitOfWork.Centers.GetFirstOrDefaultAsync(c => c.Name == tempRecord.Name);
                        tempRecord.ProcessedCenterId = addedCenter?.CenterId;
                        tempRecord.Status = ImportStatus.Completed;
                        tempRecord.ProcessedDate = DateTime.Now;
                        tempRecord.ErrorMessage = null;
                        await scopedUnitOfWork.TempCenterImports.UpdateAsync(tempRecord);
                        await scopedUnitOfWork.SaveChangesAsync();

                        // Commit Transaction
                        await scopedUnitOfWork.CommitTransactionAsync();

                        _logger.LogInformation("تمت معالجة السجل {TempId} - المركز: {CenterName}", 
                            tempRecord.TempId, tempRecord.Name);
                    }
                    catch (Exception ex)
                    {
                        // Rollback Transaction
                        await scopedUnitOfWork.RollbackTransactionAsync();

                        _logger.LogError(ex, "خطأ في معالجة السجل {TempId}", tempRecord.TempId);
                        
                        // تحديث الحالة إلى Failed
                        tempRecord.Status = ImportStatus.Failed;
                        tempRecord.ErrorMessage = ex.Message.Length > 500 ? ex.Message.Substring(0, 500) : ex.Message;
                        tempRecord.ProcessedDate = DateTime.Now;
                        await scopedUnitOfWork.TempCenterImports.UpdateAsync(tempRecord);
                        await scopedUnitOfWork.SaveChangesAsync();
                    }
                }

                _logger.LogInformation("تمت معالجة الدفعة {BatchId}", batchId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ عام في معالجة الدفعة {BatchId}", batchId);
            }
        }

        // GET: ImportData/GetImportStatus
        public async Task<IActionResult> GetImportStatus(string batchId)
        {
            var records = await _unitOfWork.TempCenterImports.GetByBatchIdAsync(batchId);
            
            var stats = new
            {
                Total = records.Count(),
                Pending = records.Count(r => r.Status == ImportStatus.Pending),
                Processing = records.Count(r => r.Status == ImportStatus.Processing),
                Completed = records.Count(r => r.Status == ImportStatus.Completed),
                Failed = records.Count(r => r.Status == ImportStatus.Failed),
                Duplicate = records.Count(r => r.Status == ImportStatus.Duplicate)
            };

            return Json(stats);
        }

        #region Teachers Import

        // GET: ImportData/Teachers
        public async Task<IActionResult> Teachers()
        {
            // عرض السجلات المؤقتة
            var tempRecords = await _unitOfWork.TempTeacherImports.GetAllAsync();
            ViewBag.TempRecords = tempRecords.OrderByDescending(t => t.UploadedDate).Take(50);
            
            return View();
        }

        // GET: ImportData/DownloadTeachersTemplate
        public IActionResult DownloadTeachersTemplate()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("المدرسين");

            // Header
            worksheet.Cell(1, 1).Value = "الاسم الأول *";
            worksheet.Cell(1, 2).Value = "اسم العائلة *";
            worksheet.Cell(1, 3).Value = "رقم الهاتف *";
            worksheet.Cell(1, 4).Value = "البريد الإلكتروني";
            worksheet.Cell(1, 5).Value = "العنوان";
            worksheet.Cell(1, 6).Value = "تاريخ الميلاد (dd/mm/yyyy)";
            worksheet.Cell(1, 7).Value = "الجنس (ذكر/أنثى)";
            worksheet.Cell(1, 8).Value = "المؤهل";
            worksheet.Cell(1, 9).Value = "التخصص";
            worksheet.Cell(1, 10).Value = "تاريخ التعيين (dd/mm/yyyy) *";
            worksheet.Cell(1, 11).Value = "المركز *";
            worksheet.Cell(1, 12).Value = "نشط (نعم/لا)";

            // Styling Header
            var headerRange = worksheet.Range(1, 1, 1, 12);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e7d32");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

            // Sample Data
            worksheet.Cell(2, 1).Value = "أحمد";
            worksheet.Cell(2, 2).Value = "محمد";
            worksheet.Cell(2, 3).Value = "0123456789";
            worksheet.Cell(2, 4).Value = "ahmad@example.com";
            worksheet.Cell(2, 5).Value = "القاهرة";
            worksheet.Cell(2, 6).Value = "15/03/1985";
            worksheet.Cell(2, 7).Value = "ذكر";
            worksheet.Cell(2, 8).Value = "بكالوريوس";
            worksheet.Cell(2, 9).Value = "قراءات قرآنية";
            worksheet.Cell(2, 10).Value = "01/01/2020";
            worksheet.Cell(2, 11).Value = "مركز كفرأبيل القرآني";
            worksheet.Cell(2, 12).Value = "نعم";

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"قالب_المدرسين_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // POST: ImportData/UploadTeachers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadTeachers(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["Error"] = "الرجاء اختيار ملف Excel";
                return RedirectToAction(nameof(Teachers));
            }

            if (!excelFile.FileName.EndsWith(".xlsx") && !excelFile.FileName.EndsWith(".xls"))
            {
                TempData["Error"] = "يجب أن يكون الملف بصيغة Excel (.xlsx أو .xls)";
                return RedirectToAction(nameof(Teachers));
            }

            try
            {
                var batchId = Guid.NewGuid().ToString();
                var uploadedBy = User.Identity?.Name ?? "Unknown";
                int rowNumber = 0;
                int successCount = 0;
                int errorCount = 0;

                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);

                    // البدء من الصف 2 (تخطي الـ Header)
                    var rows = worksheet.RowsUsed().Skip(1);

                    foreach (var row in rows)
                    {
                        rowNumber++;

                        try
                        {
                            var firstName = row.Cell(1).GetString().Trim();
                            var lastName = row.Cell(2).GetString().Trim();
                            var phoneNumber = row.Cell(3).GetString().Trim();
                            
                            // التحقق من الحقول المطلوبة
                            if (string.IsNullOrWhiteSpace(firstName) || 
                                string.IsNullOrWhiteSpace(lastName) || 
                                string.IsNullOrWhiteSpace(phoneNumber))
                            {
                                continue; // تخطي الصفوف الناقصة
                            }

                            var hireDateStr = row.Cell(10).GetString().Trim();
                            var centerName = row.Cell(11).GetString().Trim();

                            if (string.IsNullOrWhiteSpace(hireDateStr) || string.IsNullOrWhiteSpace(centerName))
                            {
                                continue;
                            }

                            var tempTeacher = new TempTeacherImport
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                PhoneNumber = phoneNumber,
                                Email = row.Cell(4).GetString().Trim(),
                                Address = row.Cell(5).GetString().Trim(),
                                DateOfBirth = TryParseDate(row.Cell(6).GetString().Trim()),
                                Gender = row.Cell(7).GetString().Trim(),
                                Qualification = row.Cell(8).GetString().Trim(),
                                Specialization = row.Cell(9).GetString().Trim(),
                                HireDate = TryParseDate(hireDateStr) ?? DateTime.Now,
                                CenterName = centerName,
                                IsActive = row.Cell(12).GetString().Trim().Equals("نعم", StringComparison.OrdinalIgnoreCase),
                                Status = ImportStatus.Pending,
                                UploadedBy = uploadedBy,
                                UploadedDate = DateTime.Now,
                                BatchId = batchId,
                                RowNumber = rowNumber
                            };

                            await _unitOfWork.TempTeacherImports.AddAsync(tempTeacher);
                            successCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "خطأ في معالجة الصف {RowNumber}", rowNumber);
                            errorCount++;
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                // بدء معالجة البيانات في Background
                _ = Task.Run(async () => await ProcessPendingTeachersAsync(batchId));

                TempData["Success"] = $"تم رفع {successCount} مدرس بنجاح. جاري المعالجة...";
                if (errorCount > 0)
                {
                    TempData["Warning"] = $"فشل في قراءة {errorCount} سجل";
                }

                return RedirectToAction(nameof(Teachers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في رفع ملف Excel");
                TempData["Error"] = "حدث خطأ أثناء معالجة الملف";
                return RedirectToAction(nameof(Teachers));
            }
        }

        // Background Processing for Teachers
        private async Task ProcessPendingTeachersAsync(string batchId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            try
            {
                await Task.Delay(2000);

                var pendingRecords = await scopedUnitOfWork.TempTeacherImports.GetByBatchIdAsync(batchId);

                foreach (var tempRecord in pendingRecords.Where(r => r.Status == ImportStatus.Pending))
                {
                    await scopedUnitOfWork.BeginTransactionAsync();
                    
                    try
                    {
                        tempRecord.Status = ImportStatus.Processing;
                        await scopedUnitOfWork.TempTeacherImports.UpdateAsync(tempRecord);
                        await scopedUnitOfWork.SaveChangesAsync();

                        // البحث عن المركز
                        var center = await scopedUnitOfWork.Centers.GetFirstOrDefaultAsync(c => c.Name == tempRecord.CenterName);
                        
                        if (center == null)
                        {
                            tempRecord.Status = ImportStatus.Failed;
                            tempRecord.ErrorMessage = $"المركز '{tempRecord.CenterName}' غير موجود";
                            tempRecord.ProcessedDate = DateTime.Now;
                            await scopedUnitOfWork.TempTeacherImports.UpdateAsync(tempRecord);
                            await scopedUnitOfWork.SaveChangesAsync();
                            await scopedUnitOfWork.CommitTransactionAsync();
                            continue;
                        }

                        // التحقق من عدم التكرار
                        var exists = await scopedUnitOfWork.Teachers.ExistsAsync(t => 
                            t.FirstName == tempRecord.FirstName && 
                            t.LastName == tempRecord.LastName && 
                            t.PhoneNumber == tempRecord.PhoneNumber);
                        
                        if (exists)
                        {
                            tempRecord.Status = ImportStatus.Duplicate;
                            tempRecord.ErrorMessage = "المدرس موجود بالفعل";
                            tempRecord.ProcessedDate = DateTime.Now;
                            await scopedUnitOfWork.TempTeacherImports.UpdateAsync(tempRecord);
                            await scopedUnitOfWork.SaveChangesAsync();
                            await scopedUnitOfWork.CommitTransactionAsync();
                            continue;
                        }

                        // تحديد الجنس
                        Domain.Enums.Gender? gender = null;
                        if (tempRecord.Gender?.Equals("ذكر", StringComparison.OrdinalIgnoreCase) == true)
                            gender = Domain.Enums.Gender.Male;
                        else if (tempRecord.Gender?.Equals("أنثى", StringComparison.OrdinalIgnoreCase) == true)
                            gender = Domain.Enums.Gender.Female;

                        // إضافة المدرس الجديد
                        var teacher = new Teacher
                        {
                            FirstName = tempRecord.FirstName,
                            LastName = tempRecord.LastName,
                            PhoneNumber = tempRecord.PhoneNumber,
                            Email = tempRecord.Email,
                            Address = tempRecord.Address,
                            DateOfBirth = tempRecord.DateOfBirth,
                            Gender = gender,
                            Qualification = tempRecord.Qualification,
                            Specialization = tempRecord.Specialization,
                            HireDate = tempRecord.HireDate,
                            CenterId = center.CenterId,
                            IsActive = tempRecord.IsActive
                        };

                        await scopedUnitOfWork.Teachers.AddAsync(teacher);
                        await scopedUnitOfWork.SaveChangesAsync();

                        // تحديث الحالة إلى Completed
                        var addedTeacher = await scopedUnitOfWork.Teachers.GetFirstOrDefaultAsync(t => 
                            t.FirstName == tempRecord.FirstName && 
                            t.LastName == tempRecord.LastName && 
                            t.PhoneNumber == tempRecord.PhoneNumber);
                        
                        tempRecord.ProcessedTeacherId = addedTeacher?.TeacherId;
                        tempRecord.Status = ImportStatus.Completed;
                        tempRecord.ProcessedDate = DateTime.Now;
                        tempRecord.ErrorMessage = null;
                        await scopedUnitOfWork.TempTeacherImports.UpdateAsync(tempRecord);
                        await scopedUnitOfWork.SaveChangesAsync();

                        await scopedUnitOfWork.CommitTransactionAsync();

                        _logger.LogInformation("تمت معالجة المدرس {TempId} - {Name}", 
                            tempRecord.TempId, $"{tempRecord.FirstName} {tempRecord.LastName}");
                    }
                    catch (Exception ex)
                    {
                        await scopedUnitOfWork.RollbackTransactionAsync();

                        _logger.LogError(ex, "خطأ في معالجة السجل {TempId}", tempRecord.TempId);
                        
                        tempRecord.Status = ImportStatus.Failed;
                        tempRecord.ErrorMessage = ex.Message.Length > 500 ? ex.Message.Substring(0, 500) : ex.Message;
                        tempRecord.ProcessedDate = DateTime.Now;
                        await scopedUnitOfWork.TempTeacherImports.UpdateAsync(tempRecord);
                        await scopedUnitOfWork.SaveChangesAsync();
                    }
                }

                _logger.LogInformation("تمت معالجة دفعة المدرسين {BatchId}", batchId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ عام في معالجة دفعة المدرسين {BatchId}", batchId);
            }
        }

        #endregion

        #region Helper Methods

        private DateTime? TryParseDate(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
                return null;

            // Try multiple date formats
            string[] formats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd/MM/yy", "dd-MM-yy", "yyyy-MM-dd" };
            
            if (DateTime.TryParseExact(dateStr, formats, System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            if (DateTime.TryParse(dateStr, out DateTime result2))
            {
                return result2;
            }

            return null;
        }

        #endregion
    }
}

