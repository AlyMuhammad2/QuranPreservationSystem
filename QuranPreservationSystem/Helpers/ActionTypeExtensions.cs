using QuranPreservationSystem.Domain.Enums;

namespace QuranPreservationSystem.Helpers;

public static class ActionTypeExtensions
{
    public static string GetDisplayName(this ActionType actionType)
    {
        return actionType switch
        {
            ActionType.Create => "إنشاء",
            ActionType.Update => "تعديل",
            ActionType.Delete => "حذف",
            ActionType.View => "عرض",
            ActionType.Export => "تصدير",
            ActionType.Import => "استيراد",
            _ => actionType.ToString()
        };
    }

    public static string GetIconClass(this ActionType actionType)
    {
        return actionType switch
        {
            ActionType.Create => "fas fa-plus-circle text-success",
            ActionType.Update => "fas fa-edit text-warning",
            ActionType.Delete => "fas fa-trash text-danger",
            ActionType.View => "fas fa-eye text-info",
            ActionType.Export => "fas fa-file-export text-primary",
            ActionType.Import => "fas fa-file-import text-primary",
            _ => "fas fa-info-circle"
        };
    }
}

