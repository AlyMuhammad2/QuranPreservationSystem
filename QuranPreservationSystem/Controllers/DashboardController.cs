using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// Dashboard Controller - لوحة التحكم الرئيسية
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IAuditLogService auditLogService,
            ILogger<DashboardController> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _auditLogService = auditLogService;
            _logger = logger;
        }

        /// <summary>
        /// الصفحة الرئيسية للوحة التحكم
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // الحصول على المستخدم الحالي
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // الإحصائيات الرئيسية
                var totalCenters = await _unitOfWork.Centers.CountAsync();
                var totalTeachers = await _unitOfWork.Teachers.CountAsync();
                var totalStudents = await _unitOfWork.Students.CountAsync();
                var totalCourses = await _unitOfWork.Courses.CountAsync();

                // إحصائيات متقدمة
                var activeCourses = await _unitOfWork.Courses.CountAsync(c => c.IsActive);
                var activeStudents = await _unitOfWork.Students.CountAsync(s => s.IsActive);
                var activeTeachers = await _unitOfWork.Teachers.CountAsync(t => t.IsActive);

                // الدورات النشطة (الجارية حالياً)
                var now = DateTime.Now;
                var allCourses = await _unitOfWork.Courses.GetAllAsync();
                var ongoingCourses = allCourses
                    .Where(c => c.IsActive && c.StartDate <= now && (!c.EndDate.HasValue || c.EndDate >= now))
                    .Take(5)
                    .ToList();

                // آخر الطلاب المسجلين
                var allStudents = await _unitOfWork.Students.GetAllAsync();
                var recentStudents = allStudents
                    .OrderByDescending(s => s.EnrollmentDate)
                    .Take(5)
                    .ToList();

                // ديوان الحفاظ - آخر المسجلين
                var allHafizes = await _unitOfWork.HafizRegistry.GetAllAsync();
                var recentHafizes = allHafizes
                    .OrderByDescending(h => h.CreatedDate)
                    .Take(5)
                    .ToList();

                // إحصائيات ديوان الحفاظ
                var totalHafizes = await _unitOfWork.HafizRegistry.CountAsync();

                // إحصائيات الاختبارات
                var totalExams = await _unitOfWork.Exams.CountAsync();
                var activeExams = await _unitOfWork.Exams.CountAsync(e => e.IsActive);

                // إحصائيات التسجيلات
                var totalEnrollments = await _unitOfWork.StudentCourses.CountAsync();

                // إحصائيات شهرية (الشهر الحالي)
                var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
                var studentsThisMonth = await _unitOfWork.Students.CountAsync(s => s.EnrollmentDate >= firstDayOfMonth);
                var coursesThisMonth = await _unitOfWork.Courses.CountAsync(c => c.CreatedDate >= firstDayOfMonth);

                // آخر النشاطات من Logs
                var recentLogs = new List<dynamic>();
                try
                {
                    var studentLogs = await _auditLogService.GetAllStudentLogsAsync(null, null, 1, 3);
                    var teacherLogs = await _auditLogService.GetAllTeacherLogsAsync(null, null, 1, 2);
                    
                    foreach (var log in studentLogs.Take(3))
                    {
                        recentLogs.Add(new
                        {
                            Type = "student",
                            Action = log.ActionType.ToString(),
                            Name = log.StudentName,
                            UserName = log.UserName,
                            Timestamp = log.Timestamp
                        });
                    }

                    foreach (var log in teacherLogs.Take(2))
                    {
                        recentLogs.Add(new
                        {
                            Type = "teacher",
                            Action = log.ActionType.ToString(),
                            Name = log.TeacherName,
                            UserName = log.UserName,
                            Timestamp = log.Timestamp
                        });
                    }

                    recentLogs = recentLogs.OrderByDescending(l => l.Timestamp).Take(5).ToList();
                }
                catch
                {
                    // في حالة عدم وجود logs
                    recentLogs = new List<dynamic>();
                }

                // تمرير البيانات للـ View
                ViewBag.Stats = new
                {
                    TotalCenters = totalCenters,
                    TotalTeachers = totalTeachers,
                    ActiveTeachers = activeTeachers,
                    TotalStudents = totalStudents,
                    ActiveStudents = activeStudents,
                    TotalCourses = totalCourses,
                    ActiveCourses = activeCourses,
                    TotalHafizes = totalHafizes,
                    TotalExams = totalExams,
                    ActiveExams = activeExams,
                    TotalEnrollments = totalEnrollments,
                    StudentsThisMonth = studentsThisMonth,
                    CoursesThisMonth = coursesThisMonth
                };

                ViewBag.UserName = user.FullName;
                ViewBag.UserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User";
                ViewBag.OngoingCourses = ongoingCourses;
                ViewBag.RecentStudents = recentStudents;
                ViewBag.RecentHafizes = recentHafizes;
                ViewBag.RecentLogs = recentLogs;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحميل لوحة التحكم");
                TempData["Error"] = "حدث خطأ في تحميل لوحة التحكم";
                return View();
            }
        }
    }
}
