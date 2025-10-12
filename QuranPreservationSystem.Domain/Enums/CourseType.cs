using System.ComponentModel.DataAnnotations;

namespace QuranPreservationSystem.Domain.Enums
{
    /// <summary>
    /// أنواع الدورات القرآنية
    /// </summary>
    public enum CourseType
    {
        [Display(Name = "تلاوة وتجويد")]
        TilawahAndTajweed = 1,

        [Display(Name = "تمهيدية")]
        Introductory = 2,

        [Display(Name = "متوسطة")]
        Intermediate = 3,

        [Display(Name = "متقدمة")]
        Advanced = 4,

        [Display(Name = "إتقان")]
        Mastery = 5,

        [Display(Name = "إجازة نظراً")]
        IjazahNazaran = 6,

        [Display(Name = "سند غيبي")]
        SanadGhaybi = 7,

        [Display(Name = "دورة طيبة")]
        Tayyibah = 8,

        [Display(Name = "إجازة طيبة")]
        IjazahTayyibah = 9,

        [Display(Name = "روايات أخرى")]
        OtherRiwayat = 10,

        [Display(Name = "القراءات العشر")]
        TenQiraah = 11
    }
}

