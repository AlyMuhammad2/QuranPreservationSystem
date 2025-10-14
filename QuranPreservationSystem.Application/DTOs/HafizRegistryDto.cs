using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using QuranPreservationSystem.Domain.Enums;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO لعرض بيانات ديوان الحفاظ
    /// </summary>
    public class HafizRegistryDto
    {
        public int HafizId { get; set; }

        [Display(Name = "اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        public int? CenterId { get; set; }

        [Display(Name = "المركز")]
        public string? CenterName { get; set; }

        [Display(Name = "سنة الإتمام")]
        public int CompletionYear { get; set; }

        [Display(Name = "الدورات المكتملة")]
        public string? CompletedCourses { get; set; }

        [Display(Name = "اسم ملف الشهادة")]
        public string? CertificateFileName { get; set; }

        [Display(Name = "حجم الملف")]
        public string? FileSizeFormatted { get; set; }

        [Display(Name = "صورة الطالب")]
        public string? PhotoPath { get; set; }

        [Display(Name = "مستوى الحفظ")]
        public MemorizationLevel? MemorizationLevel { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "آخر تعديل")]
        public DateTime? LastModifiedDate { get; set; }
    }

    /// <summary>
    /// DTO لإضافة حافظ جديد
    /// </summary>
    public class CreateHafizRegistryDto
    {
        [Display(Name = "اختر طالب موجود")]
        public int? StudentId { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الطالب يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        [Display(Name = "المركز")]
        public int? CenterId { get; set; }

        [Required(ErrorMessage = "سنة الإتمام مطلوبة")]
        [Range(1900, 2100, ErrorMessage = "سنة الإتمام يجب أن تكون بين 1900 و 2100")]
        [Display(Name = "سنة الإتمام")]
        public int CompletionYear { get; set; }

        [Display(Name = "الدورات المكتملة")]
        public List<int>? CompletedCourseIds { get; set; }

        [Display(Name = "مستوى الحفظ")]
        public MemorizationLevel? MemorizationLevel { get; set; }

        [Display(Name = "ملف الشهادة (PDF)")]
        [DataType(DataType.Upload)]
        public IFormFile? CertificateFile { get; set; }

        [Display(Name = "صورة الطالب")]
        [DataType(DataType.Upload)]
        public IFormFile? PhotoFile { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO لتحديث بيانات الحافظ
    /// </summary>
    public class UpdateHafizRegistryDto
    {
        public int HafizId { get; set; }

        [Required(ErrorMessage = "اسم الطالب مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الطالب يجب أن لا يتجاوز 200 حرف")]
        [Display(Name = "اسم الطالب")]
        public string StudentName { get; set; } = string.Empty;

        [Display(Name = "المركز")]
        public int? CenterId { get; set; }

        [Required(ErrorMessage = "سنة الإتمام مطلوبة")]
        [Range(1900, 2100, ErrorMessage = "سنة الإتمام يجب أن تكون بين 1900 و 2100")]
        [Display(Name = "سنة الإتمام")]
        public int CompletionYear { get; set; }

        [Display(Name = "الدورات المكتملة")]
        public List<int>? CompletedCourseIds { get; set; }

        [Display(Name = "مستوى الحفظ")]
        public MemorizationLevel? MemorizationLevel { get; set; }

        [Display(Name = "ملف الشهادة الجديد (PDF)")]
        [DataType(DataType.Upload)]
        public IFormFile? CertificateFile { get; set; }

        [Display(Name = "الشهادة الحالية")]
        public string? CurrentCertificateFileName { get; set; }

        [Display(Name = "صورة جديدة")]
        [DataType(DataType.Upload)]
        public IFormFile? PhotoFile { get; set; }

        [Display(Name = "الصورة الحالية")]
        public string? CurrentPhotoPath { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
    }
}

