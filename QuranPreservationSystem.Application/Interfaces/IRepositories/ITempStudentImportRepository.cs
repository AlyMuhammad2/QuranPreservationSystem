using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// واجهة مخزن استيراد الطلاب المؤقت
    /// </summary>
    public interface ITempStudentImportRepository : IGenericRepository<TempStudentImport>
    {
        /// <summary>
        /// جلب السجلات بحالة معينة
        /// </summary>
        Task<IEnumerable<TempStudentImport>> GetByStatusAsync(ImportStatus status);

        /// <summary>
        /// جلب السجلات بـ Batch Id
        /// </summary>
        Task<IEnumerable<TempStudentImport>> GetByBatchIdAsync(string batchId);

        /// <summary>
        /// جلب السجلات المعلقة (Pending)
        /// </summary>
        Task<IEnumerable<TempStudentImport>> GetPendingImportsAsync();

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

