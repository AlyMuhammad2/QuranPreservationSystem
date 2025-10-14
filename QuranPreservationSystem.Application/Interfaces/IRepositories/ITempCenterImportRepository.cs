using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// واجهة مخزن استيراد المراكز المؤقت
    /// </summary>
    public interface ITempCenterImportRepository : IGenericRepository<TempCenterImport>
    {
        /// <summary>
        /// جلب السجلات بحالة معينة
        /// </summary>
        Task<IEnumerable<TempCenterImport>> GetByStatusAsync(ImportStatus status);

        /// <summary>
        /// جلب السجلات بـ Batch Id
        /// </summary>
        Task<IEnumerable<TempCenterImport>> GetByBatchIdAsync(string batchId);

        /// <summary>
        /// جلب السجلات المعلقة (Pending)
        /// </summary>
        Task<IEnumerable<TempCenterImport>> GetPendingImportsAsync();

        /// <summary>
        /// تحديث حالة السجل
        /// </summary>
        Task UpdateStatusAsync(int tempId, ImportStatus status, string? errorMessage = null);

        /// <summary>
        /// حذف السجلات القديمة
        /// </summary>
        Task DeleteOldRecordsAsync(int daysOld = 30);
    }
}

