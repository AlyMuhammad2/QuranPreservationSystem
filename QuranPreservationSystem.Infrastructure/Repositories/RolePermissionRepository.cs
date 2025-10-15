using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories;

public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RolePermission>> GetRolePermissionsAsync(string roleId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<RolePermission?> GetRolePermissionAsync(string roleId, int permissionId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
    }

    public async Task DeleteRolePermissionsAsync(string roleId)
    {
        var rolePermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();

        _context.RolePermissions.RemoveRange(rolePermissions);
    }

    public async Task<bool> HasPermissionAsync(string roleId, string permissionName)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .AnyAsync(rp => rp.RoleId == roleId && 
                           rp.Permission.PermissionName == permissionName && 
                           rp.CanView);
    }

    public async Task<bool> HasPermissionAsync(string roleId, string permissionName, string action)
    {
        var rolePermission = await _context.RolePermissions
            .Include(rp => rp.Permission)
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && 
                                      rp.Permission.PermissionName == permissionName);

        if (rolePermission == null)
            return false;

        return action.ToLower() switch
        {
            "view" => rolePermission.CanView,
            "create" => rolePermission.CanCreate,
            "edit" => rolePermission.CanEdit,
            "delete" => rolePermission.CanDelete,
            _ => false
        };
    }

    public async Task<IEnumerable<RolePermission>> GetPermissionsByRoleIdAsync(string roleId)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Permission)
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<string?> GetRoleIdByNameAsync(string roleName)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName);
        return role?.Id;
    }
}

