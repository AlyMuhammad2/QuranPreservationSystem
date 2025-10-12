using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;

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
            var stats = new
            {
                TotalCenters = await _unitOfWork.Centers.CountAsync(),
                ActiveCenters = await _unitOfWork.Centers.CountAsync(c => c.IsActive),
                TotalTeachers = await _unitOfWork.Teachers.CountAsync(),
                ActiveTeachers = await _unitOfWork.Teachers.CountAsync(t => t.IsActive),
                TotalStudents = await _unitOfWork.Students.CountAsync(),
                ActiveStudents = await _unitOfWork.Students.CountAsync(s => s.IsActive),
                TotalCourses = await _unitOfWork.Courses.CountAsync(),
                ActiveCourses = await _unitOfWork.Courses.CountAsync(c => c.IsActive),
                TotalEnrollments = await _unitOfWork.StudentCourses.CountAsync()
            };

            ViewBag.Stats = stats;
            return View();
        }

        // GET: Reports/Export
        public IActionResult Export()
        {
            return View();
        }
    }
}

