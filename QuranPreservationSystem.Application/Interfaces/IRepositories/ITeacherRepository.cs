using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// Repository Interface للمدرسين - يحتوي على عمليات خاصة بالمدرسين
    /// </summary>
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<IEnumerable<Teacher>> GetActiveTeachersAsync();
        Task<IEnumerable<Teacher>> GetTeachersByCenterAsync(int centerId);
        Task<Teacher?> GetTeacherWithCoursesAsync(int teacherId);
        Task<IEnumerable<Teacher>> SearchTeachersByNameAsync(string name);
    }
}

