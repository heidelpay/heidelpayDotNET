using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interface;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using System;
using System.Linq;
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
            Check.NotNull(heidelpay, nameof(heidelpay));

            this.heidelpay = heidelpay;
        }

        public async Task<Charge> ChargeAsync(Charge charge, Uri url = null)
        {
            Check.NotNull(charge, nameof(charge));

            return await ApiPostAsync(charge, url);
        }

        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            Check.NotNull(authorization, nameof(authorization));

            return await ApiPostAsync(authorization);
        }

        internal async Task<TPaymentBase> CreatePaymentTypeBaseAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : PaymentTypeBase
        {
            return await ApiPostAsync(paymentType);
        }

        internal async Task<TPaymentBase> EnsurePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : IPaymentType
        {
            return await ApiPostAsync(paymentType);
        }

        private async Task<T> ApiPostAsync<T>(T resource, Uri uri = null)
           where T : IRestResource
        {
            var result = await heidelpay.RestClient.HttpPostAsync<T>(uri ?? BuildApiPostEndpointUri(resource), resource);

            if(result is IHeidelpayProvider provider)
            {
                provider.Heidelpay = heidelpay;
            }

            return result;
        }

        private Uri BuildApiPostEndpointUri(IRestResource resource)
        {
            return BuildApiPostEndpointUri(resource.TypeResourceUrl());
        }

        private Uri BuildApiPostEndpointUri(params string[] paths)
        {
            var combinedPaths = new Uri(heidelpay.RestClient?.Options?.ApiEndpoint, heidelpay.RestClient?.Options?.ApiVersion + "/");

            foreach (string extendedPath in paths)
            {
                combinedPaths = new Uri(combinedPaths, extendedPath);
            }

            return combinedPaths;
        }
    }
}
