namespace QuranPreservationSystem.Domain.Enums;

/// <summary>
/// نوع العملية على البيانات
/// </summary>
public enum ActionType
{
    /// <summary>
    /// إنشاء - Create
    /// </summary>
    Create = 1,

    /// <summary>
    /// تعديل - Update
    /// </summary>
    Update = 2,

    /// <summary>
    /// حذف - Delete
    /// </summary>
    Delete = 3,

    /// <summary>
    /// عرض - View
    /// </summary>
    View = 4,

    /// <summary>
    /// تصدير - Export
    /// </summary>
    Export = 5,

    /// <summary>
    /// استيراد - Import
    /// </summary>
    Import = 6
}

