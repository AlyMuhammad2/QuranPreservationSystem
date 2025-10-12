using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// Repository Interface لتسجيلات الطلاب في الدورات
    /// </summary>
    public interface IStudentCourseRepository : IGenericRepository<StudentCourse>
    {
        Task<IEnumerable<StudentCourse>> GetEnrollmentsByStudentAsync(int studentId);
        Task<IEnumerable<StudentCourse>> GetEnrollmentsByCourseAsync(int courseId);
        Task<StudentCourse?> GetEnrollmentAsync(int studentId, int courseId);
        Task<bool> IsStudentEnrolledAsync(int studentId, int courseId);
        Task<int> GetCourseEnrollmentCountAsync(int courseId);
        Task<IEnumerable<StudentCourse>> GetAllWithDetailsAsync();
        Task<StudentCourse?> GetStudentCourseWithDetailsAsync(int studentCourseId);
    }
}

