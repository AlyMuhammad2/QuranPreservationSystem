namespace QuranPreservationSystem.Application.DTOs;

/// <summary>
/// معلومات السورة
/// </summary>
public class SurahDto
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EnglishName { get; set; } = string.Empty;
    public string EnglishNameTranslation { get; set; } = string.Empty;
    public string RevelationType { get; set; } = string.Empty; // Meccan or Medinan
    public int NumberOfAyahs { get; set; }
}

/// <summary>
/// معلومات الآية
/// </summary>
public class AyahDto
{
    public int Number { get; set; }
    public string Text { get; set; } = string.Empty;
    public int NumberInSurah { get; set; }
    public int Juz { get; set; }
    public int Manzil { get; set; }
    public int Page { get; set; }
    public int Ruku { get; set; }
    public int HizbQuarter { get; set; }
    // Sajda: يمكن أن يكون false أو object - نتجاهله للبساطة
    // public object? Sajda { get; set; }
}

/// <summary>
/// السورة الكاملة مع الآيات
/// </summary>
public class SurahDetailDto
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string EnglishName { get; set; } = string.Empty;
    public string EnglishNameTranslation { get; set; } = string.Empty;
    public string RevelationType { get; set; } = string.Empty;
    public int NumberOfAyahs { get; set; }
    public List<AyahDto> Ayahs { get; set; } = new();
}

/// <summary>
/// الاستجابة من الـ API
/// </summary>
public class QuranApiResponse<T>
{
    public int Code { get; set; }
    public string Status { get; set; } = string.Empty;
    public T? Data { get; set; }
}

