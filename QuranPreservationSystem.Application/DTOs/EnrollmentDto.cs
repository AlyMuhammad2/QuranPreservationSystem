using QuranPreservationSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Application.DTOs
{
    /// <summary>
    /// DTO لعرض بيانات التسجيل (StudentCourse)
    /// </summary>
    public class EnrollmentDto
    {
        public int StudentCourseId { get; set; }

        public int StudentId { get; set; }
        
        [Display(Name = "اسم الطالب")]
        public string? StudentName { get; set; }

        public int CourseId { get; set; }
        
        [Display(Name = "اسم الدورة")]
        public string? CourseName { get; set; }

        [Display(Name = "نوع الدورة")]
        public CourseType? CourseType { get; set; }

        [Display(Name = "المدرس")]
        public string? TeacherName { get; set; }

        [Display(Name = "المركز")]
        public string? CenterName { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "تاريخ الاختبار")]
        [DataType(DataType.Date)]
        public DateTime? ExamDate { get; set; }

        [Display(Name = "الحالة")]
        public StudentCourseStatus Status { get; set; }

        [Display(Name = "الدرجة")]
        public decimal? Grade { get; set; }

        [Display(Name = "نسبة الحضور")]
        public int? AttendancePercentage { get; set; }

        [Display(Name = "الممتحن الأول")]
        public string? Examiner1 { get; set; }

        [Display(Name = "الممتحن الثاني")]
        public string? Examiner2 { get; set; }

        [Display(Name = "تاريخ الإكمال")]
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO لإنشاء تسجيل جديد
    /// </summary>
    public class CreateEnrollmentDto
    {
        [Required(ErrorMessage = "الطالب مطلوب")]
        [Display(Name = "الطالب")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "الدورة مطلوبة")]
        [Display(Name = "الدورة")]
        public int CourseId { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [Display(Name = "تاريخ الاختبار")]
        [DataType(DataType.Date)]
        public DateTime? ExamDate { get; set; }

        [Display(Name = "الحالة")]
        public StudentCourseStatus Status { get; set; } = StudentCourseStatus.Active;

        [Range(0, 100, ErrorMessage = "الدرجة يجب أن تكون بين 0 و 100")]
        [Display(Name = "الدرجة")]
        public decimal? Grade { get; set; }

        [Range(0, 100, ErrorMessage = "نسبة الحضور يجب أن تكون بين 0 و 100")]
        [Display(Name = "نسبة الحضور")]
        public int? AttendancePercentage { get; set; }

        [StringLength(100, ErrorMessage = "اسم الممتحن لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "الممتحن الأول")]
        public string? Examiner1 { get; set; }

        [StringLength(100, ErrorMessage = "اسم الممتحن لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "الممتحن الثاني")]
        public string? Examiner2 { get; set; }

        [Display(Name = "تاريخ الإكمال")]
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات لا يجب أن تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// DTO لتحديث التسجيل
    /// </summary>
    public class UpdateEnrollmentDto
    {
        public int StudentCourseId { get; set; }

        public int StudentId { get; set; }

        public int CourseId { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "تاريخ الاختبار")]
        [DataType(DataType.Date)]
        public DateTime? ExamDate { get; set; }

        [Display(Name = "الحالة")]
        public StudentCourseStatus Status { get; set; }

        [Range(0, 100, ErrorMessage = "الدرجة يجب أن تكون بين 0 و 100")]
        [Display(Name = "الدرجة")]
        public decimal? Grade { get; set; }

        [Range(0, 100, ErrorMessage = "نسبة الحضور يجب أن تكون بين 0 و 100")]
        [Display(Name = "نسبة الحضور")]
        public int? AttendancePercentage { get; set; }

        [StringLength(100, ErrorMessage = "اسم الممتحن لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "الممتحن الأول")]
        public string? Examiner1 { get; set; }

        [StringLength(100, ErrorMessage = "اسم الممتحن لا يجب أن يتجاوز 100 حرف")]
        [Display(Name = "الممتحن الثاني")]
        public string? Examiner2 { get; set; }

        [Display(Name = "تاريخ الإكمال")]
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [StringLength(1000, ErrorMessage = "الملاحظات لا يجب أن تتجاوز 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }

        [Display(Name = "نشط")]
        public bool IsActive { get; set; }
    }
}

