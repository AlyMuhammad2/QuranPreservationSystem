using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories;

public interface IRolePermissionRepository : IGenericRepository<RolePermission>
{
    Task<IEnumerable<RolePermission>> GetRolePermissionsAsync(string roleId);
    Task<IEnumerable<RolePermission>> GetPermissionsByRoleIdAsync(string roleId);
    Task<RolePermission?> GetRolePermissionAsync(string roleId, int permissionId);
    Task DeleteRolePermissionsAsync(string roleId);
    Task<bool> HasPermissionAsync(string roleId, string permissionName);
    Task<bool> HasPermissionAsync(string roleId, string permissionName, string action); // action: "View", "Create", "Edit", "Delete"
    Task<string?> GetRoleIdByNameAsync(string roleName);
}

