using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Enums
{
    /// <summary>
    /// الجنس
    /// </summary>
    public enum Gender
    {
        [Display(Name = "ذكر")]
        Male = 1,

        [Display(Name = "أنثى")]
        Female = 2
    }
}

