using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Infrastructure.Data.SeedData;

public static class PermissionSeedData
{
    public static void SeedPermissions(ModelBuilder modelBuilder)
    {
        var permissions = new List<Permission>
        {
            new Permission
            {
                PermissionId = 1,
                PermissionName = "Dashboard",
                DisplayName = "لوحة التحكم",
                Description = "الوصول إلى لوحة التحكم الرئيسية",
                Icon = "fa-tachometer-alt",
                DisplayOrder = 1,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 2,
                PermissionName = "Centers",
                DisplayName = "المراكز القرآنية",
                Description = "إدارة المراكز القرآنية",
                Icon = "fa-mosque",
                DisplayOrder = 2,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 3,
                PermissionName = "Teachers",
                DisplayName = "المدرسين",
                Description = "إدارة المدرسين",
                Icon = "fa-chalkboard-teacher",
                DisplayOrder = 3,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 4,
                PermissionName = "Students",
                DisplayName = "الطلاب",
                Description = "إدارة الطلاب",
                Icon = "fa-user-graduate",
                DisplayOrder = 4,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 5,
                PermissionName = "HafizRegistry",
                DisplayName = "ديوان الحفاظ",
                Description = "إدارة ديوان الحفاظ",
                Icon = "fa-book-quran",
                DisplayOrder = 5,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 6,
                PermissionName = "Courses",
                DisplayName = "الدورات",
                Description = "إدارة الدورات",
                Icon = "fa-book-open",
                DisplayOrder = 6,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 7,
                PermissionName = "Enrollments",
                DisplayName = "التسجيلات",
                Description = "إدارة تسجيلات الطلاب",
                Icon = "fa-user-plus",
                DisplayOrder = 7,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 8,
                PermissionName = "Exams",
                DisplayName = "الاختبارات",
                Description = "إدارة الاختبارات",
                Icon = "fa-file-alt",
                DisplayOrder = 8,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 9,
                PermissionName = "Reports",
                DisplayName = "التقارير والإحصائيات",
                Description = "عرض التقارير والإحصائيات",
                Icon = "fa-chart-bar",
                DisplayOrder = 9,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 10,
                PermissionName = "ImportData",
                DisplayName = "استيراد البيانات",
                Description = "استيراد البيانات من ملفات Excel",
                Icon = "fa-file-import",
                DisplayOrder = 10,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 11,
                PermissionName = "Users",
                DisplayName = "المستخدمين",
                Description = "إدارة المستخدمين والصلاحيات",
                Icon = "fa-users-cog",
                DisplayOrder = 11,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 12,
                PermissionName = "Settings",
                DisplayName = "الإعدادات",
                Description = "إدارة إعدادات النظام",
                Icon = "fa-sliders-h",
                DisplayOrder = 12,
                IsActive = true
            },
            new Permission
            {
                PermissionId = 13,
                PermissionName = "Logs",
                DisplayName = "سجل النشاطات",
                Description = "عرض سجل النشاطات",
                Icon = "fa-history",
                DisplayOrder = 13,
                IsActive = true
            }
        };

        modelBuilder.Entity<Permission>().HasData(permissions);
    }
}

