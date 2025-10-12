using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Teacher>> GetActiveTeachersAsync()
        {
            return await _dbSet.Where(t => t.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Teacher>> GetTeachersByCenterAsync(int centerId)
        {
            return await _dbSet
                .Where(t => t.CenterId == centerId)
                .ToListAsync();
        }

        public async Task<Teacher?> GetTeacherWithCoursesAsync(int teacherId)
        {
            return await _dbSet
                .Include(t => t.Courses)
                .Include(t => t.Center)
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);
        }

        public async Task<IEnumerable<Teacher>> SearchTeachersByNameAsync(string name)
        {
            return await _dbSet
                .Where(t => t.FirstName.Contains(name) || t.LastName.Contains(name))
                .ToListAsync();
        }
    }
}

