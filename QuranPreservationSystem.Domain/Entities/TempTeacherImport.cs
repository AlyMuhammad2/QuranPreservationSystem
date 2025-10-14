using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Entities
{
    /// <summary>
    /// جدول مؤقت لاستيراد بيانات المدرسين
    /// </summary>
    public class TempTeacherImport
    {
        [Key]
        public int TempId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; } // "ذكر" أو "أنثى"

        [StringLength(100)]
        public string? Qualification { get; set; }

        [StringLength(200)]
        public string? Specialization { get; set; }

        public DateTime HireDate { get; set; }

        [Required]
        [StringLength(200)]
        public string CenterName { get; set; } = string.Empty; // اسم المركز (سيتم البحث عنه)

        public bool IsActive { get; set; } = true;

        // حالة المعالجة
        public ImportStatus Status { get; set; } = ImportStatus.Pending;

        [StringLength(500)]
        public string? ErrorMessage { get; set; }

        // معلومات الرفع
        public string? UploadedBy { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        public DateTime? ProcessedDate { get; set; }

        public int? ProcessedTeacherId { get; set; } // المدرس المضاف بعد المعالجة

        // معلومات إضافية للتتبع
        public int RowNumber { get; set; } // رقم الصف في Excel

        public string? BatchId { get; set; } // معرّف الدفعة
    }
}

