using System.ComponentModel.DataAnnotations;
using QuranPreservationSystem.Domain.Enums;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO للمدرس - لنقل البيانات بين الطبقات
    /// </summary>
    public class TeacherDto
    {
        public int TeacherId { get; set; }
        
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;
        
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;
        
        public string FullName => $"{FirstName} {LastName}";
        
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }
        
        [Display(Name = "العنوان")]
        public string? Address { get; set; }
        
        [Display(Name = "تاريخ الميلاد")]
        public DateTime? DateOfBirth { get; set; }
        
        [Display(Name = "الجنس")]
        public Gender? Gender { get; set; }
        
        [Display(Name = "المؤهل العلمي")]
        public string? Qualification { get; set; }
        
        [Display(Name = "التخصص")]
        public string? Specialization { get; set; }
        
        [Display(Name = "تاريخ التعيين")]
        public DateTime HireDate { get; set; }
        
        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
        
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
        
        // معلومات المركز
        public int CenterId { get; set; }
        
        [Display(Name = "المركز")]
        public string? CenterName { get; set; }
        
        // معلومات إحصائية
        public int CoursesCount { get; set; }
    }

    public class CreateTeacherDto
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن لا يتجاوز 100 حرف")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم العائلة يجب أن لا يتجاوز 100 حرف")]
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20)]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100)]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [StringLength(500)]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "الجنس")]
        public Gender? Gender { get; set; }

        [StringLength(100)]
        [Display(Name = "المؤهل العلمي")]
        public string? Qualification { get; set; }

        [StringLength(200)]
        [Display(Name = "التخصص")]
        public string? Specialization { get; set; }

        [StringLength(1000)]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "المركز مطلوب")]
        [Display(Name = "المركز")]
        public int CenterId { get; set; }
    }

    public class UpdateTeacherDto
    {
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن لا يتجاوز 100 حرف")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم العائلة يجب أن لا يتجاوز 100 حرف")]
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20)]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100)]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [StringLength(500)]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "الجنس")]
        public Gender? Gender { get; set; }

        [StringLength(100)]
        [Display(Name = "المؤهل العلمي")]
        public string? Qualification { get; set; }

        [StringLength(200)]
        [Display(Name = "التخصص")]
        public string? Specialization { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [StringLength(1000)]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "المركز مطلوب")]
        [Display(Name = "المركز")]
        public int CenterId { get; set; }
    }
}
