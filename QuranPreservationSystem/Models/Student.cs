using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuranPreservationSystem.Models
{
    /// <summary>
    /// نموذج الطالب - يمثل طالب في الجمعية
    /// </summary>
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن لا يتجاوز 100 حرف")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم العائلة يجب أن لا يتجاوز 100 حرف")]
        public string LastName { get; set; } = string.Empty;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; } // "ذكر" أو "أنثى"

        [StringLength(100)]
        public string? GuardianName { get; set; } // اسم ولي الأمر

        [Phone(ErrorMessage = "رقم هاتف ولي الأمر غير صحيح")]
        [StringLength(20)]
        public string? GuardianPhone { get; set; }

        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string? EducationLevel { get; set; } // المستوى التعليمي

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Foreign Keys
        [Required(ErrorMessage = "المركز مطلوب")]
        public int CenterId { get; set; }

        // Navigation Properties
        /// <summary>
        /// المركز التابع له الطالب
        /// </summary>
        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; } = null!;

        /// <summary>
        /// الدورات المسجل فيها الطالب (علاقة Many-to-Many)
        /// </summary>
        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}

