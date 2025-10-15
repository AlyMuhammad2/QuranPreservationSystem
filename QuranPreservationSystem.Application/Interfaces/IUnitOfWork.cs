using QuranPreservationSystem.Application.Interfaces.IRepositories;

namespace QuranPreservationSystem.Application.Interfaces
{
    /// <summary>
    /// Unit of Work Pattern - يدير جميع المعاملات (Transactions) والـ Repositories
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Repositories
        ICenterRepository Centers { get; }
        ITeacherRepository Teachers { get; }
        IStudentRepository Students { get; }
        ICourseRepository Courses { get; }
        IStudentCourseRepository StudentCourses { get; }
        IExamRepository Exams { get; }
        IHafizRegistryRepository HafizRegistry { get; }
        ITempCenterImportRepository TempCenterImports { get; }
        ITempTeacherImportRepository TempTeacherImports { get; }
        ITempStudentImportRepository TempStudentImports { get; }

        // حفظ التغييرات
        Task<int> SaveChangesAsync();
        
        // Transaction Management
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

