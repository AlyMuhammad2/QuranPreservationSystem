using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuranPreservationSystem.Domain.Entities;

public class RolePermission
{
    [Key]
    public int RolePermissionId { get; set; }

    [Required]
    [StringLength(450)] // نفس طول IdentityRole.Id
    public string RoleId { get; set; } = string.Empty;

    [Required]
    public int PermissionId { get; set; }

    public bool CanView { get; set; } = true;
    public bool CanCreate { get; set; } = false;
    public bool CanEdit { get; set; } = false;
    public bool CanDelete { get; set; } = false;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation properties
    [ForeignKey(nameof(PermissionId))]
    public Permission Permission { get; set; } = null!;
}

