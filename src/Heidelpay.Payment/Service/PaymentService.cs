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

        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : PaymentTypeBase
        {
            return await ApiPostAsync(paymentType);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Check.NotNull(customer, nameof(customer));

            return await ApiPostAsync(customer);
        }

        public async Task<Customer> UpdateCustomerAsync(string id, Customer customer)
        {
            Check.NotNullOrEmpty(id, nameof(id));
            Check.NotNull(customer, nameof(customer));

            return await ApiPutAsync(id, customer);
        }

        public async Task DeleteCustomerAsync(string id)
        {
            Check.NotNullOrEmpty(id, nameof(id));

            await ApiDeleteAsync<Customer>(id);
        }

        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.NotNull(charge, nameof(charge));

            var result = await ApiPostAsync(charge, getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            Check.NotNull(authorization, nameof(authorization));

            var result = await ApiPostAsync(authorization, getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        public async Task<TPaymentBase> FetchPaymentTypeAsync<TPaymentBase>(string paymentTypeId)
            where TPaymentBase : PaymentTypeBase
        {
            Check.NotNullOrEmpty(paymentTypeId, nameof(paymentTypeId));
            var paymentType = ResolvePaymentTypeFromTypeId(paymentTypeId);

            return await ApiGetAsync(paymentTypeId, paymentType) as TPaymentBase;
        }

        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            var payment = await FetchPaymentAsync(paymentId);

            return payment?.Authorization;
        }

        public async Task<Charge> FetchChargeAsync(string paymentId, string chargeId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment.ChargesList.FirstOrDefault(x => x.Id.Equals(chargeId, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Cancel> FetchCancelAsync(string paymentId, string cancelId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(cancelId, nameof(cancelId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment.CancelList.FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Cancel> FetchCancelAsync(string paymentId, string chargeId, string cancelId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(cancelId, nameof(cancelId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment?
                .ChargesList?
                .FirstOrDefault(x => x.Id.Equals(chargeId, StringComparison.InvariantCultureIgnoreCase))?
                .CancelList
                .FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<Payment> FetchPaymentAsync(string paymentId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            var payment = new Payment { Id = paymentId };

            var result = await ApiGetAsync(payment);

            result.CancelList = await FetchCancelListAsync(payment);

            return result;
        }

        public async Task<string> EnsureRestResourceIdAsync<T>(T resource)
            where T : class, IRestResource
        {
            var response = (await heidelpay.RestClient.HttpPostAsync(BuildApiEndpointUri(resource), resource, typeof(IdResponse))) as IdResponse;
            return response?.Id;
        }

        public async Task<Shipment> ShipmentAsync(string paymentId, string invoiceId = null)
        {
            var shipment = new Shipment { InvoiceId = invoiceId };

            var paymentUri = BuildApiEndpointUri(shipment, shipment.ResolvePaymentUrl(paymentId), null);
            var result = await ApiPostAsync(shipment, paymentUri, false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        private async Task<IEnumerable<Cancel>> FetchCancelListAsync(Payment payment)
        {
            return Enumerable.Empty<Cancel>();
        }

        private IEnumerable<Cancel> GetCancelsForAuthorization(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_AUTHORIZE.Equals(x.TransactionType, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private IEnumerable<Cancel> GetCancelsForCharge(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_CHARGE.Equals(x.TransactionType, StringComparison.InvariantCultureIgnoreCase))
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

        private async Task<object> ApiGetAsync(string id, IRestResource resource)
        {
            var result = await heidelpay.RestClient.HttpGetAsync(BuildApiEndpointUri(resource, id), resource.GetType());
            return PostProcessApiResource(result);
        }

        private async Task<T> ApiGetAsync<T>(T resource)
             where T : class, IRestResource
        {
            var result = await heidelpay.RestClient.HttpGetAsync<T>(BuildApiEndpointUri(resource, resource.Id));
            return PostProcessApiResource(result);
        }

        private async Task<T> ApiPostAsync<T>(T resource, Uri uri = null, bool getAfterPost = true)
           where T : class, IRestResource
        {
            var posted = await heidelpay.RestClient.HttpPostAsync<T>(uri ?? BuildApiEndpointUri(resource), resource);
            return getAfterPost 
                ? await ApiGetAsync(posted) 
                : PostProcessApiResource(posted);
        }

        private async Task<T> ApiPutAsync<T>(string id, T resource, bool getAfterPut = false)
           where T : class, IRestResource
        {
            var putted = await heidelpay.RestClient.HttpPutAsync<T>(BuildApiEndpointUri(resource, id), resource);
            return getAfterPut
                ? await ApiGetAsync(putted)
                : PostProcessApiResource(putted);
        }

        private async Task ApiDeleteAsync<T>(string id)
           where T : class, IRestResource
        {
            await heidelpay.RestClient.HttpDeleteAsync<T>(BuildApiEndpointUri(default(T), id));
        }

        private Uri BuildApiEndpointUri(IRestResource resource, string id = null)
        {
            return BuildApiEndpointUri(resource, resource.ResolveResourceUrl(), id);
        }

        private Uri BuildApiEndpointUri(IRestResource resource, string resourceUrl, string id)
        {
            var rootPath = new Uri(heidelpay.RestClient?.Options?.ApiEndpoint, heidelpay.RestClient?.Options?.ApiVersion.EnsureTrailingSlash());
            var combinedPaths = new Uri(rootPath, resourceUrl);

            if(!string.IsNullOrEmpty(id))
            {
                combinedPaths = new Uri(combinedPaths, id.EnsureTrailingSlash());
            }

            return combinedPaths;
        }

        private PaymentTypeBase ResolvePaymentTypeFromTypeId(string typeId)
        {
            Check.NotNullOrEmpty(typeId, nameof(typeId));
            Check.ThrowIfTrue(typeId.Length < 5, "TypeId '" + typeId + "' is invalid");

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
                case "ivf": result = new InvoiceFactoring(); break;
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
    internal class IdResponse : IRestResource
    {
        public string TypeUrl => null;

        public string Id { get; set; }
    }
}
