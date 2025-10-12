using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Application.DTOs;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// الطلاب - إدارة الطلاب
    /// </summary>
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(
            IUnitOfWork unitOfWork,
            ILogger<StudentsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchTerm, int? centerId)
        {
            var students = await _unitOfWork.Students.GetActiveStudentsAsync();
            
            // تطبيق الفلترة حسب المركز
            if (centerId.HasValue && centerId.Value > 0)
            {
                students = students.Where(s => s.CenterId == centerId.Value).ToList();
            }

            // تطبيق البحث بالاسم
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                students = students.Where(s => 
                    s.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.PhoneNumber.Contains(searchTerm)
                ).ToList();
            }
            
            var studentDtos = students.Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                FullName = s.FullName,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                Address = s.Address,
                IdentityNumber = null, // Not available in current entity
                CenterId = s.CenterId,
                CenterName = s.Center?.Name,
                Notes = s.Notes,
                DocumentPath = s.DocumentPath,
                IsActive = s.IsActive,
                CreatedDate = s.EnrollmentDate, // Using EnrollmentDate as CreatedDate
                LastModifiedDate = null // Not available in current entity
            }).ToList();
            
            // إرسال قائمة المراكز للفلتر
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCenterId = centerId;
            
            return View(studentDtos);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var student = await _unitOfWork.Students.GetStudentWithCoursesAsync(id);
            
            if (student == null)
            {
                return NotFound();
            }

            var studentDto = new StudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = student.FullName,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                IdentityNumber = null, // Not available in current entity
                CenterId = student.CenterId,
                CenterName = student.Center?.Name,
                Notes = student.Notes,
                DocumentPath = student.DocumentPath,
                IsActive = student.IsActive,
                CreatedDate = student.EnrollmentDate, // Using EnrollmentDate as CreatedDate
                LastModifiedDate = null, // Not available in current entity
                StudentCourses = student.StudentCourses?.Select(sc => new StudentCourseDto
                {
                    StudentCourseId = sc.StudentCourseId,
                    StudentId = sc.StudentId,
                    CourseId = sc.CourseId,
                    CourseName = sc.Course?.CourseName, // Using CourseName instead of Title
                    TeacherName = sc.Course?.Teacher?.FullName,
                    RegistrationDate = sc.EnrollmentDate, // Using EnrollmentDate instead of RegistrationDate
                    ExamDate = sc.ExamDate,
                    Status = sc.Status,
                    Examiners = !string.IsNullOrEmpty(sc.Examiner1) && !string.IsNullOrEmpty(sc.Examiner2) 
                        ? $"{sc.Examiner1}, {sc.Examiner2}"
                        : sc.Examiner1 ?? sc.Examiner2
                }).ToList()
            };

            return View(studentDto);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStudentDto createStudentDto)
        {
            if (ModelState.IsValid)
            {
                var student = new Domain.Entities.Student
                {
                    FirstName = createStudentDto.FirstName,
                    LastName = createStudentDto.LastName,
                    PhoneNumber = createStudentDto.PhoneNumber,
                    Email = createStudentDto.Email,
                    Gender = createStudentDto.Gender,
                    DateOfBirth = createStudentDto.DateOfBirth ?? DateTime.Now.AddYears(-20), // Default to 20 years ago if null
                    Address = createStudentDto.Address,
                    CenterId = createStudentDto.CenterId ?? 1, // Default to first center if null
                    Notes = createStudentDto.Notes,
                    IsActive = createStudentDto.IsActive,
                    EnrollmentDate = DateTime.Now
                };

                await _unitOfWork.Students.AddAsync(student);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم إضافة طالب جديد: {StudentName}", student.FullName);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(createStudentDto);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            
            if (student == null)
            {
                return NotFound();
            }

            var updateStudentDto = new UpdateStudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                IdentityNumber = null, // Not available in current entity
                CenterId = student.CenterId,
                Notes = student.Notes,
                IsActive = student.IsActive
            };

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(updateStudentDto);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateStudentDto updateStudentDto)
        {
            if (id != updateStudentDto.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var student = await _unitOfWork.Students.GetByIdAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                student.FirstName = updateStudentDto.FirstName;
                student.LastName = updateStudentDto.LastName;
                student.PhoneNumber = updateStudentDto.PhoneNumber;
                student.Email = updateStudentDto.Email;
                student.Gender = updateStudentDto.Gender;
                student.DateOfBirth = updateStudentDto.DateOfBirth ?? student.DateOfBirth; // Keep existing if null
                student.Address = updateStudentDto.Address;
                student.CenterId = updateStudentDto.CenterId ?? student.CenterId; // Keep existing if null
                student.Notes = updateStudentDto.Notes;
                student.IsActive = updateStudentDto.IsActive;

                await _unitOfWork.Students.UpdateAsync(student);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تحديث بيانات طالب: {StudentName}", student.FullName);
                
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(updateStudentDto);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            
            if (student == null)
            {
                return NotFound();
            }

            var studentDto = new StudentDto
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = student.FullName,
                PhoneNumber = student.PhoneNumber,
                Email = student.Email,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                IdentityNumber = null, // Not available in current entity
                CenterId = student.CenterId,
                CenterName = student.Center?.Name,
                Notes = student.Notes,
                DocumentPath = student.DocumentPath,
                IsActive = student.IsActive,
                CreatedDate = student.EnrollmentDate, // Using EnrollmentDate as CreatedDate
                LastModifiedDate = null // Not available in current entity
            };

            return View(studentDto);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);
            
            if (student != null)
            {
                await _unitOfWork.Students.DeleteAsync(student);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم حذف طالب: {StudentName}", student.FullName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

