using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    /// <summary>
    /// مخزن الاختبارات
    /// </summary>
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        private readonly AppDbContext _context;

        public ExamRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Exam>> GetActiveExamsAsync()
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByCenterAsync(int centerId)
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .Where(e => e.CenterId == centerId)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByCourseAsync(int courseId)
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .Where(e => e.CourseId == courseId)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByDateAsync(DateTime examDate)
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .OrderBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetUpcomingExamsAsync()
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .OrderByDescending(e => e.CreatedDate)
                .ThenBy(e => e.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetCompletedExamsAsync()
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<Exam?> GetExamWithDetailsAsync(int examId)
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.ExamId == examId);
        }

        public async Task<IEnumerable<Exam>> GetExamsByTypeAsync(string examType)
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .Where(e => e.ExamType == examType )
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> GetExamsByLevelAsync(string level)
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .Where(e => e.Level == level )
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exam>> SearchExamsAsync(string searchTerm)
        {
            return await _dbSet
                .Include(e => e.Center)
                .Include(e => e.Course)
                .Where(e => (
                    e.ExamName.Contains(searchTerm) ||
                    e.Description!.Contains(searchTerm) ||
                    e.ExamType.Contains(searchTerm) ||
                    e.Level.Contains(searchTerm) ||
                    e.Location!.Contains(searchTerm) ||
                    e.Center.Name.Contains(searchTerm) ||
                    e.Course.CourseName.Contains(searchTerm)
                ))
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }
    }
}
