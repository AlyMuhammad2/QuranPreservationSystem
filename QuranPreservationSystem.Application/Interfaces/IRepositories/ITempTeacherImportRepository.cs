using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// واجهة مخزن استيراد المدرسين المؤقت
    /// </summary>
    public interface ITempTeacherImportRepository : IGenericRepository<TempTeacherImport>
    {
        /// <summary>
        /// جلب السجلات بحالة معينة
        /// </summary>
        Task<IEnumerable<TempTeacherImport>> GetByStatusAsync(ImportStatus status);

        /// <summary>
        /// جلب السجلات بـ Batch Id
        /// </summary>
        Task<IEnumerable<TempTeacherImport>> GetByBatchIdAsync(string batchId);

        /// <summary>
        /// جلب السجلات المعلقة (Pending)
        /// </summary>
        Task<IEnumerable<TempTeacherImport>> GetPendingImportsAsync();

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

