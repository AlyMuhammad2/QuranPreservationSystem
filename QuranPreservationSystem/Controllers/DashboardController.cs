using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            ILogger<DashboardController> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// الصفحة الرئيسية للوحة التحكم
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // الحصول على المستخدم الحالي
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // الحصول على الإحصائيات
            var stats = new
            {
                TotalCenters = await _unitOfWork.Centers.CountAsync(),
                TotalTeachers = await _unitOfWork.Teachers.CountAsync(),
                TotalStudents = await _unitOfWork.Students.CountAsync(),
                TotalCourses = await _unitOfWork.Courses.CountAsync()
            };

            ViewBag.Stats = stats;
            ViewBag.UserName = user.FullName;
            ViewBag.UserRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "User";

            return View();
        }
    }
}

