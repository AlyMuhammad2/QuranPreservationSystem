using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// المدرسين - إدارة المدرسين
    /// </summary>
    [Authorize]
    public class TeachersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TeachersController> _logger;

        public TeachersController(
            IUnitOfWork unitOfWork,
            ILogger<TeachersController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            var teachers = await _unitOfWork.Teachers.GetAllAsync();
            
            // تحويل Entities إلى DTOs
            var teacherDtos = teachers.Select(t => new TeacherDto
            {
                TeacherId = t.TeacherId,
                FirstName = t.FirstName,
                LastName = t.LastName,
                PhoneNumber = t.PhoneNumber,
                Email = t.Email,
                Address = t.Address,
                DateOfBirth = t.DateOfBirth,
                Gender = t.Gender,
                Qualification = t.Qualification,
                Specialization = t.Specialization,
                HireDate = t.HireDate,
                IsActive = t.IsActive,
                Notes = t.Notes,
                CenterId = t.CenterId,
                CenterName = t.Center?.Name,
                CoursesCount = t.Courses?.Count ?? 0
            }).ToList();
            
            return View(teacherDtos);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetTeacherWithCoursesAsync(id);
            
            if (teacher == null)
            {
                return NotFound();
            }

            // تحويل Entity إلى DTO
            var teacherDto = new TeacherDto
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PhoneNumber = teacher.PhoneNumber,
                Email = teacher.Email,
                Address = teacher.Address,
                DateOfBirth = teacher.DateOfBirth,
                Gender = teacher.Gender,
                Qualification = teacher.Qualification,
                Specialization = teacher.Specialization,
                HireDate = teacher.HireDate,
                IsActive = teacher.IsActive,
                Notes = teacher.Notes,
                CenterId = teacher.CenterId,
                CenterName = teacher.Center?.Name,
                CoursesCount = teacher.Courses?.Count ?? 0
            };

            return View(teacherDto);
        }

        // GET: Teachers/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(new CreateTeacherDto());
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeacherDto dto)
        {
            if (ModelState.IsValid)
            {
                var teacher = new Teacher
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Address = dto.Address,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    Qualification = dto.Qualification,
                    Specialization = dto.Specialization,
                    Notes = dto.Notes,
                    CenterId = dto.CenterId,
                    HireDate = DateTime.Now,
                    IsActive = true
                };
                
                await _unitOfWork.Teachers.AddAsync(teacher);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم إضافة مدرس جديد: {TeacherName}", teacher.FullName);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(dto);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
            
            if (teacher == null)
            {
                return NotFound();
            }

            // تحويل Entity إلى DTO
            var updateDto = new UpdateTeacherDto
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PhoneNumber = teacher.PhoneNumber,
                Email = teacher.Email,
                Address = teacher.Address,
                DateOfBirth = teacher.DateOfBirth,
                Gender = teacher.Gender,
                Qualification = teacher.Qualification,
                Specialization = teacher.Specialization,
                IsActive = teacher.IsActive,
                Notes = teacher.Notes,
                CenterId = teacher.CenterId
            };

            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(updateDto);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTeacherDto dto)
        {
            if (id != dto.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
                
                if (teacher == null)
                {
                    return NotFound();
                }
                
                // تحديث البيانات
                teacher.FirstName = dto.FirstName;
                teacher.LastName = dto.LastName;
                teacher.PhoneNumber = dto.PhoneNumber;
                teacher.Email = dto.Email;
                teacher.Address = dto.Address;
                teacher.DateOfBirth = dto.DateOfBirth;
                teacher.Gender = dto.Gender;
                teacher.Qualification = dto.Qualification;
                teacher.Specialization = dto.Specialization;
                teacher.IsActive = dto.IsActive;
                teacher.Notes = dto.Notes;
                teacher.CenterId = dto.CenterId;
                
                await _unitOfWork.Teachers.UpdateAsync(teacher);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تحديث بيانات مدرس: {TeacherName}", teacher.FullName);
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(dto);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
            
            if (teacher == null)
            {
                return NotFound();
            }

            // تحويل Entity إلى DTO
            var teacherDto = new TeacherDto
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PhoneNumber = teacher.PhoneNumber,
                Email = teacher.Email,
                CenterId = teacher.CenterId,
                CenterName = teacher.Center?.Name,
                IsActive = teacher.IsActive,
                HireDate = teacher.HireDate,
                Specialization = teacher.Specialization
            };

            return View(teacherDto);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
            
            if (teacher != null)
            {
                await _unitOfWork.Teachers.DeleteAsync(teacher);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم حذف مدرس: {TeacherName}", teacher.FullName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

