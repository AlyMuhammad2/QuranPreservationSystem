using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Entities
{
    /// <summary>
    /// جدول مؤقت لاستيراد بيانات المراكز
    /// </summary>
    public class TempCenterImport
    {
        [Key]
        public int TempId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        // حالة المعالجة
        public ImportStatus Status { get; set; } = ImportStatus.Pending;

        [StringLength(500)]
        public string? ErrorMessage { get; set; }

        // معلومات الرفع
        public string? UploadedBy { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        public DateTime? ProcessedDate { get; set; }

        public int? ProcessedCenterId { get; set; } // المركز المضاف بعد المعالجة

        // معلومات إضافية للتتبع
        public int RowNumber { get; set; } // رقم الصف في Excel

        public string? BatchId { get; set; } // معرّف الدفعة
    }

    /// <summary>
    /// حالة استيراد البيانات
    /// </summary>
    public enum ImportStatus
    {
        Pending = 1,      // في الانتظار
        Processing = 2,   // قيد المعالجة
        Completed = 3,    // تم بنجاح
        Failed = 4,       // فشل
        Duplicate = 5     // مكرر
    }
}

