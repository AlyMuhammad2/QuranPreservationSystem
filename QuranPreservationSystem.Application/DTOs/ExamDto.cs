using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO لعرض بيانات الاختبار
    /// </summary>
    public class ExamDto
    {
        public int ExamId { get; set; }

        [Display(Name = "اسم الاختبار")]
        public string ExamName { get; set; } = string.Empty;

        [Display(Name = "الوصف")]
        public string? Description { get; set; }

        [Display(Name = "نوع الاختبار")]
        public string ExamType { get; set; } = string.Empty;

        [Display(Name = "المستوى")]
        public string Level { get; set; } = string.Empty;

        [Display(Name = "الدرجة الكاملة")]
        public int? TotalMarks { get; set; }

        [Display(Name = "درجة النجاح")]
        public int? PassingMarks { get; set; }

        [Display(Name = "وقت البداية")]
        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }

        [Display(Name = "وقت النهاية")]
        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        [Display(Name = "مكان الاختبار")]
        public string? Location { get; set; }

        [Display(Name = "التعليمات")]
        public string? Instructions { get; set; }

        [Display(Name = "اسم ملف PDF")]
        public string? PdfFileName { get; set; }

        [Display(Name = "حجم الملف")]
        public string? FileSizeFormatted { get; set; }

        [Display(Name = "المركز")]
        public int CenterId { get; set; }

        [Display(Name = "اسم المركز")]
        public string? CenterName { get; set; }

        [Display(Name = "الدورة")]
        public int CourseId { get; set; }

        [Display(Name = "اسم الدورة")]
        public string? CourseName { get; set; }

        [Display(Name = "الملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "آخر تعديل")]
        [DataType(DataType.Date)]
        public DateTime? LastModifiedDate { get; set; }
    }

    /// <summary>
    /// DTO لإنشاء اختبار جديد
    /// </summary>
    public class CreateExamDto
    {
        [Required(ErrorMessage = "اسم الاختبار مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الاختبار يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "اسم الاختبار")]
        public string ExamName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "وصف الاختبار يجب أن لا يتجاوز 1000 حرف")]
        [Display(Name = "وصف الاختبار")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "نوع الاختبار مطلوب")]
        [StringLength(50, ErrorMessage = "نوع الاختبار يجب أن لا يتجاوز 50 حرف")]
        [Display(Name = "نوع الاختبار")]
        public string ExamType { get; set; } = string.Empty;

        [Required(ErrorMessage = "المستوى مطلوب")]
        [StringLength(50, ErrorMessage = "المستوى يجب أن لا يتجاوز 50 حرف")]
        [Display(Name = "المستوى")]
        public string Level { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "الدرجة الكاملة يجب أن تكون بين 1 و 1000")]
        [Display(Name = "الدرجة الكاملة")]
        public int? TotalMarks { get; set; }

        [Range(1, 1000, ErrorMessage = "درجة النجاح يجب أن تكون بين 1 و 1000")]
        [Display(Name = "درجة النجاح")]
        public int? PassingMarks { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "وقت البداية")]
        public TimeSpan? StartTime { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "وقت النهاية")]
        public TimeSpan? EndTime { get; set; }

        [StringLength(200, ErrorMessage = "مكان الاختبار يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "مكان الاختبار")]
        public string? Location { get; set; }

        [StringLength(1000, ErrorMessage = "التعليمات يجب أن لا تتجاوز 1000 حرف")]
        [Display(Name = "التعليمات")]
        public string? Instructions { get; set; }

        [Required(ErrorMessage = "المركز مطلوب")]
        [Display(Name = "المركز")]
        public int CenterId { get; set; }

        [Required(ErrorMessage = "الدورة مطلوبة")]
        [Display(Name = "الدورة")]
        public int CourseId { get; set; }

        [Display(Name = "ملف PDF")]
        [DataType(DataType.Upload)]
        public IFormFile? PdfFile { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات يجب أن لا تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات إضافية")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO لتحديث الاختبار
    /// </summary>
    public class UpdateExamDto
    {
        public int ExamId { get; set; }

        [Required(ErrorMessage = "اسم الاختبار مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الاختبار يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "اسم الاختبار")]
        public string ExamName { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "وصف الاختبار يجب أن لا يتجاوز 1000 حرف")]
        [Display(Name = "وصف الاختبار")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "نوع الاختبار مطلوب")]
        [StringLength(50, ErrorMessage = "نوع الاختبار يجب أن لا يتجاوز 50 حرف")]
        [Display(Name = "نوع الاختبار")]
        public string ExamType { get; set; } = string.Empty;

        [Required(ErrorMessage = "المستوى مطلوب")]
        [StringLength(50, ErrorMessage = "المستوى يجب أن لا يتجاوز 50 حرف")]
        [Display(Name = "المستوى")]
        public string Level { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "الدرجة الكاملة يجب أن تكون بين 1 و 1000")]
        [Display(Name = "الدرجة الكاملة")]
        public int? TotalMarks { get; set; }

        [Range(1, 1000, ErrorMessage = "درجة النجاح يجب أن تكون بين 1 و 1000")]
        [Display(Name = "درجة النجاح")]
        public int? PassingMarks { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "وقت البداية")]
        public TimeSpan? StartTime { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "وقت النهاية")]
        public TimeSpan? EndTime { get; set; }

        [StringLength(200, ErrorMessage = "مكان الاختبار يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "مكان الاختبار")]
        public string? Location { get; set; }

        [StringLength(1000, ErrorMessage = "التعليمات يجب أن لا تتجاوز 1000 حرف")]
        [Display(Name = "التعليمات")]
        public string? Instructions { get; set; }

        [Required(ErrorMessage = "المركز مطلوب")]
        [Display(Name = "المركز")]
        public int CenterId { get; set; }

        [Required(ErrorMessage = "الدورة مطلوبة")]
        [Display(Name = "الدورة")]
        public int CourseId { get; set; }

        [Display(Name = "ملف PDF جديد")]
        [DataType(DataType.Upload)]
        public IFormFile? PdfFile { get; set; }

        [Display(Name = "ملف PDF الحالي")]
        public string? CurrentPdfFileName { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات يجب أن لا تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات إضافية")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
    }
}
