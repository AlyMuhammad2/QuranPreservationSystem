using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs;

public class PermissionDto
{
    public int PermissionId { get; set; }

    [Required(ErrorMessage = "اسم الصلاحية مطلوب")]
    public string PermissionName { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم المعروض مطلوب")]
    public string DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
}

public class RolePermissionDto
{
    public int RolePermissionId { get; set; }
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}

public class ManageRolePermissionsDto
{
    public string RoleId { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public List<PermissionAssignment> Permissions { get; set; } = new();
}

public class PermissionAssignment
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public bool IsAssigned { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}

