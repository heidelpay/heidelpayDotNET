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

        public async Task<Charge> ChargeAsync(Charge charge)
        {
            return await ChargeAsync(charge, new Uri(heidelpay.RestClient.Options.ApiEndpoint, charge.TypeUrl));
        }

        public async Task<Charge> ChargeAsync(Charge charge, Uri url)
        {
            return await heidelpay.RestClient.HttpPostAsync<Charge>(url, charge);
        }
    }
}
