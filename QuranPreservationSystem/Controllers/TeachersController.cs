using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Authorization;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Helpers;
using ClosedXML.Excel;

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
        [PermissionAuthorize("Teachers", "View")]
        public async Task<IActionResult> Index(string searchTerm, int? centerId)
        {
            var teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            
            // تطبيق الفلترة حسب المركز
            if (centerId.HasValue && centerId.Value > 0)
            {
                teachers = teachers.Where(t => t.CenterId == centerId.Value).ToList();
            }

            // تطبيق البحث بالاسم
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                teachers = teachers.Where(t => 
                    t.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.PhoneNumber.Contains(searchTerm)
                ).ToList();
            }
            
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

            // إرسال قائمة المراكز للفلتر
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentCenterId = centerId;

            return View(teacherDtos);
        }

        // GET: Teachers/ExportToExcel
        [PermissionAuthorize("Teachers", "View")]
        public async Task<IActionResult> ExportToExcel(string searchTerm, int? centerId)
        {
            var teachers = await _unitOfWork.Teachers.GetActiveTeachersAsync();
            
            // تطبيق الفلترة
            if (centerId.HasValue && centerId.Value > 0)
            {
                teachers = teachers.Where(t => t.CenterId == centerId.Value).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                teachers = teachers.Where(t => 
                    t.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    t.PhoneNumber.Contains(searchTerm)
                ).ToList();
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("المدرسين");

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

            // Styling Header
            var headerRange = worksheet.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2e7d32");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Data
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

            var fileName = $"المدرسين_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // GET: Teachers/Details/5
        [PermissionAuthorize("Teachers", "View")]
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
        [PermissionAuthorize("Teachers", "Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Centers = await _unitOfWork.Centers.GetActiveCentersAsync();
            return View(new CreateTeacherDto());
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Teachers", "Create")]
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
        [PermissionAuthorize("Teachers", "Edit")]
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
        [PermissionAuthorize("Teachers", "Edit")]
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
        [PermissionAuthorize("Teachers", "Delete")]
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
        [PermissionAuthorize("Teachers", "Delete")]
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

