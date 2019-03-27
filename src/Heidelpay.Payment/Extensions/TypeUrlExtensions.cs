using Heidelpay.Payment.Interfaces;
using System;

namespace Heidelpay.Payment.Interface
{
    public static class TypeUrlExtensions
    {
        private const string PLACEHOLDER_CHARGE_ID = "<chargeId>";

        private const string PLACEHOLDER_PAYMENT_ID = "<paymentId>";

        private const string REFUND_URL = "payments/<paymentId>/charges/<chargeId>/cancels";

        public static string TypeResourceUrl(this IRestResource value)
        {
            return value.TypeUrl
                .Replace(PLACEHOLDER_PAYMENT_ID + "/", string.Empty)
                .EnsureTrailingSlash();
        }
    }
}
