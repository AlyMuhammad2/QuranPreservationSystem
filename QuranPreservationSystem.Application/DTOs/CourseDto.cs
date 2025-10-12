using QuranPreservationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO لعرض بيانات الدورة
    /// </summary>
    public class CourseDto
    {
        public int CourseId { get; set; }

        [Display(Name = "اسم الدورة")]
        public string CourseName { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "نوع الدورة")]
        public CourseType CourseType { get; set; }

        [Display(Name = "المستوى")]
        public string? Level { get; set; }

        [Display(Name = "تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاريخ النهاية")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "الجدول الزمني")]
        public string? Schedule { get; set; }

        [Display(Name = "الحد الأقصى للطلاب")]
        public int? MaxStudents { get; set; }

        [Display(Name = "عدد الساعات")]
        public int? DurationHours { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; }
        
        // معلومات المركز
        public int CenterId { get; set; }
        
        [Display(Name = "المركز")]
        public string? CenterName { get; set; }
        
        // معلومات المدرس
        public int TeacherId { get; set; }
        
        [Display(Name = "المدرس")]
        public string? TeacherName { get; set; }
        
        // معلومات إحصائية
        [Display(Name = "عدد الطلاب المسجلين")]
        public int EnrolledStudentsCount { get; set; }
        
        [Display(Name = "المقاعد المتاحة")]
        public int AvailableSeats { get; set; }
    }

    /// <summary>
    /// DTO لإضافة دورة جديدة
    /// </summary>
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "اسم الدورة مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الدورة لا يجب أن يتجاوز 200 حرف")]
        [Display(Name = "اسم الدورة")]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "الوصف لا يجب أن يتجاوز 1000 حرف")]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "نوع الدورة مطلوب")]
        [Display(Name = "نوع الدورة")]
        public CourseType CourseType { get; set; }

        [StringLength(50, ErrorMessage = "المستوى لا يجب أن يتجاوز 50 حرف")]
        [Display(Name = "المستوى")]
        public string? Level { get; set; }

        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        [Display(Name = "تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاريخ النهاية")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [StringLength(50, ErrorMessage = "الجدول الزمني لا يجب أن يتجاوز 50 حرف")]
        [Display(Name = "الجدول الزمني")]
        public string? Schedule { get; set; }

        [Range(1, 1000, ErrorMessage = "الحد الأقصى للطلاب يجب أن يكون بين 1 و 1000")]
        [Display(Name = "الحد الأقصى للطلاب")]
        public int? MaxStudents { get; set; }

        [Range(1, 100, ErrorMessage = "عدد الساعات يجب أن يكون بين 1 و 100")]
        [Display(Name = "عدد الساعات")]
        public int? DurationHours { get; set; }

        [Required(ErrorMessage = "المركز مطلوب")]
        [Display(Name = "المركز")]
        public int CenterId { get; set; }

        [Required(ErrorMessage = "المدرس مطلوب")]
        [Display(Name = "المدرس")]
        public int TeacherId { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات لا يجب أن تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO لتعديل بيانات الدورة
    /// </summary>
    public class UpdateCourseDto
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "اسم الدورة مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الدورة لا يجب أن يتجاوز 200 حرف")]
        [Display(Name = "اسم الدورة")]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "الوصف لا يجب أن يتجاوز 1000 حرف")]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "نوع الدورة مطلوب")]
        [Display(Name = "نوع الدورة")]
        public CourseType CourseType { get; set; }

        [StringLength(50, ErrorMessage = "المستوى لا يجب أن يتجاوز 50 حرف")]
        [Display(Name = "المستوى")]
        public string? Level { get; set; }

        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        [Display(Name = "تاريخ البداية")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "تاريخ النهاية")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [StringLength(50, ErrorMessage = "الجدول الزمني لا يجب أن يتجاوز 50 حرف")]
        [Display(Name = "الجدول الزمني")]
        public string? Schedule { get; set; }

        [Range(1, 1000, ErrorMessage = "الحد الأقصى للطلاب يجب أن يكون بين 1 و 1000")]
        [Display(Name = "الحد الأقصى للطلاب")]
        public int? MaxStudents { get; set; }

        [Range(1, 100, ErrorMessage = "عدد الساعات يجب أن يكون بين 1 و 100")]
        [Display(Name = "عدد الساعات")]
        public int? DurationHours { get; set; }

        [Required(ErrorMessage = "المركز مطلوب")]
        [Display(Name = "المركز")]
        public int CenterId { get; set; }

        [Required(ErrorMessage = "المدرس مطلوب")]
        [Display(Name = "المدرس")]
        public int TeacherId { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات لا يجب أن تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
    }
}
