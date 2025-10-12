using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    public class CenterRepository : GenericRepository<Center>, ICenterRepository
    {
        public CenterRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Center>> GetActiveCentersAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<Center?> GetCenterWithDetailsAsync(int centerId)
        {
            return await _dbSet
                .Include(c => c.Teachers)
                .Include(c => c.Students)
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.CenterId == centerId);
        }

        public async Task<IEnumerable<Center>> GetCentersWithTeachersAsync()
        {
            return await _dbSet
                .Include(c => c.Teachers)
                .ToListAsync();
        }

        public async Task<IEnumerable<Center>> GetCentersWithStudentsAsync()
        {
            return await _dbSet
                .Include(c => c.Students)
                .ToListAsync();
        }

        public async Task<IEnumerable<Center>> GetCentersWithCoursesAsync()
        {
            return await _dbSet
                .Include(c => c.Courses)
                .ToListAsync();
        }
    }
}

