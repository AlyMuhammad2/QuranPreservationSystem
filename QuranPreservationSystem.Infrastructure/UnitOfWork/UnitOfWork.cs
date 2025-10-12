using Microsoft.EntityFrameworkCore.Storage;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Infrastructure.Data;
using QuranPreservationSystem.Infrastructure.Repositories;

namespace QuranPreservationSystem.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Unit of Work Implementation - تنفيذ نمط Unit of Work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repositories
        public ICenterRepository Centers { get; private set; }
        public ITeacherRepository Teachers { get; private set; }
        public IStudentRepository Students { get; private set; }
        public ICourseRepository Courses { get; private set; }
        public IStudentCourseRepository StudentCourses { get; private set; }
        public IExamRepository Exams { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            // تهيئة الـ Repositories
            Centers = new CenterRepository(_context);
            Teachers = new TeacherRepository(_context);
            Students = new StudentRepository(_context);
            Courses = new CourseRepository(_context);
            StudentCourses = new StudentCourseRepository(_context);
            Exams = new ExamRepository(_context);
        }

        // حفظ التغييرات
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Transaction Management
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // Dispose
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}

