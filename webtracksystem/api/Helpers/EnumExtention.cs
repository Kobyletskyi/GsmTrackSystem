using System;

namespace Api.Extentions
{
    public static class EnumExtention{
        public static string GetName<T>(this T value) where T: struct, IConvertible
        {
            return Enum.GetName(typeof(T), value);
        }
    }
}