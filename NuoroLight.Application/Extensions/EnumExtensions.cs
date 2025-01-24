using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NuoroLight.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue == null)
                throw new ArgumentNullException(nameof(enumValue), "Enum value cannot be null.");

            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    return displayAttribute.GetName();
                }
            }

            // Fallback to the enum value's name if no Display attribute is found
            return enumValue.ToString();
        }
    }
}
