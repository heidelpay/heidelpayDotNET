using Heidelpay.Payment.Interfaces;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.PaymentTypes
{
    public static class IPaymentTypeExtensions
    {
        public static async Task<Authorization> AuthorizeAsync(this IAuthorizedPaymentType payment, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            Check.NotNull(payment.Heidelpay, "Heidelpay", "You cannot call authorize methods on an unattached resource. Please either inject or use heidelpay instance directly.");

            return await payment.Heidelpay.AuthorizeAsync(amount, currency, payment, returnUrl, customer);
        }

        public static async Task<Charge> ChargeAsync(this IChargeablePaymentType payment, 
            decimal amount, string currency, Uri returnUrl, Customer customer = null)
        {
            Check.NotNull(payment.Heidelpay, "Heidelpay", "You cannot call charge methods on an unattached resource. Please either inject or use heidelpay instance directly.");

            return await payment.Heidelpay.ChargeAsync(amount, currency, payment, returnUrl, customer);
        }

        public static Charge NewCharge(this IChargeablePaymentType paymentType)
        {
            return new Charge(paymentType);
        }

        public static Authorization NewAuthorization(this IAuthorizedPaymentType paymentType)
        {
            return new Authorization(paymentType);
        }
    }
}
