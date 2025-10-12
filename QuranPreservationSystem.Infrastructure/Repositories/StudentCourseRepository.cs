using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    public class StudentCourseRepository : GenericRepository<StudentCourse>, IStudentCourseRepository
    {
        public StudentCourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StudentCourse>> GetEnrollmentsByStudentAsync(int studentId)
        {
            return await _dbSet
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Teacher)
                .Include(sc => sc.Course)
                    .ThenInclude(c => c.Center)
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentCourse>> GetEnrollmentsByCourseAsync(int courseId)
        {
            return await _dbSet
                .Include(sc => sc.Student)
                .Where(sc => sc.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<StudentCourse?> GetEnrollmentAsync(int studentId, int courseId)
        {
            return await _dbSet
                .Include(sc => sc.Student)
                .Include(sc => sc.Course)
                .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task<bool> IsStudentEnrolledAsync(int studentId, int courseId)
        {
            return await _dbSet
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }

        public async Task<int> GetCourseEnrollmentCountAsync(int courseId)
        {
            return await _dbSet
                .CountAsync(sc => sc.CourseId == courseId && sc.IsActive);
        }
    }
}

