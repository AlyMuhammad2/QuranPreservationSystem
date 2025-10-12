using QuranPreservationSystem.Domain.Entities;

namespace QuranPreservationSystem.Application.Interfaces.IRepositories
{
    /// <summary>
    /// واجهة مخزن الاختبارات
    /// </summary>
    public interface IExamRepository : IGenericRepository<Exam>
    {
        /// <summary>
        /// جلب جميع الاختبارات النشطة مع التفاصيل
        /// </summary>
        /// <returns>قائمة الاختبارات النشطة</returns>
        Task<IEnumerable<Exam>> GetActiveExamsAsync();

        /// <summary>
        /// جلب الاختبارات حسب المركز
        /// </summary>
        /// <param name="centerId">معرف المركز</param>
        /// <returns>قائمة اختبارات المركز</returns>
        Task<IEnumerable<Exam>> GetExamsByCenterAsync(int centerId);

        /// <summary>
        /// جلب الاختبارات حسب الدورة
        /// </summary>
        /// <param name="courseId">معرف الدورة</param>
        /// <returns>قائمة اختبارات الدورة</returns>
        Task<IEnumerable<Exam>> GetExamsByCourseAsync(int courseId);

        /// <summary>
        /// جلب الاختبارات حسب التاريخ
        /// </summary>
        /// <param name="examDate">تاريخ الاختبار</param>
        /// <returns>قائمة اختبارات التاريخ</returns>
        Task<IEnumerable<Exam>> GetExamsByDateAsync(DateTime examDate);

        /// <summary>
        /// جلب الاختبارات المستقبلية
        /// </summary>
        /// <returns>قائمة الاختبارات المستقبلية</returns>
        Task<IEnumerable<Exam>> GetUpcomingExamsAsync();

        /// <summary>
        /// جلب الاختبارات المنتهية
        /// </summary>
        /// <returns>قائمة الاختبارات المنتهية</returns>
        Task<IEnumerable<Exam>> GetCompletedExamsAsync();

        /// <summary>
        /// جلب اختبار واحد مع التفاصيل
        /// </summary>
        /// <param name="examId">معرف الاختبار</param>
        /// <returns>بيانات الاختبار مع التفاصيل</returns>
        Task<Exam?> GetExamWithDetailsAsync(int examId);

        /// <summary>
        /// جلب الاختبارات حسب النوع
        /// </summary>
        /// <param name="examType">نوع الاختبار</param>
        /// <returns>قائمة اختبارات النوع</returns>
        Task<IEnumerable<Exam>> GetExamsByTypeAsync(string examType);

        /// <summary>
        /// جلب الاختبارات حسب المستوى
        /// </summary>
        /// <param name="level">المستوى</param>
        /// <returns>قائمة اختبارات المستوى</returns>
        Task<IEnumerable<Exam>> GetExamsByLevelAsync(string level);

        /// <summary>
        /// البحث في الاختبارات
        /// </summary>
        /// <param name="searchTerm">مصطلح البحث</param>
        /// <returns>نتائج البحث</returns>
        Task<IEnumerable<Exam>> SearchExamsAsync(string searchTerm);
    }
}
