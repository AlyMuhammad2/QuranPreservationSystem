using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuranPreservationSystem.Domain.Entities
{
    /// <summary>
    /// نموذج المدرس - يمثل مدرس في الجمعية
    /// </summary>
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "اسم المدرس مطلوب")]
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

        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; } // "ذكر" أو "أنثى"

        [StringLength(100)]
        public string? Qualification { get; set; } // المؤهل العلمي

        [StringLength(200)]
        public string? Specialization { get; set; } // التخصص (حفظ، تجويد، إلخ)

        public DateTime HireDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Foreign Keys
        [Required(ErrorMessage = "المركز مطلوب")]
        public int CenterId { get; set; }

        // Navigation Properties
        /// <summary>
        /// المركز التابع له المدرس
        /// </summary>
        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; } = null!;

        /// <summary>
        /// الدورات التي يدرسها هذا المدرس
        /// </summary>
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

