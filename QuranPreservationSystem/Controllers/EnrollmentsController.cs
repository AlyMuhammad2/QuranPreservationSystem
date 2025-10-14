using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Helpers;
using ClosedXML.Excel;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// التسجيلات - إدارة تسجيل الطلاب في الدورات
    /// </summary>
    [Authorize]
    public class EnrollmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EnrollmentsController> _logger;

        public EnrollmentsController(
            IUnitOfWork unitOfWork,
            ILogger<EnrollmentsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index(string searchTerm, int? centerId)
        {
            var enrollments = await _unitOfWork.StudentCourses.GetAllWithDetailsAsync();
            
            // تطبيق الفلترة حسب المركز
            if (centerId.HasValue && centerId.Value > 0)
            {
                enrollments = enrollments.Where(e => e.Course?.CenterId == centerId.Value).ToList();
            }

            // تطبيق البحث بالاسم
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                enrollments = enrollments.Where(e => 
                    (e.Student != null && (
                        e.Student.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        e.Student.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    )) ||
                    (e.Course != null && e.Course.CourseName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            var enrollmentDtos = enrollments.Select(e => new EnrollmentDto
            {
                StudentCourseId = e.StudentCourseId,
                StudentId = e.StudentId,
                StudentName = e.Student?.FullName,
                CourseId = e.CourseId,
                CourseName = e.Course?.CourseName,
                CourseType = e.Course?.CourseType,
                TeacherName = e.Course?.Teacher?.FullName,
                CenterName = e.Course?.Center?.Name,
                EnrollmentDate = e.EnrollmentDate,
                ExamDate = e.ExamDate,
                Status = e.Status,
                Grade = e.Grade,
                AttendancePercentage = e.AttendancePercentage,
                Examiner1 = e.Examiner1,
                Examiner2 = e.Examiner2,
                CompletionDate = e.CompletionDate,
                Notes = e.Notes,
                IsActive = e.IsActive
            }).ToList();

            // إرسال قائمة المراكز للفلتر
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCenterId = centerId;

            return View(enrollmentDtos);
        }

        // GET: Enrollments/ExportToExcel
        public async Task<IActionResult> ExportToExcel(string searchTerm, int? centerId)
        {
            var enrollments = await _unitOfWork.StudentCourses.GetAllWithDetailsAsync();
            
            // تطبيق الفلترة
            if (centerId.HasValue && centerId.Value > 0)
            {
                enrollments = enrollments.Where(e => e.Course?.CenterId == centerId.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                enrollments = enrollments.Where(e => 
                    (e.Student != null && (
                        e.Student.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        e.Student.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                    )) ||
                    (e.Course != null && e.Course.CourseName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("التسجيلات");

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "اسم الطالب";
            worksheet.Cell(1, 3).Value = "الدورة";
            worksheet.Cell(1, 4).Value = "المدرس";
            worksheet.Cell(1, 5).Value = "المركز";
            worksheet.Cell(1, 6).Value = "تاريخ التسجيل";
            worksheet.Cell(1, 7).Value = "الحالة";
            worksheet.Cell(1, 8).Value = "الدرجة";
            worksheet.Cell(1, 9).Value = "نسبة الحضور";

            // Styling Header
            var headerRange = worksheet.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e7d32");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
            int row = 2;
            int index = 1;
            foreach (var enrollment in enrollments)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = enrollment.Student?.FullName ?? "-";
                worksheet.Cell(row, 3).Value = enrollment.Course?.CourseName ?? "-";
                worksheet.Cell(row, 4).Value = enrollment.Course?.Teacher?.FullName ?? "-";
                worksheet.Cell(row, 5).Value = enrollment.Course?.Center?.Name ?? "-";
                worksheet.Cell(row, 6).Value = enrollment.EnrollmentDate.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 7).Value = enrollment.Status.GetDisplayName();
                worksheet.Cell(row, 8).Value = enrollment.Grade?.ToString() ?? "-";
                worksheet.Cell(row, 9).Value = enrollment.AttendancePercentage?.ToString() ?? "-";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"التسجيلات_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var enrollment = await _unitOfWork.StudentCourses.GetStudentCourseWithDetailsAsync(id);
            
            if (enrollment == null)
            {
                return NotFound();
            }

            var enrollmentDto = new EnrollmentDto
            {
                StudentCourseId = enrollment.StudentCourseId,
                StudentId = enrollment.StudentId,
                StudentName = enrollment.Student?.FullName,
                CourseId = enrollment.CourseId,
                CourseName = enrollment.Course?.CourseName,
                CourseType = enrollment.Course?.CourseType,
                TeacherName = enrollment.Course?.Teacher?.FullName,
                CenterName = enrollment.Course?.Center?.Name,
                EnrollmentDate = enrollment.EnrollmentDate,
                ExamDate = enrollment.ExamDate,
                Status = enrollment.Status,
                Grade = enrollment.Grade,
                AttendancePercentage = enrollment.AttendancePercentage,
                Examiner1 = enrollment.Examiner1,
                Examiner2 = enrollment.Examiner2,
                CompletionDate = enrollment.CompletionDate,
                Notes = enrollment.Notes,
                IsActive = enrollment.IsActive
            };

            return View(enrollmentDto);
        }

        // GET: Enrollments/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Students = await _unitOfWork.Students.GetActiveStudentsAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEnrollmentDto createEnrollmentDto)
        {
            if (ModelState.IsValid)
            {
                // التحقق من عدم تسجيل الطالب مرتين في نفس الدورة
                var exists = await _unitOfWork.StudentCourses.IsStudentEnrolledAsync(
                    createEnrollmentDto.StudentId, 
                    createEnrollmentDto.CourseId);

                if (exists)
                {
                    ModelState.AddModelError("", "الطالب مسجل بالفعل في هذه الدورة");
                    ViewBag.Students = await _unitOfWork.Students.GetActiveStudentsAsync();
                    ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
                    return View(createEnrollmentDto);
                }

                var enrollment = new Domain.Entities.StudentCourse
                {
                    StudentId = createEnrollmentDto.StudentId,
                    CourseId = createEnrollmentDto.CourseId,
                    EnrollmentDate = createEnrollmentDto.EnrollmentDate,
                    ExamDate = createEnrollmentDto.ExamDate,
                    Status = createEnrollmentDto.Status,
                    Grade = createEnrollmentDto.Grade,
                    AttendancePercentage = createEnrollmentDto.AttendancePercentage,
                    Examiner1 = createEnrollmentDto.Examiner1,
                    Examiner2 = createEnrollmentDto.Examiner2,
                    CompletionDate = createEnrollmentDto.CompletionDate,
                    Notes = createEnrollmentDto.Notes,
                    IsActive = createEnrollmentDto.IsActive
                };

                await _unitOfWork.StudentCourses.AddAsync(enrollment);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تسجيل طالب {StudentId} في دورة {CourseId}", 
                    createEnrollmentDto.StudentId, createEnrollmentDto.CourseId);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Students = await _unitOfWork.Students.GetActiveStudentsAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(createEnrollmentDto);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var enrollment = await _unitOfWork.StudentCourses.GetStudentCourseWithDetailsAsync(id);
            
            if (enrollment == null)
            {
                return NotFound();
            }

            var updateEnrollmentDto = new UpdateEnrollmentDto
            {
                StudentCourseId = enrollment.StudentCourseId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                EnrollmentDate = enrollment.EnrollmentDate,
                ExamDate = enrollment.ExamDate,
                Status = enrollment.Status,
                Grade = enrollment.Grade,
                AttendancePercentage = enrollment.AttendancePercentage,
                Examiner1 = enrollment.Examiner1,
                Examiner2 = enrollment.Examiner2,
                CompletionDate = enrollment.CompletionDate,
                Notes = enrollment.Notes,
                IsActive = enrollment.IsActive
            };

            ViewBag.StudentName = enrollment.Student?.FullName;
            ViewBag.CourseName = enrollment.Course?.CourseName;
            return View(updateEnrollmentDto);
        }

        // POST: Enrollments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEnrollmentDto updateEnrollmentDto)
        {
            if (id != updateEnrollmentDto.StudentCourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var enrollment = await _unitOfWork.StudentCourses.GetStudentCourseWithDetailsAsync(id);
                if (enrollment == null)
                {
                    return NotFound();
                }

                enrollment.EnrollmentDate = updateEnrollmentDto.EnrollmentDate;
                enrollment.ExamDate = updateEnrollmentDto.ExamDate;
                enrollment.Status = updateEnrollmentDto.Status;
                enrollment.Grade = updateEnrollmentDto.Grade;
                enrollment.AttendancePercentage = updateEnrollmentDto.AttendancePercentage;
                enrollment.Examiner1 = updateEnrollmentDto.Examiner1;
                enrollment.Examiner2 = updateEnrollmentDto.Examiner2;
                enrollment.CompletionDate = updateEnrollmentDto.CompletionDate;
                enrollment.Notes = updateEnrollmentDto.Notes;
                enrollment.IsActive = updateEnrollmentDto.IsActive;

                await _unitOfWork.StudentCourses.UpdateAsync(enrollment);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تحديث تسجيل الطالب {StudentId} في دورة {CourseId}", 
                    enrollment.StudentId, enrollment.CourseId);
                
                return RedirectToAction(nameof(Index));
            }

            var enrollmentForView = await _unitOfWork.StudentCourses.GetStudentCourseWithDetailsAsync(id);
            ViewBag.StudentName = enrollmentForView?.Student?.FullName;
            ViewBag.CourseName = enrollmentForView?.Course?.CourseName;
            return View(updateEnrollmentDto);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _unitOfWork.StudentCourses.GetStudentCourseWithDetailsAsync(id);
            
            if (enrollment == null)
            {
                return NotFound();
            }

            var enrollmentDto = new EnrollmentDto
            {
                StudentCourseId = enrollment.StudentCourseId,
                StudentId = enrollment.StudentId,
                StudentName = enrollment.Student?.FullName,
                CourseId = enrollment.CourseId,
                CourseName = enrollment.Course?.CourseName,
                CourseType = enrollment.Course?.CourseType,
                TeacherName = enrollment.Course?.Teacher?.FullName,
                CenterName = enrollment.Course?.Center?.Name,
                EnrollmentDate = enrollment.EnrollmentDate,
                ExamDate = enrollment.ExamDate,
                Status = enrollment.Status,
                Grade = enrollment.Grade,
                AttendancePercentage = enrollment.AttendancePercentage,
                Examiner1 = enrollment.Examiner1,
                Examiner2 = enrollment.Examiner2,
                CompletionDate = enrollment.CompletionDate,
                Notes = enrollment.Notes,
                IsActive = enrollment.IsActive
            };

            return View(enrollmentDto);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _unitOfWork.StudentCourses.GetStudentCourseWithDetailsAsync(id);
            
            if (enrollment != null)
            {
                await _unitOfWork.StudentCourses.DeleteAsync(enrollment);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم حذف تسجيل الطالب {StudentId} من دورة {CourseId}", 
                    enrollment.StudentId, enrollment.CourseId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
