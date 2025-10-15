using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Infrastructure.Identity
{
    /// <summary>
    /// نموذج المستخدم - يمثل مستخدم في النظام (مدير، مشرف، إلخ)
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [StringLength(200, ErrorMessage = "الاسم الكامل يجب أن لا يتجاوز 200 حرف")]
        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }

        [StringLength(500)]
        public string? ProfilePicturePath { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Foreign Keys (Optional - ربط المستخدم بمركز أو مدرس)
        public int? CenterId { get; set; }
        public int? TeacherId { get; set; }

        // Navigation Properties
        public virtual Center? Center { get; set; }
        public virtual Teacher? Teacher { get; set; }
    }
}

