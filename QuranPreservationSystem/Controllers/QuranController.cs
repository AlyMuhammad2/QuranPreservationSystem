using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuranPreservationSystem.Application.Interfaces;

namespace QuranPreservationSystem.Controllers;

/// <summary>
/// القرآن الكريم
/// </summary>
[Authorize]
public class QuranController : Controller
{
    private readonly IQuranService _quranService;
    private readonly ILogger<QuranController> _logger;

    public QuranController(
        IQuranService quranService,
        ILogger<QuranController> logger)
    {
        _quranService = quranService;
        _logger = logger;
    }

    /// <summary>
    /// فهرس القرآن الكريم - قائمة السور
    /// </summary>
    public async Task<IActionResult> Index(string? filter)
    {
        try
        {
            var surahs = await _quranService.GetAllSurahsAsync();

            // فلترة حسب النوع (مكية/مدنية)
            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.ToLower() == "meccan")
                {
                    surahs = surahs.Where(s => s.RevelationType.ToLower() == "meccan").ToList();
                }
                else if (filter.ToLower() == "medinan")
                {
                    surahs = surahs.Where(s => s.RevelationType.ToLower() == "medinan").ToList();
                }
            }

            ViewBag.CurrentFilter = filter;
            ViewBag.TotalSurahs = surahs.Count;
            ViewBag.MeccanCount = surahs.Count(s => s.RevelationType.ToLower() == "meccan");
            ViewBag.MedinanCount = surahs.Count(s => s.RevelationType.ToLower() == "medinan");

            return View(surahs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في تحميل فهرس القرآن الكريم");
            TempData["Error"] = "حدث خطأ في تحميل فهرس القرآن الكريم";
            return View(new List<Application.DTOs.SurahDto>());
        }
    }

    /// <summary>
    /// عرض سورة معينة
    /// </summary>
    public async Task<IActionResult> Surah(int id)
    {
        if (id < 1 || id > 114)
        {
            TempData["Error"] = "رقم السورة غير صحيح";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var surah = await _quranService.GetSurahByNumberAsync(id);

            if (surah == null)
            {
                TempData["Error"] = "لم يتم العثور على السورة";
                return RedirectToAction(nameof(Index));
            }

            // معلومات للتنقل
            ViewBag.PreviousSurah = id > 1 ? id - 1 : (int?)null;
            ViewBag.NextSurah = id < 114 ? id + 1 : (int?)null;
            ViewBag.HasBasmala = id != 1 && id != 9; // كل السور فيها بسملة ما عدا الفاتحة (البسملة آية) والتوبة

            return View(surah);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في تحميل السورة رقم {SurahNumber}", id);
            TempData["Error"] = "حدث خطأ في تحميل السورة";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// البحث في القرآن الكريم
    /// </summary>
    public async Task<IActionResult> Search(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var results = await _quranService.SearchAsync(keyword);
            ViewBag.Keyword = keyword;
            ViewBag.ResultsCount = results.Count;

            return View(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطأ في البحث عن الكلمة: {Keyword}", keyword);
            TempData["Error"] = "حدث خطأ في البحث";
            return RedirectToAction(nameof(Index));
        }
    }
}

