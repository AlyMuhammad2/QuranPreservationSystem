using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QuranPreservationSystem.Domain.Enums;

namespace QuranPreservationSystem.Domain.Entities
{
    /// <summary>
    /// ديوان الحفاظ - سجل الطلاب الذين أتموا حفظ القرآن الكريم
    /// </summary>
    public class HafizRegistry
    {
        [Key]
        public int HafizId { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الطالب يجب أن لا يتجاوز 200 حرف")]
        public string StudentName { get; set; } = string.Empty;

        public int? CenterId { get; set; }

        [Required(ErrorMessage = "سنة الإتمام مطلوبة")]
        [Range(1900, 2100, ErrorMessage = "سنة الإتمام يجب أن تكون بين 1900 و 2100")]
        public int CompletionYear { get; set; }

        [StringLength(1000, ErrorMessage = "الدورات يجب أن لا تتجاوز 1000 حرف")]
        public string? CompletedCourses { get; set; } // الدورات الحاصل عليها (نص مفصول بفواصل)

        [StringLength(500, ErrorMessage = "مسار الشهادة يجب أن لا يتجاوز 500 حرف")]
        public string? CertificatePath { get; set; } // مسار ملف PDF للشهادة

        [StringLength(100, ErrorMessage = "اسم الملف يجب أن لا يتجاوز 100 حرف")]
        public string? CertificateFileName { get; set; } // اسم ملف الشهادة الأصلي

        public long? CertificateFileSize { get; set; } // حجم الملف بالبايت

        [StringLength(20, ErrorMessage = "نوع الملف يجب أن لا يتجاوز 20 حرف")]
        public string? CertificateFileType { get; set; } // نوع الملف (MIME type)

        [StringLength(500, ErrorMessage = "مسار الصورة يجب أن لا يتجاوز 500 حرف")]
        public string? PhotoPath { get; set; } // مسار صورة الطالب

        public MemorizationLevel? MemorizationLevel { get; set; } // مستوى الحفظ (الربع، النصف، كاملاً)

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastModifiedDate { get; set; }

        // Navigation Properties
        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; } = null!;
    }
}

