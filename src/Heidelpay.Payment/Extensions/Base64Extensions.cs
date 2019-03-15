using Heidelpay.Payment.Extensions;
using System.Text;

namespace System
{
    public static class Base64Extensions
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
    }
}
