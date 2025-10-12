using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace QuranPreservationSystem.Helpers
{
    /// <summary>
    /// Extension methods للـ Enums
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// جلب الاسم المعروض من Display attribute
        /// </summary>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? enumValue.ToString();
        }
    }
}

