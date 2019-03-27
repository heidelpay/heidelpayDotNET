using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interface;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
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

        public async Task<TPaymentBase> FetchPaymentTypeAsync<TPaymentBase>(string typeId)
            where TPaymentBase : PaymentTypeBase
        {
            var paymentTypeBase = ResolvePaymentTypeFromTypeId(typeId);

            return await ApiGetAsync(paymentTypeBase, typeId) as TPaymentBase;
        }

        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            var payment = await FetchPaymentAsync(paymentId);

            return payment?.Authorization;
        }

        public async Task<Charge> FetchChargeAsync(string paymentId, string chargeId)
        {
            var payment = await FetchPaymentAsync(paymentId);

            return payment.ChargesList.FirstOrDefault(x => x.Id.Equals(chargeId, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Cancel> FetchCancelAsync(string paymentId, string cancelId)
        {
            var payment = await FetchPaymentAsync(paymentId);

            return payment.CancelList.FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Cancel> FetchCancelAsync(string paymentId, string chargeId, string cancelId)
        {
            var payment = await FetchPaymentAsync(paymentId);

            return payment?
                .ChargesList?
                .FirstOrDefault(x => x.Id.Equals(chargeId, StringComparison.InvariantCultureIgnoreCase))?
                .CancelList
                .FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Payment> FetchPaymentAsync(string paymentId)
        {
            var payment = new Payment { Id = paymentId };

            var result = await ApiGetAsync(payment);

            result.CancelList = await FetchCancelListAsync(payment);

            return result;
        }

        public async Task<TPaymentBase> CreatePaymentTypeBaseAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : PaymentTypeBase
        {
            var result = await ApiPostAsync(paymentType);
            return await FetchPaymentTypeAsync<TPaymentBase>(result.Id);
        }

        public async Task<TPaymentBase> EnsurePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : IPaymentType
        {
            return await ApiPostAsync(paymentType);
        }

        private async Task<IEnumerable<Cancel>> FetchCancelListAsync(Payment payment)
        {
            return Enumerable.Empty<Cancel>();
        }

        private IEnumerable<Cancel> GetCancelsForAuthorization(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_AUTHORIZE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private IEnumerable<Cancel> GetCancelsForCharge(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_CHARGE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private IEnumerable<Transaction> GetCancels(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_CHARGE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase) 
                || TRANSACTION_TYPE_CANCEL_AUTHORIZE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private IEnumerable<Transaction> GetCharges(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CHARGE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private IEnumerable<Transaction> GetAuthorization(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_AUTHORIZATION.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<object> ApiGetAsync(IRestResource resource, string id = null)
        {
            var result = await heidelpay.RestClient.HttpGetAsync(BuildApiEndpointUri(resource, id ?? resource.Id), resource.GetType());

            return PostProcessApiResource(result);
        }

        private async Task<T> ApiGetAsync<T>(T resource)
             where T : IRestResource
        {
            var result = await heidelpay.RestClient.HttpGetAsync<T>(BuildApiEndpointUri(resource, resource.Id));

            return PostProcessApiResource(result);
        }

        private async Task<T> ApiPostAsync<T>(T resource, Uri uri = null)
           where T : IRestResource
        {
            var result = await heidelpay.RestClient.HttpPostAsync<T>(uri ?? BuildApiEndpointUri(resource), resource);

            return PostProcessApiResource(result);
        }

        private Uri BuildApiEndpointUri(IRestResource resource, params string[] paths)
        {
            var rootPath = new Uri(heidelpay.RestClient?.Options?.ApiEndpoint, heidelpay.RestClient?.Options?.ApiVersion.EnsureTrailingSlash());
            var combinedPaths = new Uri(rootPath, resource.TypeResourceUrl());

            foreach (string extendedPath in paths)
            {
                combinedPaths = new Uri(combinedPaths, extendedPath.EnsureTrailingSlash());
            }

            return combinedPaths;
        }

        private PaymentTypeBase ResolvePaymentTypeFromTypeId(string typeId)
        {
            if (typeId.Length < 5)
                throw new PaymentException("TypeId '" + typeId + "' is invalid");

            var shortTypeId = typeId
                .Substring(2, 3)
                .ToLower();

            PaymentTypeBase result = null;

            switch (shortTypeId)
            {
                case "crd": result = new Card { Number = "", ExpiryDate = "" }; break;
                case "eps": result = new Eps(); break;
                case "gro": result = new Giropay(); break;
                case "idl": result = new Ideal(); break;
                case "ivc": result = new Invoice(); break;
                case "ivg": result = new InvoiceGuaranteed(); break;
                case "ppl": result = new Paypal(); break;
                case "ppy": result = new Prepayment(); break;
                case "p24": result = new Przelewy24(); break;
                case "sdd": result = new SepaDirectDebit(); break;
                case "ddg": result = new SepaDirectDebitGuaranteed(); break;
                case "sft": result = new Sofort(); break;
                case "pis": result = new Pis(); break;
                default: throw new PaymentException("Type '" + shortTypeId + "' is currently not supported by the SDK");
            }

            return PostProcessApiResource(result);
        }
        private T PostProcessApiResource<T>(T resource)
        {
            if (resource is IHeidelpayProvider provider)
            {
                provider.Heidelpay = heidelpay;
            }

            return resource;
        }
    }
}
