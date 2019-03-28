using System.Globalization;
using System.Text;

namespace System
{
    internal static class CoreExtensions
    {
        public static string EncodeToBase64(this string plainText)
        {
            Check.NotNull(plainText, nameof(plainText));

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string DecodeFromBase64(this string base64EncodedData)
        {
            Check.NotNull(base64EncodedData, nameof(base64EncodedData));

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static readonly string[] AllowedDateTimeFormats = new[] 
        {
            DateTimeFormat,
            DateOnlyFormat,
        };

        public const string DateOnlyFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static bool TryParseDateTime(this string value, out DateTime result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default; 
                return false;
            }

            return DateTime.TryParseExact(value, AllowedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string EnsureTrailingSlash(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.EndsWith("/"))
                return value;

            return value + "/";
        }
    }
}
