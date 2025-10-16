using QuranPreservationSystem.Application.DTOs;

namespace QuranPreservationSystem.Application.Interfaces;

/// <summary>
/// خدمة القرآن الكريم
/// </summary>
public interface IQuranService
{
    /// <summary>
    /// جلب قائمة جميع السور
    /// </summary>
    Task<List<SurahDto>> GetAllSurahsAsync();

    /// <summary>
    /// جلب سورة معينة مع آياتها
    /// </summary>
    Task<SurahDetailDto?> GetSurahByNumberAsync(int surahNumber);

    /// <summary>
    /// البحث في القرآن الكريم
    /// </summary>
    Task<List<AyahDto>> SearchAsync(string keyword);

    /// <summary>
    /// الحصول على معلومات سورة معينة
    /// </summary>
    Task<SurahDto?> GetSurahInfoAsync(int surahNumber);
}

