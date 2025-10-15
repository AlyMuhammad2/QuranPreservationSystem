using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Entities
{
    /// <summary>
    /// جدول مؤقت لاستيراد بيانات الطلاب - مطابق لـ Student Entity
    /// </summary>
    public class TempStudentImport
    {
        [Key]
        public int TempId { get; set; }

        // ========== الحقول المطلوبة (من Student Entity) ==========
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string CenterName { get; set; } = string.Empty; // اسم المركز (سيتم البحث عنه)

        public DateTime? DateOfBirth { get; set; } // مطلوب في Student

        [StringLength(10)]
        public string? Gender { get; set; } // "ذكر" أو "أنثى" - مطلوب في Student

        // ========== الحقول الاختيارية (من Student Entity) ==========
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public DateTime? EnrollmentDate { get; set; } // تاريخ التسجيل

        [StringLength(100)]
        public string? EducationLevel { get; set; } // المستوى التعليمي

        [StringLength(1000)]
        public string? Notes { get; set; } // ملاحظات

        public bool IsActive { get; set; } = true;

        // ========== حالة المعالجة ==========
        public ImportStatus Status { get; set; } = ImportStatus.Pending;

        [StringLength(500)]
        public string? ErrorMessage { get; set; }

        // ========== معلومات الرفع والتتبع ==========
        public string? UploadedBy { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        public DateTime? ProcessedDate { get; set; }

        public int? ProcessedStudentId { get; set; }

        public int RowNumber { get; set; }

        public string? BatchId { get; set; }
    }
}

