using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Domain.Enums;
using QuranPreservationSystem.Infrastructure.Identity;

namespace QuranPreservationSystem.Infrastructure.Data
{
    /// <summary>
    /// تهيئة قاعدة البيانات وإضافة البيانات الأولية
    /// </summary>
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // تطبيق أي migrations معلقة
            await context.Database.MigrateAsync();

            // إنشاء الأدوار إذا لم تكن موجودة
            await CreateRolesAsync(roleManager);

            // إنشاء المستخدمين الافتراضيين
            await CreateDefaultUsersAsync(userManager);
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = UserRole.GetAllRoles().ToArray();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task CreateDefaultUsersAsync(UserManager<ApplicationUser> userManager)
        {
            // إنشاء مستخدم Admin
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@quransystem.com",
                    EmailConfirmed = true,
                    FirstName = "مدير",
                    LastName = "النظام",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRole.Admin);
                }
            }

            // إنشاء مستخدم Supervisor
            var supervisorUser = await userManager.FindByNameAsync("supervisor");
            if (supervisorUser == null)
            {
                supervisorUser = new ApplicationUser
                {
                    UserName = "supervisor",
                    Email = "supervisor@quransystem.com",
                    EmailConfirmed = true,
                    FirstName = "المشرف",
                    LastName = "العام",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(supervisorUser, "Supervisor@123");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(supervisorUser, UserRole.Supervisor);
                }
            }
        }
    }
}

