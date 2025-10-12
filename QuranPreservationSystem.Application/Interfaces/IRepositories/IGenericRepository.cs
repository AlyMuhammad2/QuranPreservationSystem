using System.Linq.Expressions;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// Generic Repository Interface - يحتوي على العمليات الأساسية (CRUD)
    /// </summary>
    /// <typeparam name="T">نوع الكيان (Entity)</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        // القراءة (Read)
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        // الإضافة (Create)
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);

        // التحديث (Update)
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        // الحذف (Delete)
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);

        // عمليات إضافية
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}

