using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using QuranPreservationSystem.Application.DTOs;
using QuranPreservationSystem.Application.Interfaces;
using System.Text.Json;

namespace QuranPreservationSystem.Infrastructure.Services;

/// <summary>
/// خدمة القرآن الكريم - التعامل مع Al-Quran Cloud API
/// </summary>
public class QuranService : IQuranService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<QuranService> _logger;
    private const string BaseUrl = "https://api.alquran.cloud/v1";
    private const string CacheKeyAllSurahs = "AllSurahs";
    private const string CacheKeySurahPrefix = "Surah_";

    public QuranService(
        HttpClient httpClient,
        IMemoryCache cache,
        ILogger<QuranService> logger)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// جلب قائمة جميع السور
    /// </summary>
    public async Task<List<SurahDto>> GetAllSurahsAsync()
    {
        try
        {
            // التحقق من الـ Cache أولاً
            if (_cache.TryGetValue(CacheKeyAllSurahs, out List<SurahDto>? cachedSurahs) && cachedSurahs != null)
            {
                return cachedSurahs;
            }

            // جلب البيانات من الـ API
            var response = await _httpClient.GetStringAsync("/surah");
            var apiResponse = JsonSerializer.Deserialize<QuranApiResponse<List<SurahDto>>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse?.Data != null)
            {
                var surahs = apiResponse.Data;
                
                // حفظ في الـ Cache لمدة 24 ساعة
                _cache.Set(CacheKeyAllSurahs, surahs, TimeSpan.FromHours(24));
                
                return surahs;
            }

            _logger.LogWarning("فشل في جلب قائمة السور من الـ API");
            return new List<SurahDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في جلب قائمة السور");
            return new List<SurahDto>();
        }
    }

    /// <summary>
    /// جلب سورة معينة مع آياتها
    /// </summary>
    public async Task<SurahDetailDto?> GetSurahByNumberAsync(int surahNumber)
    {
        if (surahNumber < 1 || surahNumber > 114)
        {
            return null;
        }

        try
        {
            var cacheKey = $"{CacheKeySurahPrefix}{surahNumber}";

            // التحقق من الـ Cache
            if (_cache.TryGetValue(cacheKey, out SurahDetailDto? cachedSurah) && cachedSurah != null)
            {
                return cachedSurah;
            }

            // جلب البيانات من الـ API مع edition للنص العثماني
            var response = await _httpClient.GetStringAsync($"/surah/{surahNumber}/ar.alafasy");
            var apiResponse = JsonSerializer.Deserialize<QuranApiResponse<SurahDetailDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse?.Data != null)
            {
                var surah = apiResponse.Data;
                
                // حفظ في الـ Cache لمدة 7 أيام
                _cache.Set(cacheKey, surah, TimeSpan.FromDays(7));
                
                return surah;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في جلب السورة رقم {SurahNumber}", surahNumber);
            return null;
        }
    }

    /// <summary>
    /// الحصول على معلومات سورة معينة فقط (بدون الآيات)
    /// </summary>
    public async Task<SurahDto?> GetSurahInfoAsync(int surahNumber)
    {
        var allSurahs = await GetAllSurahsAsync();
        return allSurahs.FirstOrDefault(s => s.Number == surahNumber);
    }

    /// <summary>
    /// البحث في القرآن الكريم
    /// </summary>
    public async Task<List<AyahDto>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return new List<AyahDto>();
        }

        try
        {
            // البحث عبر الـ API
            var response = await _httpClient.GetStringAsync($"/search/{keyword}/all/ar");
            var apiResponse = JsonSerializer.Deserialize<QuranApiResponse<SearchResultDto>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (apiResponse?.Data?.Matches != null)
            {
                return apiResponse.Data.Matches;
            }

            return new List<AyahDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في البحث عن الكلمة: {Keyword}", keyword);
            return new List<AyahDto>();
        }
    }
}

/// <summary>
/// نتائج البحث من الـ API
/// </summary>
public class SearchResultDto
{
    public int Count { get; set; }
    public List<AyahDto> Matches { get; set; } = new();
}

