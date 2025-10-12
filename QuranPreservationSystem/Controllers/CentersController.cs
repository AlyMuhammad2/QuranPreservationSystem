using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Controllers
{
    /// <summary>
    /// المراكز القرآنية - إدارة المراكز
    /// </summary>
    [Authorize]
    public class CentersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CentersController> _logger;

        public CentersController(
            IUnitOfWork unitOfWork,
            ILogger<CentersController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Centers
        public async Task<IActionResult> Index()
        {
            var centers = await _unitOfWork.Centers.GetAllAsync();
            
            // تحويل Entities إلى DTOs
            var centerDtos = centers.Select(c => new CenterDto
            {
                CenterId = c.CenterId,
                Name = c.Name,
                Address = c.Address,
                PhoneNumber = c.PhoneNumber,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate,
                TeachersCount = c.Teachers?.Count ?? 0,
                StudentsCount = c.Students?.Count ?? 0,
                CoursesCount = c.Courses?.Count ?? 0
            }).ToList();
            
            return View(centerDtos);
        }

        // GET: Centers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var center = await _unitOfWork.Centers.GetCenterWithDetailsAsync(id);
            
            if (center == null)
            {
                return NotFound();
            }

            // تحويل Entity إلى DTO
            var centerDto = new CenterDto
            {
                CenterId = center.CenterId,
                Name = center.Name,
                Address = center.Address,
                PhoneNumber = center.PhoneNumber,
                Description = center.Description,
                IsActive = center.IsActive,
                CreatedDate = center.CreatedDate,
                TeachersCount = center.Teachers?.Count ?? 0,
                StudentsCount = center.Students?.Count ?? 0,
                CoursesCount = center.Courses?.Count ?? 0
            };

            return View(centerDto);
        }

        // GET: Centers/Create
        public IActionResult Create()
        {
            return View(new CreateCenterDto());
        }

        // POST: Centers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCenterDto dto)
        {
            if (ModelState.IsValid)
            {
                var center = new Center
                {
                    Name = dto.Name,
                    Address = dto.Address,
                    PhoneNumber = dto.PhoneNumber,
                    Description = dto.Description,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
                
                await _unitOfWork.Centers.AddAsync(center);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم إضافة مركز جديد: {CenterName}", center.Name);
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(dto);
        }

        // GET: Centers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var center = await _unitOfWork.Centers.GetByIdAsync(id);
            
            if (center == null)
            {
                return NotFound();
            }

            // تحويل Entity إلى DTO
            var updateDto = new UpdateCenterDto
            {
                CenterId = center.CenterId,
                Name = center.Name,
                Address = center.Address,
                PhoneNumber = center.PhoneNumber,
                Description = center.Description,
                IsActive = center.IsActive
            };

            return View(updateDto);
        }

        // POST: Centers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCenterDto dto)
        {
            if (id != dto.CenterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var center = await _unitOfWork.Centers.GetByIdAsync(id);
                
                if (center == null)
                {
                    return NotFound();
                }
                
                // تحديث البيانات
                center.Name = dto.Name;
                center.Address = dto.Address;
                center.PhoneNumber = dto.PhoneNumber;
                center.Description = dto.Description;
                center.IsActive = dto.IsActive;
                
                await _unitOfWork.Centers.UpdateAsync(center);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم تحديث مركز: {CenterName}", center.Name);
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(dto);
        }

        // GET: Centers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var center = await _unitOfWork.Centers.GetByIdAsync(id);
            
            if (center == null)
            {
                return NotFound();
            }

            // تحويل Entity إلى DTO
            var centerDto = new CenterDto
            {
                CenterId = center.CenterId,
                Name = center.Name,
                Address = center.Address,
                PhoneNumber = center.PhoneNumber,
                Description = center.Description,
                IsActive = center.IsActive,
                CreatedDate = center.CreatedDate
            };

            return View(centerDto);
        }

        // POST: Centers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var center = await _unitOfWork.Centers.GetByIdAsync(id);
            
            if (center != null)
            {
                await _unitOfWork.Centers.DeleteAsync(center);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("تم حذف مركز: {CenterName}", center.Name);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

