using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    /// <summary>
    /// مخزن ديوان الحفاظ
    /// </summary>
    public class HafizRegistryRepository : GenericRepository<HafizRegistry>, IHafizRegistryRepository
    {
        public HafizRegistryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<HafizRegistry>> GetActiveHafazAsync()
        {
            return await _dbSet
                .Include(h => h.Center)
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.CompletionYear)
                .ThenBy(h => h.StudentName)
                .ToListAsync();
        }

        public async Task<IEnumerable<HafizRegistry>> GetHafazByCenterAsync(int centerId)
        {
            return await _dbSet
                .Include(h => h.Center)
                .Where(h => h.CenterId == centerId && h.IsActive)
                .OrderByDescending(h => h.CompletionYear)
                .ThenBy(h => h.StudentName)
                .ToListAsync();
        }

        public async Task<IEnumerable<HafizRegistry>> GetHafazByYearAsync(int year)
        {
            return await _dbSet
                .Include(h => h.Center)
                .Where(h => h.CompletionYear == year && h.IsActive)
                .OrderBy(h => h.StudentName)
                .ToListAsync();
        }

        public async Task<HafizRegistry?> GetHafizWithDetailsAsync(int hafizId)
        {
            return await _dbSet
                .Include(h => h.Center)
                .FirstOrDefaultAsync(h => h.HafizId == hafizId);
        }

        public async Task<IEnumerable<HafizRegistry>> SearchHafazAsync(string searchTerm)
        {
            return await _dbSet
                .Include(h => h.Center)
                .Where(h => h.IsActive && (
                    h.StudentName.Contains(searchTerm) ||
                    (h.CompletedCourses != null && h.CompletedCourses.Contains(searchTerm)) ||
                    h.Center.Name.Contains(searchTerm)
                ))
                .OrderByDescending(h => h.CompletionYear)
                .ThenBy(h => h.StudentName)
                .ToListAsync();
        }

        public async Task<(IEnumerable<HafizRegistry> Items, int TotalCount)> GetPagedHafazAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null, 
            int? centerId = null)
        {
            var query = _dbSet.Include(h => h.Center).Where(h => h.IsActive);

            // تطبيق الفلترة
            if (centerId.HasValue && centerId.Value > 0)
            {
                query = query.Where(h => h.CenterId == centerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(h =>
                    h.StudentName.Contains(searchTerm) ||
                    (h.CompletedCourses != null && h.CompletedCourses.Contains(searchTerm)) ||
                    h.Center.Name.Contains(searchTerm)
                );
            }

            // حساب إجمالي العدد
            var totalCount = await query.CountAsync();

            // تطبيق Pagination
            var items = await query
                .OrderByDescending(h => h.CompletionYear)
                .ThenBy(h => h.StudentName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}

