using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuranPreservationSystem.Models
{
    /// <summary>
    /// نموذج الدورة - يمثل دورة تعليمية في الجمعية
    /// </summary>
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "اسم الدورة مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الدورة يجب أن لا يتجاوز 200 حرف")]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? CourseType { get; set; } // نوع الدورة (حفظ، تجويد، مراجعة، إلخ)

        [StringLength(50)]
        public string? Level { get; set; } // المستوى (مبتدئ، متوسط، متقدم)

        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string? Schedule { get; set; } // الجدول الزمني (مثال: السبت والأربعاء 5-7 مساءً)

        [Range(1, 1000, ErrorMessage = "الحد الأقصى للطلاب يجب أن يكون بين 1 و 1000")]
        public int? MaxStudents { get; set; }

        [Range(1, 100, ErrorMessage = "عدد الساعات يجب أن يكون بين 1 و 100")]
        public int? DurationHours { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Foreign Keys
        [Required(ErrorMessage = "المركز مطلوب")]
        public int CenterId { get; set; }

        [Required(ErrorMessage = "المدرس مطلوب")]
        public int TeacherId { get; set; }

        // Navigation Properties
        /// <summary>
        /// المركز التابعة له الدورة
        /// </summary>
        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; } = null!;

        /// <summary>
        /// المدرس المسؤول عن الدورة
        /// </summary>
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;

        /// <summary>
        /// الطلاب المسجلين في الدورة (علاقة Many-to-Many)
        /// </summary>
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}

