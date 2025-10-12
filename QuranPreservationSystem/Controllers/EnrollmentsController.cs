using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;

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
        public async Task<IActionResult> Index()
        {
            var enrollments = await _unitOfWork.StudentCourses.GetAllAsync();
            return View(enrollments);
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
        public async Task<IActionResult> Create(Domain.Entities.StudentCourse enrollment)
        {
            if (ModelState.IsValid)
            {
                // التحقق من عدم تسجيل الطالب مرتين في نفس الدورة
                var exists = await _unitOfWork.StudentCourses.IsStudentEnrolledAsync(
                    enrollment.StudentId, 
                    enrollment.CourseId);

                if (exists)
                {
                    ModelState.AddModelError("", "الطالب مسجل بالفعل في هذه الدورة");
                    ViewBag.Students = await _unitOfWork.Students.GetActiveStudentsAsync();
                    ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
                    return View(enrollment);
                }

                await _unitOfWork.StudentCourses.AddAsync(enrollment);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تسجيل طالب في دورة");
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Students = await _unitOfWork.Students.GetActiveStudentsAsync();
            ViewBag.Courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(enrollment);
        }
    }
}

