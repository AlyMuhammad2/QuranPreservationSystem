using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Authorization;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Helpers;
using QuranPreservationSystem.Infrastructure.Identity;
using ClosedXML.Excel;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// الاختبارات - إدارة اختبارات التجويد
    /// </summary>
    [Authorize]
    [PermissionAuthorize("Exams", "View")]
    public class ExamsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExamsController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuditLogService _auditLogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExamsController(
            IUnitOfWork unitOfWork, 
            ILogger<ExamsController> logger, 
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

        // GET: Exams
        public async Task<IActionResult> Index(string searchTerm, int? centerId)
        {
            var exams = await _unitOfWork.Exams.GetActiveExamsAsync();
            
            // تطبيق الفلترة حسب المركز
            if (centerId.HasValue && centerId.Value > 0)
            {
                exams = exams.Where(e => e.CenterId == centerId.Value).ToList();
            }

            // تطبيق البحث بالاسم
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                exams = exams.Where(e => 
                    e.ExamName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (e.Description != null && e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            var examDtos = exams.Select(e => new ExamDto
            {
                ExamId = e.ExamId,
                ExamName = e.ExamName,
                Description = e.Description,
                ExamType = e.ExamType,
                Level = e.Level,
                TotalMarks = e.TotalMarks,
                PassingMarks = e.PassingMarks,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                Location = e.Location,
                Instructions = e.Instructions,
                PdfFileName = e.PdfFileName,
                FileSizeFormatted = e.PdfFileSize.HasValue ? FormatFileSize(e.PdfFileSize.Value) : null,
                CenterId = e.CenterId,
                CenterName = e.Center?.Name,
                CourseId = e.CourseId,
                CourseName = e.Course?.CourseName,
                Notes = e.Notes,
                IsActive = e.IsActive,
                CreatedDate = e.CreatedDate,
                LastModifiedDate = e.LastModifiedDate
            }).ToList();

            // إرسال قائمة المراكز للفلتر
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCenterId = centerId;

            return View(examDtos);
        }

        // GET: Exams/ExportToExcel
        public async Task<IActionResult> ExportToExcel(string searchTerm, int? centerId)
        {
            var exams = await _unitOfWork.Exams.GetActiveExamsAsync();
            
            // تطبيق الفلترة
            if (centerId.HasValue && centerId.Value > 0)
            {
                exams = exams.Where(e => e.CenterId == centerId.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                exams = exams.Where(e => 
                    e.ExamName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (e.Description != null && e.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("الاختبارات");

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "اسم الاختبار";
            worksheet.Cell(1, 3).Value = "المركز";
            worksheet.Cell(1, 4).Value = "الدورة";
            worksheet.Cell(1, 5).Value = "النوع";
            worksheet.Cell(1, 6).Value = "المستوى";
            worksheet.Cell(1, 7).Value = "الدرجة الكاملة";
            worksheet.Cell(1, 8).Value = "درجة النجاح";
            worksheet.Cell(1, 9).Value = "المكان";
            worksheet.Cell(1, 10).Value = "الحالة";

            // Styling Header
            var headerRange = worksheet.Range(1, 1, 1, 10);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e7d32");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
            int row = 2;
            int index = 1;
            foreach (var exam in exams)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = exam.ExamName;
                worksheet.Cell(row, 3).Value = exam.Center?.Name ?? "-";
                worksheet.Cell(row, 4).Value = exam.Course?.CourseName ?? "-";
                worksheet.Cell(row, 5).Value = exam.ExamType ?? "-";
                worksheet.Cell(row, 6).Value = exam.Level ?? "-";
                worksheet.Cell(row, 7).Value = exam.TotalMarks?.ToString() ?? "-";
                worksheet.Cell(row, 8).Value = exam.PassingMarks?.ToString() ?? "-";
                worksheet.Cell(row, 9).Value = exam.Location ?? "-";
                worksheet.Cell(row, 10).Value = exam.IsActive ? "نشط" : "غير نشط";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"الاختبارات_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // GET: Exams/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var exam = await _unitOfWork.Exams.GetExamWithDetailsAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var examDto = new ExamDto
            {
                ExamId = exam.ExamId,
                ExamName = exam.ExamName,
                Description = exam.Description,
                ExamType = exam.ExamType,
                Level = exam.Level,
                TotalMarks = exam.TotalMarks,
                PassingMarks = exam.PassingMarks,
                StartTime = exam.StartTime,
                EndTime = exam.EndTime,
                Location = exam.Location,
                Instructions = exam.Instructions,
                PdfFileName = exam.PdfFileName,
                FileSizeFormatted = exam.PdfFileSize.HasValue ? FormatFileSize(exam.PdfFileSize.Value) : null,
                CenterId = exam.CenterId,
                CenterName = exam.Center?.Name,
                CourseId = exam.CourseId,
                CourseName = exam.Course?.CourseName,
                Notes = exam.Notes,
                IsActive = exam.IsActive,
                CreatedDate = exam.CreatedDate,
                LastModifiedDate = exam.LastModifiedDate
            };

            return View(examDto);
        }

        // GET: Exams/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View();
        }

        // POST: Exams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateExamDto createExamDto)
        {
            if (ModelState.IsValid)
            {
                var exam = new Domain.Entities.Exam
                {
                    ExamName = createExamDto.ExamName,
                    Description = createExamDto.Description,
                    ExamType = createExamDto.ExamType,
                    Level = createExamDto.Level,
                    TotalMarks = createExamDto.TotalMarks,
                    PassingMarks = createExamDto.PassingMarks,
                    StartTime = createExamDto.StartTime,
                    EndTime = createExamDto.EndTime,
                    Location = createExamDto.Location,
                    Instructions = createExamDto.Instructions,
                    CenterId = createExamDto.CenterId,
                    CourseId = createExamDto.CourseId,
                    Notes = createExamDto.Notes,
                    IsActive = createExamDto.IsActive,
                    CreatedDate = DateTime.Now
                };

                // Handle PDF file upload
                if (createExamDto.PdfFile != null && createExamDto.PdfFile.Length > 0)
                {
                    var fileResult = await SaveFileAsync(createExamDto.PdfFile);
                    if (fileResult.Success)
                    {
                        exam.PdfFilePath = fileResult.FilePath;
                        exam.PdfFileName = fileResult.FileName;
                        exam.PdfFileType = createExamDto.PdfFile.ContentType;
                        exam.PdfFileSize = createExamDto.PdfFile.Length;
                    }
                    else
                    {
                        ModelState.AddModelError("PdfFile", fileResult.ErrorMessage);
                        ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
                        ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
                        return View(createExamDto);
                    }
                }

                await _unitOfWork.Exams.AddAsync(exam);
                await _unitOfWork.SaveChangesAsync();
                
                await _auditLogService.LogCreateAsync(User, _userManager, HttpContext, "exam", exam.ExamId, exam, exam.ExamName);
                
                TempData["Success"] = $"تم إضافة الاختبار '{exam.ExamName}' بنجاح";
                _logger.LogInformation("تم إنشاء اختبار جديد: {ExamId} - {ExamName}", exam.ExamId, exam.ExamName);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(createExamDto);
        }

        // GET: Exams/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var exam = await _unitOfWork.Exams.GetExamWithDetailsAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var updateExamDto = new UpdateExamDto
            {
                ExamId = exam.ExamId,
                ExamName = exam.ExamName,
                Description = exam.Description,
                ExamType = exam.ExamType,
                Level = exam.Level,
                TotalMarks = exam.TotalMarks,
                PassingMarks = exam.PassingMarks,
                StartTime = exam.StartTime,
                EndTime = exam.EndTime,
                Location = exam.Location,
                Instructions = exam.Instructions,
                CenterId = exam.CenterId,
                CourseId = exam.CourseId,
                CurrentPdfFileName = exam.PdfFileName,
                Notes = exam.Notes,
                IsActive = exam.IsActive
            };

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(updateExamDto);
        }

        // POST: Exams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateExamDto updateExamDto)
        {
            if (id != updateExamDto.ExamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var exam = await _unitOfWork.Exams.GetExamWithDetailsAsync(id);
                if (exam == null)
                {
                    return NotFound();
                }

                var oldExam = new Exam { ExamId = exam.ExamId, ExamName = exam.ExamName, ExamType = exam.ExamType, Level = exam.Level, IsActive = exam.IsActive };

                exam.ExamName = updateExamDto.ExamName;
                exam.Description = updateExamDto.Description;
                exam.ExamType = updateExamDto.ExamType;
                exam.Level = updateExamDto.Level;
                exam.TotalMarks = updateExamDto.TotalMarks;
                exam.PassingMarks = updateExamDto.PassingMarks;
                exam.StartTime = updateExamDto.StartTime;
                exam.EndTime = updateExamDto.EndTime;
                exam.Location = updateExamDto.Location;
                exam.Instructions = updateExamDto.Instructions;
                exam.CenterId = updateExamDto.CenterId;
                exam.CourseId = updateExamDto.CourseId;
                exam.Notes = updateExamDto.Notes;
                exam.IsActive = updateExamDto.IsActive;
                exam.LastModifiedDate = DateTime.Now;

                // Handle new PDF file upload
                if (updateExamDto.PdfFile != null && updateExamDto.PdfFile.Length > 0)
                {
                    // Delete old file if exists
                    if (!string.IsNullOrEmpty(exam.PdfFilePath))
                    {
                        DeleteFile(exam.PdfFilePath);
                    }

                    var fileResult = await SaveFileAsync(updateExamDto.PdfFile);
                    if (fileResult.Success)
                    {
                        exam.PdfFilePath = fileResult.FilePath;
                        exam.PdfFileName = fileResult.FileName;
                        exam.PdfFileType = updateExamDto.PdfFile.ContentType;
                        exam.PdfFileSize = updateExamDto.PdfFile.Length;
                    }
                    else
                    {
                        ModelState.AddModelError("PdfFile", fileResult.ErrorMessage);
                        ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
                        ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
                        return View(updateExamDto);
                    }
                }

                await _unitOfWork.Exams.UpdateAsync(exam);
                await _unitOfWork.SaveChangesAsync();
                
                await _auditLogService.LogUpdateAsync(User, _userManager, HttpContext, "exam", exam.ExamId, oldExam, exam, exam.ExamName);
                
                TempData["Success"] = $"تم تحديث الاختبار '{exam.ExamName}' بنجاح";
                _logger.LogInformation("تم تحديث اختبار: {ExamId} - {ExamName}", exam.ExamId, exam.ExamName);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(updateExamDto);
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _unitOfWork.Exams.GetExamWithDetailsAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var examDto = new ExamDto
            {
                ExamId = exam.ExamId,
                ExamName = exam.ExamName,
                Description = exam.Description,
                ExamType = exam.ExamType,
                Level = exam.Level,
                TotalMarks = exam.TotalMarks,
                PassingMarks = exam.PassingMarks,
                StartTime = exam.StartTime,
                EndTime = exam.EndTime,
                Location = exam.Location,
                Instructions = exam.Instructions,
                PdfFileName = exam.PdfFileName,
                FileSizeFormatted = exam.PdfFileSize.HasValue ? FormatFileSize(exam.PdfFileSize.Value) : null,
                CenterId = exam.CenterId,
                CenterName = exam.Center?.Name,
                CourseId = exam.CourseId,
                CourseName = exam.Course?.CourseName,
                Notes = exam.Notes,
                IsActive = exam.IsActive,
                CreatedDate = exam.CreatedDate,
                LastModifiedDate = exam.LastModifiedDate
            };

            return View(examDto);
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(id);
            if (exam != null)
            {
                await _auditLogService.LogDeleteAsync(User, _userManager, HttpContext, "exam", exam.ExamId, exam, exam.ExamName);
                
                // Delete PDF file if exists
                if (!string.IsNullOrEmpty(exam.PdfFilePath))
                {
                    DeleteFile(exam.PdfFilePath);
                }

                await _unitOfWork.Exams.DeleteAsync(exam);
                await _unitOfWork.SaveChangesAsync();
                
                TempData["Success"] = $"تم حذف الاختبار '{exam.ExamName}' بنجاح";
                _logger.LogInformation("تم حذف اختبار: {ExamId} - {ExamName}", exam.ExamId, exam.ExamName);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Exams/Download/5
        public async Task<IActionResult> Download(int id)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(id);
            if (exam == null || string.IsNullOrEmpty(exam.PdfFilePath))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, exam.PdfFilePath);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/pdf", exam.PdfFileName);
        }

        #region Private Methods

        private async Task<(bool Success, string FilePath, string FileName, string ErrorMessage)> SaveFileAsync(IFormFile file)
        {
            try
            {
                // Validate file type
                if (file.ContentType != "application/pdf")
                {
                    return (false, "", "", "يجب أن يكون الملف من نوع PDF");
                }

                // Validate file size (10MB max)
                if (file.Length > 10 * 1024 * 1024)
                {
                    return (false, "", "", "حجم الملف يجب أن لا يتجاوز 10 ميجابايت");
                }

                // Create upload directory
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "exams");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique file name
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                var relativePath = Path.Combine("uploads", "exams", fileName).Replace("\\", "/");

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