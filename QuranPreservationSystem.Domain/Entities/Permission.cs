using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Entities;

public class Permission
{
    [Key]
    public int PermissionId { get; set; }

    [Required]
    [StringLength(100)]
    public string PermissionName { get; set; } = string.Empty; // مثل: "Dashboard", "Centers", "Teachers", etc.

    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; } = string.Empty; // الاسم العربي: "لوحة التحكم", "المراكز", "المدرسين"

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; } // FontAwesome icon class

    public int DisplayOrder { get; set; } = 0; // ترتيب العرض

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

