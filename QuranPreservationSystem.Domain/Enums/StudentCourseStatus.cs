using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Enums
{
    /// <summary>
    /// حالة تسجيل الطالب في الدورة
    /// </summary>
    public enum StudentCourseStatus
    {
        [Display(Name = "نشط")]
        Active = 1,

        [Display(Name = "مكتمل")]
        Completed = 2,

        [Display(Name = "منسحب")]
        Withdrawn = 3,

        [Display(Name = "متأخر")]
        Late = 4,

        [Display(Name = "معلق")]
        Suspended = 5
    }
}

