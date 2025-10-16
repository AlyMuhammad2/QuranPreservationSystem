using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs;

/// <summary>
/// الملف الشخصي للمستخدم
/// </summary>
public class ProfileDto
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [StringLength(50, ErrorMessage = "اسم المستخدم يجب أن يكون بين {2} و {1} حرفاً", MinimumLength = 3)]
    [Display(Name = "اسم المستخدم")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    [Display(Name = "البريد الإلكتروني")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "الاسم الكامل مطلوب")]
    [StringLength(100, ErrorMessage = "الاسم الكامل يجب أن يكون بين {2} و {1} حرفاً", MinimumLength = 3)]
    [Display(Name = "الاسم الكامل")]
    public string FullName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    [Display(Name = "رقم الهاتف")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "الدور")]
    public string Role { get; set; } = string.Empty;

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// تغيير كلمة المرور
/// </summary>
public class ChangePasswordDto
{
    [Required(ErrorMessage = "كلمة المرور الحالية مطلوبة")]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور الحالية")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
    [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تكون بين {2} و {1} حرفاً", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور الجديدة")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيدها غير متطابقتين")]
    [Display(Name = "تأكيد كلمة المرور")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

