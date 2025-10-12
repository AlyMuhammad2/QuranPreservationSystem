namespace QuranPreservationSystem.Domain.Enums
{
    /// <summary>
    /// حالة تسجيل الطالب في الدورة
    /// </summary>
    public enum StudentCourseStatus
    {
        /// <summary>
        /// نشط
        /// </summary>
        Active = 1,

        /// <summary>
        /// مكتمل
        /// </summary>
        Completed = 2,

        /// <summary>
        /// منسحب
        /// </summary>
        Withdrawn = 3,

        /// <summary>
        /// متأخر
        /// </summary>
        Late = 4,

        /// <summary>
        /// معلق
        /// </summary>
        Suspended = 5
    }
}

