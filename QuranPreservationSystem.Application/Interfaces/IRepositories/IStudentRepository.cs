using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// Repository Interface للطلاب - يحتوي على عمليات خاصة بالطلاب
    /// </summary>
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<IEnumerable<Student>> GetActiveStudentsAsync();
        Task<IEnumerable<Student>> GetStudentsByCenterAsync(int centerId);
        Task<Student?> GetStudentWithCoursesAsync(int studentId);
        Task<IEnumerable<Student>> SearchStudentsByNameAsync(string name);
        Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId);
    }
}

