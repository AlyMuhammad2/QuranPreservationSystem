using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Domain.Enums;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _dbSet
                .Include(c => c.Center)
                .Include(c => c.Teacher)
                .Include(c => c.StudentCourses)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByCenterAsync(int centerId)
        {
            return await _dbSet
                .Where(c => c.CenterId == centerId)
                .Include(c => c.Teacher)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId)
        {
            return await _dbSet
                .Where(c => c.TeacherId == teacherId)
                .Include(c => c.Center)
                .ToListAsync();
        }

        public async Task<Course?> GetCourseWithDetailsAsync(int courseId)
        {
            return await _dbSet
                .Include(c => c.Center)
                .Include(c => c.Teacher)
                .Include(c => c.StudentCourses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> GetUpcomingCoursesAsync()
        {
            return await _dbSet
                .Where(c => c.StartDate > DateTime.Now)
                .OrderBy(c => c.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByTypeAsync(string courseType)
        {
            // محاولة تحويل string إلى enum
            if (Enum.TryParse<CourseType>(courseType, true, out var courseTypeEnum))
            {
                return await _dbSet
                    .Where(c => c.CourseType == courseTypeEnum)
                    .ToListAsync();
            }
            
            return new List<Course>();
        }
    }
}

