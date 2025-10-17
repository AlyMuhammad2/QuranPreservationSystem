using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Authorization;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Helpers;
using QuranPreservationSystem.Infrastructure.Identity;
using ClosedXML.Excel;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// ديوان الحفاظ - إدارة سجل حفاظ القرآن الكريم
    /// </summary>
    [Authorize]
    [PermissionAuthorize("HafizRegistry", "View")]
    public class HafizRegistryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HafizRegistryController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuditLogService _auditLogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private const int PageSize = 10; // عدد السجلات في كل صفحة

        public HafizRegistryController(
            IUnitOfWork unitOfWork,
            ILogger<HafizRegistryController> logger,
            IWebHostEnvironment webHostEnvironment,
            IAuditLogService auditLogService,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _auditLogService = auditLogService;
            _userManager = userManager;
        }

        // GET: HafizRegistry
        public async Task<IActionResult> Index(string searchTerm, int? centerId, int pageNumber = 1)
        {
            // جلب البيانات مع Pagination
            var (hafazList, totalCount) = await _unitOfWork.HafizRegistry.GetPagedHafazAsync(
                pageNumber, 
                PageSize, 
                searchTerm, 
                centerId);

            // تحويل إلى DTOs
            var hafazDtos = hafazList.Select(h => new HafizRegistryDto
            {
                HafizId = h.HafizId,
                StudentName = h.StudentName,
                CenterId = h.CenterId,
                CenterName = h.Center?.Name,
                CompletionYear = h.CompletionYear,
                CompletedCourses = h.CompletedCourses,
                CertificateFileName = h.CertificateFileName,
                FileSizeFormatted = h.CertificateFileSize.HasValue ? FormatFileSize(h.CertificateFileSize.Value) : null,
                PhotoPath = h.PhotoPath,
                MemorizationLevel = h.MemorizationLevel,
                IsActive = h.IsActive,
                CreatedDate = h.CreatedDate,
                LastModifiedDate = h.LastModifiedDate
            }).ToList();

            // حساب Pagination Info
            var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCenterId = centerId;
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            return View(hafazDtos);
        }

        // GET: HafizRegistry/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var hafiz = await _unitOfWork.HafizRegistry.GetHafizWithDetailsAsync(id);
            if (hafiz == null)
            {
                return NotFound();
            }

            var hafizDto = new HafizRegistryDto
            {
                HafizId = hafiz.HafizId,
                StudentName = hafiz.StudentName,
                CenterId = hafiz.CenterId,
                CenterName = hafiz.Center?.Name,
                CompletionYear = hafiz.CompletionYear,
                CompletedCourses = hafiz.CompletedCourses,
                CertificateFileName = hafiz.CertificateFileName,
                FileSizeFormatted = hafiz.CertificateFileSize.HasValue ? FormatFileSize(hafiz.CertificateFileSize.Value) : null,
                PhotoPath = hafiz.PhotoPath,
                MemorizationLevel = hafiz.MemorizationLevel,
                IsActive = hafiz.IsActive,
                CreatedDate = hafiz.CreatedDate,
                LastModifiedDate = hafiz.LastModifiedDate
            };

            return View(hafizDto);
        }

        // GET: HafizRegistry/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Students = await _unitOfWork.Students.GetActiveStudentsAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View();
        }

        // POST: HafizRegistry/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHafizRegistryDto createDto)
        {
            if (ModelState.IsValid)
            {
                // بناء قائمة الدورات
                string? completedCoursesText = null;
                if (createDto.CompletedCourseIds != null && createDto.CompletedCourseIds.Any())
                {
                    var courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
                    var selectedCourses = courses.Where(c => createDto.CompletedCourseIds.Contains(c.CourseId))
                                                 .Select(c => c.CourseName);
                    completedCoursesText = string.Join(", ", selectedCourses);
                }

                var hafiz = new HafizRegistry
                {
                    StudentName = createDto.StudentName,
                    CenterId = createDto.CenterId ?? 0,
                    CompletionYear = createDto.CompletionYear,
                    CompletedCourses = completedCoursesText,
                    MemorizationLevel = createDto.MemorizationLevel,
                    IsActive = createDto.IsActive,
                    CreatedDate = DateTime.Now
                };

                // Handle Certificate PDF Upload
                if (createDto.CertificateFile != null && createDto.CertificateFile.Length > 0)
                {
                    var certResult = await SaveFileAsync(createDto.CertificateFile, "certificates");
                    if (certResult.Success)
                    {
                        hafiz.CertificatePath = certResult.FilePath;
                        hafiz.CertificateFileName = certResult.FileName;
                        hafiz.CertificateFileType = createDto.CertificateFile.ContentType;
                        hafiz.CertificateFileSize = createDto.CertificateFile.Length;
                    }
                }

                // Handle Photo Upload
                if (createDto.PhotoFile != null && createDto.PhotoFile.Length > 0)
                {
                    var photoResult = await SaveFileAsync(createDto.PhotoFile, "photos");
                    if (photoResult.Success)
                    {
                        hafiz.PhotoPath = photoResult.FilePath;
                    }
                }

                await _unitOfWork.HafizRegistry.AddAsync(hafiz);
                await _unitOfWork.SaveChangesAsync();

                await _auditLogService.LogCreateAsync(User, _userManager, HttpContext, "hafizregistry", hafiz.HafizId, hafiz, hafiz.StudentName);

                TempData["Success"] = $"تم إضافة الحافظ '{hafiz.StudentName}' إلى ديوان الحفاظ بنجاح";
                _logger.LogInformation("تم إضافة حافظ جديد: {StudentName}", hafiz.StudentName);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Students = await _unitOfWork.Students.GetActiveStudentsAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(createDto);
        }

        // GET: HafizRegistry/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var hafiz = await _unitOfWork.HafizRegistry.GetHafizWithDetailsAsync(id);
            if (hafiz == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateHafizRegistryDto
            {
                HafizId = hafiz.HafizId,
                StudentName = hafiz.StudentName,
                CenterId = hafiz.CenterId,
                CompletionYear = hafiz.CompletionYear,
                MemorizationLevel = hafiz.MemorizationLevel,
                CurrentCertificateFileName = hafiz.CertificateFileName,
                CurrentPhotoPath = hafiz.PhotoPath,
                IsActive = hafiz.IsActive
            };

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(updateDto);
        }

        // POST: HafizRegistry/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateHafizRegistryDto updateDto)
        {
            if (id != updateDto.HafizId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var hafiz = await _unitOfWork.HafizRegistry.GetHafizWithDetailsAsync(id);
                if (hafiz == null)
                {
                    return NotFound();
                }

                // بناء قائمة الدورات
                string? completedCoursesText = null;
                if (updateDto.CompletedCourseIds != null && updateDto.CompletedCourseIds.Any())
                {
                    var courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
                    var selectedCourses = courses.Where(c => updateDto.CompletedCourseIds.Contains(c.CourseId))
                                                 .Select(c => c.CourseName);
                    completedCoursesText = string.Join(", ", selectedCourses);
                }

                hafiz.StudentName = updateDto.StudentName;
                hafiz.CenterId = updateDto.CenterId ?? 0;
                hafiz.CompletionYear = updateDto.CompletionYear;
                hafiz.CompletedCourses = completedCoursesText;
                hafiz.MemorizationLevel = updateDto.MemorizationLevel;
                hafiz.IsActive = updateDto.IsActive;
                hafiz.LastModifiedDate = DateTime.Now;

                // Handle New Certificate Upload
                if (updateDto.CertificateFile != null && updateDto.CertificateFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(hafiz.CertificatePath))
                    {
                        DeleteFile(hafiz.CertificatePath);
                    }

                    var certResult = await SaveFileAsync(updateDto.CertificateFile, "certificates");
                    if (certResult.Success)
                    {
                        hafiz.CertificatePath = certResult.FilePath;
                        hafiz.CertificateFileName = certResult.FileName;
                        hafiz.CertificateFileType = updateDto.CertificateFile.ContentType;
                        hafiz.CertificateFileSize = updateDto.CertificateFile.Length;
                    }
                }

                // Handle New Photo Upload
                if (updateDto.PhotoFile != null && updateDto.PhotoFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(hafiz.PhotoPath))
                    {
                        DeleteFile(hafiz.PhotoPath);
                    }

                    var photoResult = await SaveFileAsync(updateDto.PhotoFile, "photos");
                    if (photoResult.Success)
                    {
                        hafiz.PhotoPath = photoResult.FilePath;
                    }
                }

                await _unitOfWork.HafizRegistry.UpdateAsync(hafiz);
                await _unitOfWork.SaveChangesAsync();

                await _auditLogService.LogUpdateAsync(User, _userManager, HttpContext, "hafizregistry", hafiz.HafizId, hafiz, hafiz, hafiz.StudentName);

                TempData["Success"] = $"تم تحديث بيانات الحافظ '{hafiz.StudentName}' بنجاح";
                _logger.LogInformation("تم تحديث بيانات الحافظ: {StudentName}", hafiz.StudentName);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(updateDto);
        }

        // GET: HafizRegistry/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var hafiz = await _unitOfWork.HafizRegistry.GetHafizWithDetailsAsync(id);
            if (hafiz == null)
            {
                return NotFound();
            }

            var hafizDto = new HafizRegistryDto
            {
                HafizId = hafiz.HafizId,
                StudentName = hafiz.StudentName,
                CenterName = hafiz.Center?.Name,
                CompletionYear = hafiz.CompletionYear,
                CompletedCourses = hafiz.CompletedCourses,
                MemorizationLevel = hafiz.MemorizationLevel
            };

            return View(hafizDto);
        }

        // POST: HafizRegistry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hafiz = await _unitOfWork.HafizRegistry.GetByIdAsync(id);
            if (hafiz != null)
            {
                await _auditLogService.LogDeleteAsync(User, _userManager, HttpContext, "hafizregistry", hafiz.HafizId, hafiz, hafiz.StudentName);
                
                // Delete Files
                if (!string.IsNullOrEmpty(hafiz.CertificatePath))
                {
                    DeleteFile(hafiz.CertificatePath);
                }
                if (!string.IsNullOrEmpty(hafiz.PhotoPath))
                {
                    DeleteFile(hafiz.PhotoPath);
                }

                await _unitOfWork.HafizRegistry.DeleteAsync(hafiz);
                await _unitOfWork.SaveChangesAsync();

                TempData["Success"] = $"تم حذف الحافظ '{hafiz.StudentName}' من ديوان الحفاظ بنجاح";
                _logger.LogInformation("تم حذف الحافظ: {StudentName}", hafiz.StudentName);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: HafizRegistry/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var hafiz = await _unitOfWork.HafizRegistry.GetByIdAsync(id);
            if (hafiz == null || string.IsNullOrEmpty(hafiz.CertificatePath))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, hafiz.CertificatePath);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", hafiz.CertificateFileName);
        }

        // GET: HafizRegistry/ExportToExcel
        public async Task<IActionResult> ExportToExcel(string? searchTerm, int? centerId)
        {
            var hafazList = await _unitOfWork.HafizRegistry.GetActiveHafazAsync();

            // تطبيق الفلترة
            if (centerId.HasValue && centerId.Value > 0)
            {
                hafazList = hafazList.Where(h => h.CenterId == centerId.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                hafazList = hafazList.Where(h => 
                    h.StudentName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (h.Center != null && h.Center.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("ديوان الحفاظ");

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "اسم الطالب";
            worksheet.Cell(1, 3).Value = "المركز";
            worksheet.Cell(1, 4).Value = "سنة الإتمام";
            worksheet.Cell(1, 5).Value = "الدورات المكتملة";
            worksheet.Cell(1, 6).Value = "مستوى الحفظ";
            worksheet.Cell(1, 7).Value = "الشهادة";

            // Styling Header
            var headerRange = worksheet.Range(1, 1, 1, 7);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e7d32");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
            int row = 2;
            int index = 1;
            foreach (var hafiz in hafazList)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = hafiz.StudentName;
                worksheet.Cell(row, 3).Value = hafiz.Center?.Name ?? "-";
                worksheet.Cell(row, 4).Value = hafiz.CompletionYear;
                worksheet.Cell(row, 5).Value = hafiz.CompletedCourses ?? "-";
                worksheet.Cell(row, 6).Value = hafiz.MemorizationLevel.HasValue ? hafiz.MemorizationLevel.Value.GetDisplayName() : "-";
                worksheet.Cell(row, 7).Value = !string.IsNullOrEmpty(hafiz.CertificateFileName) ? "نعم" : "لا";
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"ديوان_الحفاظ_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #region Private Methods

        private async Task<(bool Success, string FilePath, string FileName, string ErrorMessage)> SaveFileAsync(IFormFile file, string subfolder)
        {
            try
            {
                // Validate file size (10MB max)
                if (file.Length > 10 * 1024 * 1024)
                {
                    return (false, "", "", "حجم الملف يجب أن لا يتجاوز 10 ميجابايت");
                }

                // Create upload directory
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "hafiz", subfolder);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique file name
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                var relativePath = Path.Combine("uploads", "hafiz", subfolder, fileName).Replace("\\", "/");

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return (true, relativePath, file.FileName, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving file: {FileName}", file.FileName);
                return (false, "", "", "حدث خطأ أثناء حفظ الملف");
            }
        }

        private void DeleteFile(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        #endregion
    }
}

