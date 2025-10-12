using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
        {
            return await _dbSet
                .Include(s => s.Center)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByCenterAsync(int centerId)
        {
            return await _dbSet
                .Where(s => s.CenterId == centerId)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentWithCoursesAsync(int studentId)
        {
            return await _dbSet
                .Include(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
                .Include(s => s.Center)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<IEnumerable<Student>> SearchStudentsByNameAsync(string name)
        {
            return await _dbSet
                .Where(s => s.FirstName.Contains(name) || s.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentsByCourseAsync(int courseId)
        {
            return await _dbSet
                .Include(s => s.StudentCourses)
                .Where(s => s.StudentCourses.Any(sc => sc.CourseId == courseId))
                .ToListAsync();
        }
    }
}

