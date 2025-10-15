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
                    Id = "3",
                    Name = UserRole.Teacher,
                    NormalizedName = UserRole.Teacher.ToUpper(),
                    ConcurrencyStamp = "c8554266-b401-4519-9aeb-ff6d5f3c4b63"
                }
           
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}

