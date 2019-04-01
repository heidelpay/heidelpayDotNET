using Heidelpay.Payment.Interfaces;
using System;

namespace Heidelpay.Payment.Interface
{
    internal static class TypeUrlExtensions
    {
        private const string PLACEHOLDER_CHARGE_ID = "<chargeId>";

        private const string PLACEHOLDER_PAYMENT_ID = "<paymentId>";

        private const string REFUND_URL = "payments/<paymentId>/charges/<chargeId>/cancels";

        public static string ResolveResourceUrl(this IRestResource value)
        {
            return value.TypeUrl
                .Replace(PLACEHOLDER_PAYMENT_ID + "/", string.Empty)
                .EnsureTrailingSlash();
        }

        public static string ResolvePaymentUrl(this IRestResource value, string paymentId)
        {
            return value.TypeUrl
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .EnsureTrailingSlash();
        }
        public static string ResolveRefundUrl(this IRestResource value, string paymentId, string chargeId)
        {
            return REFUND_URL
                .Replace(PLACEHOLDER_PAYMENT_ID, paymentId)
                .Replace(PLACEHOLDER_CHARGE_ID, chargeId)
                .EnsureTrailingSlash();
        }
    }
}
