using Heidelpay.Payment.Interfaces;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    public static class IPaymentTypeExtensions
    {
        public static async Task<Authorization> AuthorizeAsync(this IPaymentAuthorize payment, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            return await payment.Heidelpay.AuthorizeAsync(amount, currency, payment, returnUrl, customer);
        }

        public static async Task<Charge> ChargeAsync(this IPaymentCharge payment, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            return await payment.Heidelpay.ChargeAsync(amount, currency, payment, returnUrl, customer);
        }
    }
}
