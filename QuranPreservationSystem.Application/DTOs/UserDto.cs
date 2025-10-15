using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs;

public class UserDto
{
    public string? UserId { get; set; }

    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [StringLength(200, ErrorMessage = "الاسم الكامل يجب ألا يتجاوز 200 حرف")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [StringLength(100, ErrorMessage = "اسم المستخدم يجب ألا يتجاوز 100 حرف")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "الدور مطلوب")]
    public string Role { get; set; } = string.Empty;

    public string? RoleDisplayName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    // For Create/Edit
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقتين")]
    public string? ConfirmPassword { get; set; }
}

public class CreateUserDto
{
    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "الدور مطلوب")]
    public string Role { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقتين")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class EditUserDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "الدور مطلوب")]
    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Optional password change
    [StringLength(100, MinimumLength = 6, ErrorMessage = "كلمة المرور يجب أن تكون بين 6 و 100 حرف")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقتين")]
    public string? ConfirmNewPassword { get; set; }
}

