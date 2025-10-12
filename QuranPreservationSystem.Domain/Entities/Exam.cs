using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuranPreservationSystem.Domain.Entities
{
    /// <summary>
    /// كيان الاختبار - إدارة اختبارات التجويد
    /// </summary>
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }

        [Required(ErrorMessage = "اسم الاختبار مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الاختبار يجب أن لا يتجاوز 200 حرف")]
        public string ExamName { get; set; } = string.Empty; // اسم الاختبار

        [StringLength(1000, ErrorMessage = "وصف الاختبار يجب أن لا يتجاوز 1000 حرف")]
        public string? Description { get; set; } // وصف الاختبار

        [Required(ErrorMessage = "نوع الاختبار مطلوب")]
        [StringLength(50, ErrorMessage = "نوع الاختبار يجب أن لا يتجاوز 50 حرف")]
        public string ExamType { get; set; } = string.Empty; // نوع الاختبار (شفوي، تحريري، عملي)

        [Required(ErrorMessage = "المستوى مطلوب")]
        [StringLength(50, ErrorMessage = "المستوى يجب أن لا يتجاوز 50 حرف")]
        public string Level { get; set; } = string.Empty; // المستوى (تمهيدي، متوسط، متقدم)

        [Range(1, 1000, ErrorMessage = "الدرجة الكاملة يجب أن تكون بين 1 و 1000")]
        public int? TotalMarks { get; set; } // الدرجة الكاملة للاختبار (اختيارية)

        [Range(1, 1000, ErrorMessage = "درجة النجاح يجب أن تكون بين 1 و 1000")]
        public int? PassingMarks { get; set; } // درجة النجاح (اختيارية)

        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; } // وقت بداية الاختبار

        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; } // وقت انتهاء الاختبار

        [StringLength(200, ErrorMessage = "مكان الاختبار يجب أن لا يتجاوز 200 حرف")]
        public string? Location { get; set; } // مكان الاختبار

        [StringLength(1000, ErrorMessage = "التعليمات يجب أن لا تتجاوز 1000 حرف")]
        public string? Instructions { get; set; } // تعليمات الاختبار

        [StringLength(500, ErrorMessage = "مسار ملف PDF يجب أن لا يتجاوز 500 حرف")]
        public string? PdfFilePath { get; set; } // مسار ملف PDF للاختبار

        [StringLength(100, ErrorMessage = "اسم ملف PDF يجب أن لا يتجاوز 100 حرف")]
        public string? PdfFileName { get; set; } // اسم ملف PDF الأصلي

        [StringLength(20, ErrorMessage = "نوع ملف PDF يجب أن لا يتجاوز 20 حرف")]
        public string? PdfFileType { get; set; } // نوع ملف PDF (MIME type)

        public long? PdfFileSize { get; set; } // حجم ملف PDF بالبايت

        [Required(ErrorMessage = "المركز مطلوب")]
        public int CenterId { get; set; } // معرف المركز

        [Required(ErrorMessage = "الدورة مطلوبة")]
        public int CourseId { get; set; } // معرف الدورة

        [StringLength(1000, ErrorMessage = "الملاحظات يجب أن لا تتجاوز 1000 حرف")]
        public string? Notes { get; set; } // ملاحظات إضافية

        public bool IsActive { get; set; } = true; // نشط

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now; // تاريخ الإنشاء

        public DateTime? LastModifiedDate { get; set; } // تاريخ آخر تعديل

        // Navigation Properties
        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; } = null!;

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;
    }
}
