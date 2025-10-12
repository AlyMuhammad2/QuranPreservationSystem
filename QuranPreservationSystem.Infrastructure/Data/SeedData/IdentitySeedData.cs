using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Domain.Enums;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Infrastructure.Data.SeedData
{
    /// <summary>
    /// بيانات أولية للمستخدمين والأدوار
    /// </summary>
    public static class IdentitySeedData
    {
        public static void SeedRolesAndUsers(ModelBuilder modelBuilder)
        {
            // ملاحظة: تم نقل seed للـ users إلى DbInitializer.cs
            // هنا نضيف الأدوار فقط في الـ migration
            
            // إنشاء الأدوار (Roles)
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = UserRole.Admin,
                    NormalizedName = UserRole.Admin.ToUpper(),
                    ConcurrencyStamp = "c8554266-b401-4519-9aeb-ff6d5f3c4b61"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = UserRole.Supervisor,
                    NormalizedName = UserRole.Supervisor.ToUpper(),
                    ConcurrencyStamp = "c8554266-b401-4519-9aeb-ff6d5f3c4b62"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = UserRole.Teacher,
                    NormalizedName = UserRole.Teacher.ToUpper(),
                    ConcurrencyStamp = "c8554266-b401-4519-9aeb-ff6d5f3c4b63"
                },
                new IdentityRole
                {
                    Id = "4",
                    Name = UserRole.CenterManager,
                    NormalizedName = UserRole.CenterManager.ToUpper(),
                    ConcurrencyStamp = "c8554266-b401-4519-9aeb-ff6d5f3c4b64"
                },
                new IdentityRole
                {
                    Id = "5",
                    Name = UserRole.User,
                    NormalizedName = UserRole.User.ToUpper(),
                    ConcurrencyStamp = "c8554266-b401-4519-9aeb-ff6d5f3c4b65"
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            // ملاحظة: تم نقل إنشاء المستخدمين إلى DbInitializer.cs
            // لضمان عمل الـ password hashing بشكل صحيح
        }
    }
}

