using Heidelpay.Payment.Interface;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Service
{
    internal sealed class PaymentService
    {
        private static readonly string TRANSACTION_TYPE_AUTHORIZATION = "authorize";
        private static readonly string TRANSACTION_TYPE_CHARGE = "charge";
        private static readonly string TRANSACTION_TYPE_CANCEL_AUTHORIZE = "cancel-authorize";
        private static readonly string TRANSACTION_TYPE_CANCEL_CHARGE = "cancel-charge";

        private readonly Heidelpay heidelpay;

        public PaymentService(Heidelpay heidelpay)
        {
            this.heidelpay = heidelpay;
        }

        internal async Task<TPaymentBase> CreatePaymentTypeBaseAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : PaymentTypeBase
        {
            return PostProcessResult(await EnsurePaymentTypeAsync(paymentType));
        }

        internal async Task<TPaymentBase> EnsurePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : IPaymentType
        {
            return await heidelpay.RestClient.HttpPostAsync<TPaymentBase>(new Uri(heidelpay.ApiEndpointUri, paymentType.TypeResourceUrl()), paymentType);
        }

        public async Task<Charge> ChargeAsync(Charge charge)
        {
            return await ChargeAsync(charge, new Uri(heidelpay.ApiEndpointUri, charge.TypeResourceUrl()));
        }

        public async Task<Charge> ChargeAsync(Charge charge, Uri url)
        {
            return PostProcessResult(await heidelpay.RestClient.HttpPostAsync<Charge>(url, charge));
        }

        private TPaymentBase PostProcessResult<TPaymentBase>(TPaymentBase payment)
            where TPaymentBase : IHeidelpayProvider
        {
            payment.Heidelpay = heidelpay;
            return payment;
        }
    }
}
