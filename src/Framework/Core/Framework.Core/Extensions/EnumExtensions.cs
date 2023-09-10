using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Extensions;

public static class EnumExtensions
{
    public static T GetValueFromName<T>(this string name) where T : Enum
    {
        var type = typeof(T);

        foreach (var field in type.GetFields())
        {
            if (field.Name == name)
            {
                return (T) field.GetValue(null)!;
            }

            if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
            {
                if (attribute.Name == name)
                {
                    return (T) field.GetValue(null)!;
                }
            }
        }

        throw new ArgumentOutOfRangeException(nameof(name));
    }
}