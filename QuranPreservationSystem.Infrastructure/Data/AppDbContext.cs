using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Domain.Entities;
using QuranPreservationSystem.Infrastructure.Data.SeedData;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<Exam> Exams { get; set; }
        public DbSet<HafizRegistry> HafizRegistry { get; set; }
        public DbSet<TempCenterImport> TempCenterImports { get; set; }
        public DbSet<TempTeacherImport> TempTeacherImports { get; set; }
        public DbSet<TempStudentImport> TempStudentImports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // تجاهل warning الـ pending model changes
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

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

            // علاقة Exam -> Center (Many-to-One)
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Center)
                .WithMany()
                .HasForeignKey(e => e.CenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقة Exam -> Course (Many-to-One)
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقة HafizRegistry -> Center (Many-to-One)
            modelBuilder.Entity<HafizRegistry>()
                .HasOne(h => h.Center)
                .WithMany()
                .HasForeignKey(h => h.CenterId)
                .OnDelete(DeleteBehavior.SetNull);

            // تكوين الأنواع والقيود الإضافية
            modelBuilder.Entity<Center>()
                .HasIndex(c => c.Name);

            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.PhoneNumber);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.PhoneNumber);

            modelBuilder.Entity<Course>()
                .HasIndex(c => c.CourseName);

            modelBuilder.Entity<Exam>()
                .HasIndex(e => e.ExamName);

            modelBuilder.Entity<HafizRegistry>()
                .HasIndex(h => h.StudentName);

            modelBuilder.Entity<HafizRegistry>()
                .HasIndex(h => h.CompletionYear);

            // Indexes للـ TempCenterImport
            modelBuilder.Entity<TempCenterImport>()
                .HasIndex(t => t.Status);

            modelBuilder.Entity<TempCenterImport>()
                .HasIndex(t => t.BatchId);

            // Indexes للـ TempTeacherImport
            modelBuilder.Entity<TempTeacherImport>()
                .HasIndex(t => t.Status);

            modelBuilder.Entity<TempTeacherImport>()
                .HasIndex(t => t.BatchId);

            // Indexes للـ TempStudentImport
            modelBuilder.Entity<TempStudentImport>()
                .HasIndex(t => t.Status);

            modelBuilder.Entity<TempStudentImport>()
                .HasIndex(t => t.BatchId);

            // تكوين الأنواع العددية
            modelBuilder.Entity<StudentCourse>()
                .Property(sc => sc.Grade)
                .HasPrecision(5, 2);

            // تكوين Enum Types
            modelBuilder.Entity<Student>()
                .Property(s => s.Gender)
                .HasConversion<int>();

            modelBuilder.Entity<Teacher>()
                .Property(t => t.Gender)
                .HasConversion<int>();

            modelBuilder.Entity<Course>()
                .Property(c => c.CourseType)
                .HasConversion<int>();

            modelBuilder.Entity<HafizRegistry>()
                .Property(h => h.MemorizationLevel)
                .HasConversion<int?>();

            modelBuilder.Entity<TempCenterImport>()
                .Property(t => t.Status)
                .HasConversion<int>();

            modelBuilder.Entity<TempTeacherImport>()
                .Property(t => t.Status)
                .HasConversion<int>();

            modelBuilder.Entity<TempStudentImport>()
                .Property(t => t.Status)
                .HasConversion<int>();

            modelBuilder.Entity<StudentCourse>()
                .Property(sc => sc.Status)
                .HasConversion<int>();

            // تكوين علاقات ApplicationUser
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Center)
                .WithMany()
                .HasForeignKey(u => u.CenterId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Teacher)
                .WithMany()
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

            // إضافة البيانات الأولية (Seed Data)
            CenterSeedData.SeedCenters(modelBuilder);
            IdentitySeedData.SeedRolesAndUsers(modelBuilder);
        }
    }
}

