using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Heidelpay.Payment.Extensions
{
    public static class CoreExtensions
    {
        static readonly string[] AllowedDateTimeFormats = new[] 
        {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd",
        };

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
    }
}
