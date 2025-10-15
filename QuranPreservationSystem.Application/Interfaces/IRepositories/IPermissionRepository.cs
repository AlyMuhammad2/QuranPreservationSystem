using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<IEnumerable<Permission>> GetActivePermissionsAsync();
    Task<Permission?> GetByNameAsync(string permissionName);
}

