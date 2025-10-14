using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// واجهة مخزن ديوان الحفاظ
    /// </summary>
    public interface IHafizRegistryRepository : IGenericRepository<HafizRegistry>
    {
        /// <summary>
        /// جلب جميع الحفاظ النشطين مع التفاصيل
        /// </summary>
        Task<IEnumerable<HafizRegistry>> GetActiveHafazAsync();

        /// <summary>
        /// جلب الحفاظ حسب المركز
        /// </summary>
        Task<IEnumerable<HafizRegistry>> GetHafazByCenterAsync(int centerId);

        /// <summary>
        /// جلب الحفاظ حسب السنة
        /// </summary>
        Task<IEnumerable<HafizRegistry>> GetHafazByYearAsync(int year);

        /// <summary>
        /// جلب حافظ واحد مع التفاصيل
        /// </summary>
        Task<HafizRegistry?> GetHafizWithDetailsAsync(int hafizId);

        /// <summary>
        /// البحث في ديوان الحفاظ
        /// </summary>
        Task<IEnumerable<HafizRegistry>> SearchHafazAsync(string searchTerm);

        /// <summary>
        /// جلب الحفاظ مع Pagination
        /// </summary>
        Task<(IEnumerable<HafizRegistry> Items, int TotalCount)> GetPagedHafazAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null, 
            int? centerId = null);
    }
}

