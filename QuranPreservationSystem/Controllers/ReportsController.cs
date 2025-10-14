using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Helpers;
using ClosedXML.Excel;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// التقارير والإحصائيات
    /// </summary>
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            IUnitOfWork unitOfWork,
            ILogger<ReportsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Reports
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reports/Statistics
        public async Task<IActionResult> Statistics()
        {
            var centers = await _unitOfWork.Centers.GetAllAsync();
            var teachers = await _unitOfWork.Teachers.GetAllAsync();
            var students = await _unitOfWork.Students.GetAllAsync();
            var courses = await _unitOfWork.Courses.GetAllAsync();
            var enrollments = await _unitOfWork.StudentCourses.GetAllAsync();
            var exams = await _unitOfWork.Exams.GetAllAsync();
            var hafaz = await _unitOfWork.HafizRegistry.GetAllAsync();

            var stats = new
            {
                // المراكز
                TotalCenters = centers.Count(),
                ActiveCenters = centers.Count(c => c.IsActive),
                
                // المدرسين
                TotalTeachers = teachers.Count(),
                ActiveTeachers = teachers.Count(t => t.IsActive),
                MaleTeachers = teachers.Count(t => t.Gender == Domain.Enums.Gender.Male),
                FemaleTeachers = teachers.Count(t => t.Gender == Domain.Enums.Gender.Female),
                
                // الطلاب
                TotalStudents = students.Count(),
                ActiveStudents = students.Count(s => s.IsActive),
                MaleStudents = students.Count(s => s.Gender == Domain.Enums.Gender.Male),
                FemaleStudents = students.Count(s => s.Gender == Domain.Enums.Gender.Female),
                
                // الدورات
                TotalCourses = courses.Count(),
                ActiveCourses = courses.Count(c => c.IsActive),
                TilawahCourses = courses.Count(c => c.CourseType == Domain.Enums.CourseType.TilawahAndTajweed),
                IntroductoryCourses = courses.Count(c => c.CourseType == Domain.Enums.CourseType.Introductory),
                IntermediateCourses = courses.Count(c => c.CourseType == Domain.Enums.CourseType.Intermediate),
                
                // التسجيلات
                TotalEnrollments = enrollments.Count(),
                ActiveEnrollments = enrollments.Count(e => e.Status == Domain.Enums.StudentCourseStatus.Active),
                CompletedEnrollments = enrollments.Count(e => e.Status == Domain.Enums.StudentCourseStatus.Completed),
                
                // الاختبارات
                TotalExams = exams.Count(),
                ActiveExams = exams.Count(e => e.IsActive),
                
                // ديوان الحفاظ
                TotalHafaz = hafaz.Count(),
                ActiveHafaz = hafaz.Count(h => h.IsActive),
                HafazWithCertificates = hafaz.Count(h => !string.IsNullOrEmpty(h.CertificateFileName)),
                CompleteHafaz = hafaz.Count(h => h.MemorizationLevel == Domain.Enums.MemorizationLevel.Complete),
                HalfHafaz = hafaz.Count(h => h.MemorizationLevel == Domain.Enums.MemorizationLevel.Half),
                QuarterHafaz = hafaz.Count(h => h.MemorizationLevel == Domain.Enums.MemorizationLevel.Quarter)
            };

            ViewBag.Stats = stats;
            return View();
        }

        // GET: Reports/Export
        public IActionResult Export()
        {
            return View();
        }

        #region Centers Reports

        // GET: Reports/CentersReport
        public async Task<IActionResult> CentersReport()
        {
            var centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            
            var centerStats = new List<dynamic>();
            foreach (var center in centers)
            {
                var teachers = await _unitOfWork.Teachers.FindAsync(t => t.CenterId == center.CenterId && t.IsActive);
                var students = await _unitOfWork.Students.FindAsync(s => s.CenterId == center.CenterId && s.IsActive);
                var courses = await _unitOfWork.Courses.FindAsync(c => c.CenterId == center.CenterId && c.IsActive);
                var hafaz = await _unitOfWork.HafizRegistry.FindAsync(h => h.CenterId == center.CenterId && h.IsActive);

                centerStats.Add(new
                {
                    CenterName = center.Name,
                    Address = center.Address,
                    PhoneNumber = center.PhoneNumber,
                    TeachersCount = teachers.Count(),
                    StudentsCount = students.Count(),
                    CoursesCount = courses.Count(),
                    HafazCount = hafaz.Count()
                });
            }

            ViewBag.CenterStats = centerStats;
            return View();
        }

        // GET: Reports/ExportCenters
        public async Task<IActionResult> ExportCenters()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("المراكز");

            var centers = await _unitOfWork.Centers.GetAllAsync();

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "اسم المركز";
            worksheet.Cell(1, 3).Value = "العنوان";
            worksheet.Cell(1, 4).Value = "الهاتف";
            worksheet.Cell(1, 5).Value = "عدد المدرسين";
            worksheet.Cell(1, 6).Value = "عدد الطلاب";
            worksheet.Cell(1, 7).Value = "عدد الدورات";
            worksheet.Cell(1, 8).Value = "عدد الحفاظ";
            worksheet.Cell(1, 9).Value = "الحالة";

            StyleHeader(worksheet.Range(1, 1, 1, 9));

            int row = 2;
            int index = 1;

            foreach (var center in centers)
            {
                var teachers = await _unitOfWork.Teachers.FindAsync(t => t.CenterId == center.CenterId && t.IsActive);
                var students = await _unitOfWork.Students.FindAsync(s => s.CenterId == center.CenterId && s.IsActive);
                var courses = await _unitOfWork.Courses.FindAsync(c => c.CenterId == center.CenterId && c.IsActive);
                var hafaz = await _unitOfWork.HafizRegistry.FindAsync(h => h.CenterId == center.CenterId && h.IsActive);

                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = center.Name;
                worksheet.Cell(row, 3).Value = center.Address ?? "-";
                worksheet.Cell(row, 4).Value = center.PhoneNumber ?? "-";
                worksheet.Cell(row, 5).Value = teachers.Count();
                worksheet.Cell(row, 6).Value = students.Count();
                worksheet.Cell(row, 7).Value = courses.Count();
                worksheet.Cell(row, 8).Value = hafaz.Count();
                worksheet.Cell(row, 9).Value = center.IsActive ? "نشط" : "غير نشط";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"تقرير_المراكز_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion

        #region Teachers Reports

        // GET: Reports/ExportTeachers
        public async Task<IActionResult> ExportTeachers()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("المدرسين");

            var teachers = await _unitOfWork.Teachers.GetAllAsync();

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "الاسم الكامل";
            worksheet.Cell(1, 3).Value = "المركز";
            worksheet.Cell(1, 4).Value = "الهاتف";
            worksheet.Cell(1, 5).Value = "البريد الإلكتروني";
            worksheet.Cell(1, 6).Value = "الجنس";
            worksheet.Cell(1, 7).Value = "التخصص";
            worksheet.Cell(1, 8).Value = "تاريخ التعيين";
            worksheet.Cell(1, 9).Value = "الحالة";

            StyleHeader(worksheet.Range(1, 1, 1, 9));

            int row = 2;
            int index = 1;

            foreach (var teacher in teachers)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = $"{teacher.FirstName} {teacher.LastName}";
                worksheet.Cell(row, 3).Value = teacher.Center?.Name ?? "-";
                worksheet.Cell(row, 4).Value = teacher.PhoneNumber;
                worksheet.Cell(row, 5).Value = teacher.Email ?? "-";
                worksheet.Cell(row, 6).Value = teacher.Gender.HasValue ? teacher.Gender.Value.GetDisplayName() : "-";
                worksheet.Cell(row, 7).Value = teacher.Specialization ?? "-";
                worksheet.Cell(row, 8).Value = teacher.HireDate.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 9).Value = teacher.IsActive ? "نشط" : "غير نشط";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"تقرير_المدرسين_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion

        #region Students Reports

        // GET: Reports/ExportStudents
        public async Task<IActionResult> ExportStudents()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("الطلاب");

            var students = await _unitOfWork.Students.GetAllAsync();

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "الاسم الكامل";
            worksheet.Cell(1, 3).Value = "المركز";
            worksheet.Cell(1, 4).Value = "الهاتف";
            worksheet.Cell(1, 5).Value = "البريد الإلكتروني";
            worksheet.Cell(1, 6).Value = "الجنس";
            worksheet.Cell(1, 7).Value = "تاريخ الميلاد";
            worksheet.Cell(1, 8).Value = "تاريخ التسجيل";
            worksheet.Cell(1, 9).Value = "الحالة";

            StyleHeader(worksheet.Range(1, 1, 1, 9));

            int row = 2;
            int index = 1;

            foreach (var student in students)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = $"{student.FirstName} {student.LastName}";
                worksheet.Cell(row, 3).Value = student.Center?.Name ?? "-";
                worksheet.Cell(row, 4).Value = student.PhoneNumber;
                worksheet.Cell(row, 5).Value = student.Email ?? "-";
                worksheet.Cell(row, 6).Value = student.Gender.GetDisplayName();
                worksheet.Cell(row, 7).Value = student.DateOfBirth.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 8).Value = student.EnrollmentDate.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 9).Value = student.IsActive ? "نشط" : "غير نشط";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"تقرير_الطلاب_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion

        #region Courses Reports

        // GET: Reports/ExportCourses
        public async Task<IActionResult> ExportCourses()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("الدورات");

            var courses = await _unitOfWork.Courses.GetActiveCoursesAsync();

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "اسم الدورة";
            worksheet.Cell(1, 3).Value = "النوع";
            worksheet.Cell(1, 4).Value = "المركز";
            worksheet.Cell(1, 5).Value = "المدرس";
            worksheet.Cell(1, 6).Value = "تاريخ البداية";
            worksheet.Cell(1, 7).Value = "تاريخ النهاية";
            worksheet.Cell(1, 8).Value = "المدة (ساعات)";
            worksheet.Cell(1, 9).Value = "الحالة";

            StyleHeader(worksheet.Range(1, 1, 1, 9));

            int row = 2;
            int index = 1;

            foreach (var course in courses)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = course.CourseName;
                worksheet.Cell(row, 3).Value = course.CourseType.GetDisplayName();
                worksheet.Cell(row, 4).Value = course.Center?.Name ?? "-";
                worksheet.Cell(row, 5).Value = course.Teacher != null ? $"{course.Teacher.FirstName} {course.Teacher.LastName}" : "-";
                worksheet.Cell(row, 6).Value = course.StartDate.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 7).Value = course.EndDate?.ToString("dd/MM/yyyy") ?? "-";
                worksheet.Cell(row, 8).Value = course.DurationHours?.ToString() ?? "-";
                worksheet.Cell(row, 9).Value = course.IsActive ? "نشط" : "غير نشط";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"تقرير_الدورات_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion

        #region Enrollments Reports

        // GET: Reports/ExportEnrollments
        public async Task<IActionResult> ExportEnrollments()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("التسجيلات");

            var enrollments = await _unitOfWork.StudentCourses.GetAllAsync();

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

            StyleHeader(worksheet.Range(1, 1, 1, 9));

            int row = 2;
            int index = 1;

            foreach (var enrollment in enrollments)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = enrollment.Student != null ? $"{enrollment.Student.FirstName} {enrollment.Student.LastName}" : "-";
                worksheet.Cell(row, 3).Value = enrollment.Course?.CourseName ?? "-";
                worksheet.Cell(row, 4).Value = enrollment.Course?.Teacher != null ? $"{enrollment.Course.Teacher.FirstName} {enrollment.Course.Teacher.LastName}" : "-";
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

            var fileName = $"تقرير_التسجيلات_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion

        #region Exams Reports

        // GET: Reports/ExportExams
        public async Task<IActionResult> ExportExams()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("الاختبارات");

            var exams = await _unitOfWork.Exams.GetActiveExamsAsync();

            // Header
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "اسم الاختبار";
            worksheet.Cell(1, 3).Value = "المركز";
            worksheet.Cell(1, 4).Value = "الدورة";
            worksheet.Cell(1, 5).Value = "النوع";
            worksheet.Cell(1, 6).Value = "المستوى";
            worksheet.Cell(1, 7).Value = "الدرجة الكاملة";
            worksheet.Cell(1, 8).Value = "درجة النجاح";
            worksheet.Cell(1, 9).Value = "الحالة";

            StyleHeader(worksheet.Range(1, 1, 1, 9));

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
                worksheet.Cell(row, 9).Value = exam.IsActive ? "نشط" : "غير نشط";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            var fileName = $"تقرير_الاختبارات_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        #endregion

        #region Private Helper Methods

        private void StyleHeader(IXLRange headerRange)
        {
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e7d32");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
        }

        #endregion
    }
}
