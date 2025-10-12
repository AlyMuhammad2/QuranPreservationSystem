using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets - الجداول
        public DbSet<Center> Centers { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تكوين العلاقات والقيود

            // علاقة Center -> Teachers (One-to-Many)
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Center)
                .WithMany(c => c.Teachers)
                .HasForeignKey(t => t.CenterId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف المركز إذا كان له مدرسين

            // علاقة Center -> Students (One-to-Many)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Center)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.CenterId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف المركز إذا كان له طلاب

            // علاقة Center -> Courses (One-to-Many)
            modelBuilder.Entity<Course>()
                .HasOne(co => co.Center)
                .WithMany(c => c.Courses)
                .HasForeignKey(co => co.CenterId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف المركز إذا كان له دورات

            // علاقة Teacher -> Courses (One-to-Many)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف المدرس إذا كان له دورات

            // علاقة Student <-> Course (Many-to-Many عبر StudentCourse)
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // عند حذف الطالب، حذف التسجيلات

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // عند حذف الدورة، حذف التسجيلات

            // إضافة Index للأداء
            modelBuilder.Entity<StudentCourse>()
                .HasIndex(sc => new { sc.StudentId, sc.CourseId })
                .IsUnique(); // منع تسجيل الطالب في نفس الدورة مرتين

            // تكوين الأنواع والقيود الإضافية
            modelBuilder.Entity<Center>()
                .HasIndex(c => c.Name);

            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.PhoneNumber);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.PhoneNumber);

            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseName);

            // تكوين الأنواع العددية
            modelBuilder.Entity<StudentCourse>()
                .Property(sc => sc.Grade)
                .HasPrecision(5, 2);
        }
    }
}

