using Microsoft.AspNetCore.Identity;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Services;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string userId, string permissionName, string action);
    Task<List<string>> GetUserPermissionsAsync(string userId);
}

public class PermissionService : IPermissionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public PermissionService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HasPermissionAsync(string userId, string permissionName, string action)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var roles = await _userManager.GetRolesAsync(user);
        
        // Admin له جميع الصلاحيات
        if (roles.Contains("Admin"))
            return true;

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var hasPermission = await _unitOfWork.RolePermissions
                    .HasPermissionAsync(role.Id, permissionName, action);

                if (hasPermission)
                    return true;
            }
        }

        return false;
    }

    public async Task<List<string>> GetUserPermissionsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new List<string>();

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();

        // Admin له جميع الصلاحيات
        if (roles.Contains("Admin"))
        {
            var allPermissions = await _unitOfWork.Permissions.GetAllAsync();
            return allPermissions.Select(p => p.PermissionName).ToList();
        }

        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var rolePermissions = await _unitOfWork.RolePermissions
                    .GetPermissionsByRoleIdAsync(role.Id);

                permissions.AddRange(rolePermissions
                    .Where(rp => rp.CanView)
                    .Select(rp => rp.Permission.PermissionName));
            }
        }

        return permissions.Distinct().ToList();
    }
}

