using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Enums
{
    /// <summary>
    /// مستوى حفظ القرآن الكريم
    /// </summary>
    public enum MemorizationLevel
    {
        [Display(Name = "الربع")]
        Quarter = 1,

        [Display(Name = "النصف")]
        Half = 2,

        [Display(Name = "كاملاً")]
        Complete = 3
    }
}

