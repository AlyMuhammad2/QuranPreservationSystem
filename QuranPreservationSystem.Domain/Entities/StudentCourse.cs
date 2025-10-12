using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuranPreservationSystem.Domain.Entities
{
    /// <summary>
    /// جدول وسيط للعلاقة Many-to-Many بين الطالب والدورة
    /// يحتوي على معلومات إضافية عن تسجيل الطالب في الدورة
    /// </summary>
    public class StudentCourse
    {
        [Key]
        public int StudentCourseId { get; set; }

        // Foreign Keys
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        // معلومات إضافية عن التسجيل
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? Status { get; set; } // حالة التسجيل (نشط، مكتمل، منسحب، إلخ)

        [Range(0, 100)]
        public decimal? Grade { get; set; } // الدرجة أو التقييم

        [Range(0, 100)]
        public int? AttendancePercentage { get; set; } // نسبة الحضور

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime? CompletionDate { get; set; } // تاريخ إنهاء الدورة

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        /// <summary>
        /// الطالب المسجل
        /// </summary>
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        /// <summary>
        /// الدورة المسجل فيها
        /// </summary>
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;
    }
}

