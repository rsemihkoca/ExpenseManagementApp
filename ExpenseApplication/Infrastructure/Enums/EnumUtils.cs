using System.ComponentModel;
using Infrastructure.Exceptions;

namespace Business.Enums;

public static class EnumUtils
{
    public static T? IntToEnum<T>(int value)
    {
        try
        {
            return (T) Enum.ToObject(typeof(T), value);
        }
        catch (ArgumentException)
        {
            return default(T);
        }
    }

    public static string EnumToString<T>(T enumValue)
    {
        return Enum.GetName(typeof(T), enumValue) ?? throw new HttpException("Enum value not found", 500);
    }
    
    public static string? GetDescription<T>(T enumValue)
    {
        var fi = enumValue.GetType().GetField(enumValue.ToString() ?? throw new InvalidOperationException());
        var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
    }
}
