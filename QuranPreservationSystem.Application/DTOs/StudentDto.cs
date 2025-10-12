using QuranPreservationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO لعرض بيانات الطالب
    /// </summary>
    public class StudentDto
    {
        public int StudentId { get; set; }

        [Display(Name = "الاسم الكامل")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Display(Name = "الجنس")]
        public Gender Gender { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [Display(Name = "رقم الهوية")]
        public string? IdentityNumber { get; set; }

        [Display(Name = "المركز")]
        public int? CenterId { get; set; }

        [Display(Name = "اسم المركز")]
        public string? CenterName { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "مسار الملف")]
        public string? DocumentPath { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "تاريخ آخر تحديث")]
        public DateTime? LastModifiedDate { get; set; }

        // Navigation Properties
        public List<StudentCourseDto>? StudentCourses { get; set; }
    }

    /// <summary>
    /// DTO لإضافة طالب جديد
    /// </summary>
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الأول لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم العائلة لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(20, ErrorMessage = "رقم الهاتف لا يجب أن يتجاوز 20 رقم")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100, ErrorMessage = "البريد الإلكتروني لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "الجنس مطلوب")]
        [Display(Name = "الجنس")]
        public Gender Gender { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "العنوان لا يجب أن يتجاوز 500 حرف")]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [StringLength(20, ErrorMessage = "رقم الهوية لا يجب أن يتجاوز 20 رقم")]
        [Display(Name = "رقم الهوية")]
        public string? IdentityNumber { get; set; }

        [Display(Name = "المركز")]
        public int? CenterId { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات لا يجب أن تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO لتعديل بيانات الطالب
    /// </summary>
    public class UpdateStudentDto
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الأول لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم العائلة لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [StringLength(20, ErrorMessage = "رقم الهاتف لا يجب أن يتجاوز 20 رقم")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100, ErrorMessage = "البريد الإلكتروني لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "البريد الإلكتروني")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "الجنس مطلوب")]
        [Display(Name = "الجنس")]
        public Gender Gender { get; set; }

        [Display(Name = "تاريخ الميلاد")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "العنوان لا يجب أن يتجاوز 500 حرف")]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }

        [StringLength(20, ErrorMessage = "رقم الهوية لا يجب أن يتجاوز 20 رقم")]
        [Display(Name = "رقم الهوية")]
        public string? IdentityNumber { get; set; }

        [Display(Name = "المركز")]
        public int? CenterId { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات لا يجب أن تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO لعرض بيانات دورة الطالب
    /// </summary>
    public class StudentCourseDto
    {
        public int StudentCourseId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        
        [Display(Name = "اسم الدورة")]
        public string? CourseName { get; set; }
        
        [Display(Name = "المدرس")]
        public string? TeacherName { get; set; }
        
        [Display(Name = "تاريخ التسجيل")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }
        
        [Display(Name = "تاريخ الاختبار")]
        [DataType(DataType.Date)]
        public DateTime? ExamDate { get; set; }
        
        [Display(Name = "الحالة")]
        public StudentCourseStatus Status { get; set; }
        
        [Display(Name = "اللجنة الممتحنة")]
        public string? Examiners { get; set; }
    }
}