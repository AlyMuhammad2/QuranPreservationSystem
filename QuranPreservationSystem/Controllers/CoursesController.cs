using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Application.DTOs;

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
        public async Task<IActionResult> Index(string searchTerm, int? centerId)
        {
            var courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
            
            // تطبيق الفلترة حسب المركز
            if (centerId.HasValue && centerId.Value > 0)
            {
                courses = courses.Where(c => c.CenterId == centerId.Value).ToList();
            }

            // تطبيق البحث بالاسم
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                courses = courses.Where(c => 
                    c.CourseName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (c.Description != null && c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            
            var courseDtos = courses.Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                CourseType = c.CourseType,
                Level = c.Level,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Schedule = c.Schedule,
                MaxStudents = c.MaxStudents,
                DurationHours = c.DurationHours,
                IsActive = c.IsActive,
                Notes = c.Notes,
                CreatedDate = c.CreatedDate,
                CenterId = c.CenterId,
                CenterName = c.Center?.Name,
                TeacherId = c.TeacherId,
                TeacherName = c.Teacher?.FullName,
                EnrolledStudentsCount = c.StudentCourses?.Count ?? 0,
                AvailableSeats = (c.MaxStudents ?? 0) - (c.StudentCourses?.Count ?? 0)
            }).ToList();

            // إرسال قائمة المراكز للفلتر
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCenterId = centerId;

            return View(courseDtos);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            var courseDto = new CourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                CourseType = course.CourseType,
                Level = course.Level,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Schedule = course.Schedule,
                MaxStudents = course.MaxStudents,
                DurationHours = course.DurationHours,
                IsActive = course.IsActive,
                Notes = course.Notes,
                CreatedDate = course.CreatedDate,
                CenterId = course.CenterId,
                CenterName = course.Center?.Name,
                TeacherId = course.TeacherId,
                TeacherName = course.Teacher?.FullName,
                EnrolledStudentsCount = course.StudentCourses?.Count ?? 0,
                AvailableSeats = (course.MaxStudents ?? 0) - (course.StudentCourses?.Count ?? 0)
            };

            return View(courseDto);
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
        public async Task<IActionResult> Create(CreateCourseDto createCourseDto)
        {
            if (ModelState.IsValid)
            {
                var course = new Domain.Entities.Course
                {
                    CourseName = createCourseDto.CourseName,
                    Description = createCourseDto.Description,
                    CourseType = createCourseDto.CourseType,
                    Level = createCourseDto.Level,
                    StartDate = createCourseDto.StartDate,
                    EndDate = createCourseDto.EndDate,
                    Schedule = createCourseDto.Schedule,
                    MaxStudents = createCourseDto.MaxStudents,
                    DurationHours = createCourseDto.DurationHours,
                    CenterId = createCourseDto.CenterId,
                    TeacherId = createCourseDto.TeacherId,
                    Notes = createCourseDto.Notes,
                    IsActive = createCourseDto.IsActive,
                    CreatedDate = DateTime.Now
                };

                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم إضافة دورة جديدة: {CourseName}", course.CourseName);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            return View(createCourseDto);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            var updateCourseDto = new UpdateCourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                CourseType = course.CourseType,
                Level = course.Level,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Schedule = course.Schedule,
                MaxStudents = course.MaxStudents,
                DurationHours = course.DurationHours,
                CenterId = course.CenterId,
                TeacherId = course.TeacherId,
                IsActive = course.IsActive,
                Notes = course.Notes
            };

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            return View(updateCourseDto);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCourseDto updateCourseDto)
        {
            if (id != updateCourseDto.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
                if (course == null)
                {
                    return NotFound();
                }

                course.CourseName = updateCourseDto.CourseName;
                course.Description = updateCourseDto.Description;
                course.CourseType = updateCourseDto.CourseType;
                course.Level = updateCourseDto.Level;
                course.StartDate = updateCourseDto.StartDate;
                course.EndDate = updateCourseDto.EndDate;
                course.Schedule = updateCourseDto.Schedule;
                course.MaxStudents = updateCourseDto.MaxStudents;
                course.DurationHours = updateCourseDto.DurationHours;
                course.CenterId = updateCourseDto.CenterId;
                course.TeacherId = updateCourseDto.TeacherId;
                course.IsActive = updateCourseDto.IsActive;
                course.Notes = updateCourseDto.Notes;

                await _unitOfWork.Courses.UpdateAsync(course);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تحديث دورة: {CourseName}", course.CourseName);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.Teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            return View(updateCourseDto);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            var courseDto = new CourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                CourseType = course.CourseType,
                Level = course.Level,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Schedule = course.Schedule,
                MaxStudents = course.MaxStudents,
                DurationHours = course.DurationHours,
                IsActive = course.IsActive,
                Notes = course.Notes,
                CreatedDate = course.CreatedDate,
                CenterId = course.CenterId,
                CenterName = course.Center?.Name,
                TeacherId = course.TeacherId,
                TeacherName = course.Teacher?.FullName,
                EnrolledStudentsCount = course.StudentCourses?.Count ?? 0,
                AvailableSeats = (course.MaxStudents ?? 0) - (course.StudentCourses?.Count ?? 0)
            };

            return View(courseDto);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _unitOfWork.Courses.GetCourseWithDetailsAsync(id);
            
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
