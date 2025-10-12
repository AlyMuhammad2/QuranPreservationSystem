using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// Repository Interface للمراكز - يحتوي على عمليات خاصة بالمراكز
    /// </summary>
    public interface ICenterRepository : IGenericRepository<Center>
    {
        Task<IEnumerable<Center>> GetActiveCentersAsync();
        Task<Center?> GetCenterWithDetailsAsync(int centerId);
        Task<IEnumerable<Center>> GetCentersWithTeachersAsync();
        Task<IEnumerable<Center>> GetCentersWithStudentsAsync();
        Task<IEnumerable<Center>> GetCentersWithCoursesAsync();
    }
}

