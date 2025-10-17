using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Controllers;

[Authorize(Roles = "Admin")]
public class RolePermissionsController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RolePermissionsController> _logger;

    public RolePermissionsController(
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        ILogger<RolePermissionsController> logger)
    {
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // GET: RolePermissions
    public async Task<IActionResult> Index()
    {
        var roles = _roleManager.Roles.ToList();
        var rolePermissionsList = new List<RolePermissionDto>();

        foreach (var role in roles)
        {
            var permissions = await _unitOfWork.RolePermissions.GetRolePermissionsAsync(role.Id);
            
            foreach (var permission in permissions)
            {
                rolePermissionsList.Add(new RolePermissionDto
                {
                    RolePermissionId = permission.RolePermissionId,
                    RoleId = permission.RoleId,
                    RoleName = role.Name ?? "",
                    PermissionId = permission.PermissionId,
                    PermissionName = permission.Permission.PermissionName,
                    DisplayName = permission.Permission.DisplayName,
                    CanView = permission.CanView,
                    CanCreate = permission.CanCreate,
                    CanEdit = permission.CanEdit,
                    CanDelete = permission.CanDelete
                });
            }
        }

        ViewBag.Roles = roles;
        return View(rolePermissionsList);
    }

    // GET: RolePermissions/Manage/roleId
    public async Task<IActionResult> Manage(string roleId)
    {
        if (string.IsNullOrEmpty(roleId))
            return NotFound();

        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null)
            return NotFound();

        var allPermissions = await _unitOfWork.Permissions.GetActivePermissionsAsync();
        var rolePermissions = await _unitOfWork.RolePermissions.GetRolePermissionsAsync(roleId);

        var model = new ManageRolePermissionsDto
        {
            RoleId = roleId,
            RoleName = role.Name ?? "",
            Permissions = new List<PermissionAssignment>()
        };

        foreach (var permission in allPermissions)
        {
            var rolePermission = rolePermissions.FirstOrDefault(rp => rp.PermissionId == permission.PermissionId);
            
            model.Permissions.Add(new PermissionAssignment
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                DisplayName = permission.DisplayName,
                Icon = permission.Icon,
                IsAssigned = rolePermission != null,
                CanView = rolePermission?.CanView ?? false,
                CanCreate = rolePermission?.CanCreate ?? false,
                CanEdit = rolePermission?.CanEdit ?? false,
                CanDelete = rolePermission?.CanDelete ?? false
            });
        }

        return View(model);
    }

    // POST: RolePermissions/Manage
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Manage(ManageRolePermissionsDto model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var role = await _roleManager.FindByIdAsync(model.RoleId);
        if (role == null)
            return NotFound();

        try
        {
            // حذف الصلاحيات الحالية
            await _unitOfWork.RolePermissions.DeleteRolePermissionsAsync(model.RoleId);

            // إضافة الصلاحيات الجديدة
            foreach (var permission in model.Permissions.Where(p => p.IsAssigned))
            {
                var rolePermission = new RolePermission
                {
                    RoleId = model.RoleId,
                    PermissionId = permission.PermissionId,
                    CanView = permission.CanView,
                    CanCreate = permission.CanCreate,
                    CanEdit = permission.CanEdit,
                    CanDelete = permission.CanDelete,
                    CreatedDate = DateTime.Now
                };

                await _unitOfWork.RolePermissions.AddAsync(rolePermission);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Permissions updated for role {role.Name}");
            TempData["Success"] = $"تم تحديث صلاحيات دور {role.Name} بنجاح";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role permissions");
            ModelState.AddModelError("", "حدث خطأ أثناء تحديث الصلاحيات");
            return View(model);
        }
    }
}

