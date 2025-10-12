using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO للمركز - لنقل البيانات بين الطبقات
    /// </summary>
    public class CenterDto
    {
        public int CenterId { get; set; }
        
        [Display(Name = "اسم المركز")]
        public string Name { get; set; } = string.Empty;
        
        [Display(Name = "العنوان")]
        public string? Address { get; set; }
        
        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "الوصف")]
        public string? Description { get; set; }
        
        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
        
        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; }
        
        // معلومات إحصائية
        public int TeachersCount { get; set; }
        public int StudentsCount { get; set; }
        public int CoursesCount { get; set; }
    }

    public class CreateCenterDto
    {
        [Required(ErrorMessage = "اسم المركز مطلوب")]
        [StringLength(200, ErrorMessage = "اسم المركز يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "اسم المركز")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "العنوان يجب أن لا يتجاوز 500 حرف")]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }
        
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20)]
        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }
        
        [StringLength(1000, ErrorMessage = "الوصف يجب أن لا يتجاوز 1000 حرف")]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }
    }

    public class UpdateCenterDto
    {
        public int CenterId { get; set; }
        
        [Required(ErrorMessage = "اسم المركز مطلوب")]
        [StringLength(200, ErrorMessage = "اسم المركز يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "اسم المركز")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "العنوان يجب أن لا يتجاوز 500 حرف")]
        [Display(Name = "العنوان")]
        public string? Address { get; set; }
        
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [StringLength(20)]
        [Display(Name = "رقم الهاتف")]
        public string? PhoneNumber { get; set; }
        
        [StringLength(1000, ErrorMessage = "الوصف يجب أن لا يتجاوز 1000 حرف")]
        [Display(Name = "الوصف")]
        public string? Description { get; set; }
        
        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
    }
}

