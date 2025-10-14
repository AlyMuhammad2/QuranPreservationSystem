using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data;

namespace QuranPreservationSystem.Infrastructure.Repositories
{
    /// <summary>
    /// مخزن استيراد المراكز المؤقت
    /// </summary>
    public class TempCenterImportRepository : GenericRepository<TempCenterImport>, ITempCenterImportRepository
    {
        public TempCenterImportRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TempCenterImport>> GetByStatusAsync(ImportStatus status)
        {
            return await _dbSet
                .Where(t => t.Status == status)
                .OrderBy(t => t.UploadedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TempCenterImport>> GetByBatchIdAsync(string batchId)
        {
            return await _dbSet
                .Where(t => t.BatchId == batchId)
                .OrderBy(t => t.RowNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<TempCenterImport>> GetPendingImportsAsync()
        {
            return await _dbSet
                .Where(t => t.Status == ImportStatus.Pending)
                .OrderBy(t => t.UploadedDate)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int tempId, ImportStatus status, string? errorMessage = null)
        {
            var record = await _dbSet.FindAsync(tempId);
            if (record != null)
            {
                record.Status = status;
                record.ErrorMessage = errorMessage;
                record.ProcessedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteOldRecordsAsync(int daysOld = 30)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysOld);
            var oldRecords = await _dbSet
                .Where(t => t.UploadedDate < cutoffDate)
                .ToListAsync();

            _dbSet.RemoveRange(oldRecords);
            await _context.SaveChangesAsync();
        }
    }
}

