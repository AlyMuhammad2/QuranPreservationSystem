using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Authorization;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Helpers;
using QuranPreservationSystem.Infrastructure.Identity;

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
        private readonly IAuditLogService _auditLogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CentersController(
            IUnitOfWork unitOfWork,
            ILogger<CentersController> logger,
            IAuditLogService auditLogService,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _auditLogService = auditLogService;
            _userManager = userManager;
        }

        // GET: Centers
        [PermissionAuthorize("Centers", "View")]
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
        [PermissionAuthorize("Centers", "View")]
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
        [PermissionAuthorize("Centers", "Create")]
        public IActionResult Create()
        {
            return View(new CreateCenterDto());
        }

        // POST: Centers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PermissionAuthorize("Centers", "Create")]
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
                
                await _auditLogService.LogCreateAsync(User, _userManager, HttpContext, "center", center.CenterId, center, center.Name);
                
                TempData["Success"] = $"تم إضافة المركز '{center.Name}' بنجاح";
                _logger.LogInformation("تم إضافة مركز جديد: {CenterName}", center.Name);
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(dto);
        }

        // GET: Centers/Edit/5
        [PermissionAuthorize("Centers", "Edit")]
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
        [PermissionAuthorize("Centers", "Edit")]
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
                
                var oldCenter = new Center { CenterId = center.CenterId, Name = center.Name, Address = center.Address, PhoneNumber = center.PhoneNumber, Description = center.Description, IsActive = center.IsActive };
                
                // تحديث البيانات
                center.Name = dto.Name;
                center.Address = dto.Address;
                center.PhoneNumber = dto.PhoneNumber;
                center.Description = dto.Description;
                center.IsActive = dto.IsActive;
                
                await _unitOfWork.Centers.UpdateAsync(center);
                await _unitOfWork.SaveChangesAsync();
                
                await _auditLogService.LogUpdateAsync(User, _userManager, HttpContext, "center", center.CenterId, oldCenter, center, center.Name);
                
                TempData["Success"] = $"تم تحديث المركز '{center.Name}' بنجاح";
                _logger.LogInformation("تم تحديث مركز: {CenterName}", center.Name);
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(dto);
        }

        // GET: Centers/Delete/5
        [PermissionAuthorize("Centers", "Delete")]
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
        [PermissionAuthorize("Centers", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var center = await _unitOfWork.Centers.GetByIdAsync(id);
            
            if (center != null)
            {
                await _auditLogService.LogDeleteAsync(User, _userManager, HttpContext, "center", center.CenterId, center, center.Name);
                
                await _unitOfWork.Centers.DeleteAsync(center);
                await _unitOfWork.SaveChangesAsync();
                
                TempData["Success"] = $"تم حذف المركز '{center.Name}' بنجاح";
                _logger.LogInformation("تم حذف مركز: {CenterName}", center.Name);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

