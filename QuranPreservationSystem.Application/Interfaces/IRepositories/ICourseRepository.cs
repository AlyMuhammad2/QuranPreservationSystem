using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// Repository Interface للدورات - يحتوي على عمليات خاصة بالدورات
    /// </summary>
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetActiveCoursesAsync();
        Task<IEnumerable<Course>> GetCoursesByCenterAsync(int centerId);
        Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId);
        Task<Course?> GetCourseWithDetailsAsync(int courseId);
        Task<IEnumerable<Course>> GetUpcomingCoursesAsync();
        Task<IEnumerable<Course>> GetCoursesByTypeAsync(string courseType);
    }
}

