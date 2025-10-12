using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// الدورات - إدارة الدورات التعليمية
    /// </summary>
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(
            IUnitOfWork unitOfWork,
            ILogger<CoursesController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Domain.Entities.Course course)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم إضافة دورة جديدة: {CourseName}", course.CourseName);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Domain.Entities.Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Courses.UpdateAsync(course);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تحديث دورة: {CourseName}", course.CourseName);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            
            if (course != null)
            {
                await _unitOfWork.Courses.DeleteAsync(course);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم حذف دورة: {CourseName}", course.CourseName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

