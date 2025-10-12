using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Models
{
    /// <summary>
    /// نموذج المركز - يمثل مركز من مراكز الجمعية
    /// </summary>
    public class Center
    {
        [Key]
        public int CenterId { get; set; }

        [Required(ErrorMessage = "اسم المركز مطلوب")]
        [StringLength(200, ErrorMessage = "اسم المركز يجب أن لا يتجاوز 200 حرف")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "العنوان يجب أن لا يتجاوز 500 حرف")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        /// <summary>
        /// الدورات التابعة لهذا المركز
        /// </summary>
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        /// <summary>
        /// المدرسين التابعين لهذا المركز
        /// </summary>
        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

        /// <summary>
        /// الطلاب التابعين لهذا المركز
        /// </summary>
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}

