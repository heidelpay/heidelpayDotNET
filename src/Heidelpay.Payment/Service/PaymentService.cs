// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="PaymentService.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Service
{
    /// <summary>
    /// Class PaymentService. This class cannot be inherited.
    /// </summary>
    internal sealed class PaymentService
    {
        /// <summary>
        /// The transaction type authorization
        /// </summary>
        private static readonly string TRANSACTION_TYPE_AUTHORIZATION = "authorize";
        /// <summary>
        /// The transaction type charge
        /// </summary>
        private static readonly string TRANSACTION_TYPE_CHARGE = "charge";
        /// <summary>
        /// The transaction type cancel authorize
        /// </summary>
        private static readonly string TRANSACTION_TYPE_CANCEL_AUTHORIZE = "cancel-authorize";
        /// <summary>
        /// The transaction type cancel charge
        /// </summary>
        private static readonly string TRANSACTION_TYPE_CANCEL_CHARGE = "cancel-charge";

        /// <summary>
        /// The heidelpay
        /// </summary>
        private readonly HeidelpayClient heidelpay;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentService"/> class.
        /// </summary>
        /// <param name="heidelpay">The heidelpay client instance.</param>
        public PaymentService(HeidelpayClient heidelpay)
        {
            Check.ThrowIfNull(heidelpay, nameof(heidelpay));

            this.heidelpay = heidelpay;
        }

        /// <summary>
        /// create payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the t payment base.</typeparam>
        /// <param name="paymentType">Type of the payment.</param>
        /// <returns>Task&lt;TPaymentBase&gt;.</returns>
        public async Task<TPaymentBase> CreatePaymentTypeAsync<TPaymentBase>(TPaymentBase paymentType)
            where TPaymentBase : class, IPaymentType
        {
            return await ApiPostAsync(paymentType);
        }

        /// <summary>
        /// create basket as an asynchronous operation.
        /// </summary>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> CreateBasketAsync(Basket basket)
        {
            Check.ThrowIfNull(basket, nameof(basket));

            return await ApiPostAsync(basket);
        }

        /// <summary>
        /// create meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> CreateMetaDataAsync(MetaData metadata)
        {
            Check.ThrowIfNull(metadata, nameof(metadata));

            var created = await heidelpay.RestClient.HttpPostAsync<MetaData>(BuildApiEndpointUri<MetaData>(), metadata.MetadataMap);
            created.MetadataMap = await heidelpay.RestClient.HttpGetAsync<Dictionary<string,string>>(BuildApiEndpointUri<MetaData>(created.Id));
            return created;
        }

        /// <summary>
        /// update basket as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="basket">The basket.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> UpdateBasketAsync(string id, Basket basket)
        {
            Check.ThrowIfNullOrEmpty(id, nameof(id));
            Check.ThrowIfNull(basket, nameof(basket));

            return await ApiPutAsync(id, basket, getAfterPut: true);
        }

        /// <summary>
        /// create customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Check.ThrowIfNull(customer, nameof(customer));

            return await ApiPostAsync(customer, getAfterPost: true);
        }

        /// <summary>
        /// update customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> UpdateCustomerAsync(string id, Customer customer)
        {
            Check.ThrowIfNullOrEmpty(id, nameof(id));
            Check.ThrowIfNull(customer, nameof(customer));

            return await ApiPutAsync(id, customer, getAfterPut: true);
        }

        /// <summary>
        /// delete customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public async Task DeleteCustomerAsync(string id)
        {
            Check.ThrowIfNullOrEmpty(id, nameof(id));

            await ApiDeleteAsync<Customer>(id);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.ThrowIfNull(charge, nameof(charge));

            var result = await ApiPostAsync(charge, getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// authorize as an asynchronous operation.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> AuthorizeAsync(Authorization authorization)
        {
            Check.ThrowIfNull(authorization, nameof(authorization));

            var result = await ApiPostAsync(authorization, getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// fetch payment type as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TPaymentBase">The type of the t payment base.</typeparam>
        /// <param name="paymentTypeId">The payment type identifier.</param>
        /// <returns>Task&lt;TPaymentBase&gt;.</returns>
        public async Task<TPaymentBase> FetchPaymentTypeAsync<TPaymentBase>(string paymentTypeId)
            where TPaymentBase : class, IPaymentType
        {
            Check.ThrowIfNullOrEmpty(paymentTypeId, nameof(paymentTypeId));
            var paymentType = ResolvePaymentTypeFromTypeId(paymentTypeId);

            return await ApiGetAsync(paymentType, paymentTypeId) as TPaymentBase;
        }

        /// <summary>
        /// fetch authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            var payment = await FetchPaymentAsync(paymentId);

            return payment?.Authorization;
        }

        /// <summary>
        /// fetch charge as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> FetchChargeAsync(string paymentId, string chargeId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment.ChargesList.FirstOrDefault(x => x.Id.Equals(chargeId, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// fetch cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> FetchCancelAsync(string paymentId, string cancelId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(cancelId, nameof(cancelId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment.CancelList.FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// fetch cancel as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="cancelId">The cancel identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> FetchCancelAsync(string paymentId, string chargeId, string cancelId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));
            Check.ThrowIfNullOrEmpty(cancelId, nameof(cancelId));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));

            var payment = await FetchPaymentAsync(paymentId);

            return payment?
                .ChargesList?
                .FirstOrDefault(x => x.Id.Equals(chargeId, StringComparison.InvariantCultureIgnoreCase))?
                .CancelList
                .FirstOrDefault(x => x.Id.Equals(cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// fetch payment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        public async Task<Payment> FetchPaymentAsync(string paymentId)
        {
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var payment = await ApiGetAsync<Payment>(paymentId);

            return await PostProcessPayment(payment);
        }

        /// <summary>
        /// fetch basket as an asynchronous operation.
        /// </summary>
        /// <param name="basketId">The basket identifier.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            Check.ThrowIfNullOrEmpty(basketId, nameof(basketId));

            return await ApiGetAsync<Basket>(basketId);
        }

        /// <summary>
        /// fetch customer as an asynchronous operation.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            Check.ThrowIfNullOrEmpty(customerId, nameof(customerId));

            return await ApiGetAsync<Customer>(customerId);
        }

        /// <summary>
        /// fetch meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metaDataId">The meta data identifier.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            Check.ThrowIfNullOrEmpty(metaDataId, nameof(metaDataId));

            var fetched = await heidelpay.RestClient.HttpGetAsync<Dictionary<string, string>>(BuildApiEndpointUri<MetaData>(metaDataId));

            return new MetaData { Id = metaDataId, MetadataMap = fetched };
        }

        /// <summary>
        /// ensure rest resource identifier as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public async Task<string> EnsureRestResourceIdAsync<T>(T resource)
            where T : class, IRestResource
        {
            var response = (await heidelpay.RestClient.HttpPostAsync(BuildApiEndpointUri<T>(), resource, typeof(IdResponse))) as IdResponse;
            return response?.Id;
        }

        /// <summary>
        /// shipment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <param name="invoiceId">The invoice identifier.</param>
        /// <returns>Task&lt;Shipment&gt;.</returns>
        public async Task<Shipment> ShipmentAsync(string paymentId, string invoiceId = null)
        {
            var shipment = new Shipment { InvoiceId = invoiceId };

            var paymentUri = BuildApiEndpointUri(HeidelpayRegistry.ResolvePaymentUrl<Shipment>(paymentId), null);
            var result = await ApiPostAsync(shipment, paymentUri, false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        internal async Task<Charge> ChargeAsync(Charge charge, string paymentId)
        {
            Check.ThrowIfNull(charge, nameof(charge));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(charge, BuildApiEndpointUri(HeidelpayRegistry.ResolvePaymentUrl<Charge>(paymentId), null), getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// cancel as an asynchronous operation.
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        internal async Task<Cancel> CancelAsync(Cancel cancel, string paymentId)
        {
            Check.ThrowIfNull(cancel, nameof(cancel));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(cancel, BuildApiEndpointUri(HeidelpayRegistry.ResolvePaymentUrl<Cancel>(paymentId), null), getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.PaymentId);

            return result;
        }

        /// <summary>
        /// cancel charge as an asynchronous operation.
        /// </summary>
        /// <param name="cancel">The cancel.</param>
        /// <param name="chargeId">The charge identifier.</param>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Cancel&gt;.</returns>
        public async Task<Cancel> CancelChargeAsync(Cancel cancel, string chargeId, string paymentId)
        { 
            Check.ThrowIfNull(cancel, nameof(cancel));
            Check.ThrowIfNullOrEmpty(chargeId, nameof(chargeId));
            Check.ThrowIfNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(cancel, BuildApiEndpointUri(HeidelpayRegistry.ResolveRefundUrl(paymentId, chargeId), null), getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Resolves the payment type from type identifier.
        /// </summary>
        /// <param name="typeId">The type identifier.</param>
        /// <returns>PaymentTypeBase.</returns>
        /// <exception cref="PaymentException">Type '" + shortTypeId + "' is currently not supported by the SDK</exception>
        internal Type ResolvePaymentTypeFromTypeId(string typeId)
        {
            Check.ThrowIfNullOrEmpty(typeId, nameof(typeId));
            Check.ThrowIfTrue(typeId.Length < 5, "TypeId '" + typeId + "' is invalid");

            var shortTypeId = typeId
                .Substring(2, 3)
                .ToLower();

            Type result = null;

            switch (shortTypeId)
            {
                case "crd": result = typeof(Card); break;
                case "eps": result = typeof(Eps); break;
                case "gro": result = typeof(Giropay); break;
                case "idl": result = typeof(Ideal); break;
                case "ivc": result = typeof(Invoice); break;
                case "ivf": result = typeof(InvoiceFactoring); break;
                case "ivg": result = typeof(InvoiceGuaranteed); break;
                case "ppl": result = typeof(Paypal); break;
                case "ppy": result = typeof(Prepayment); break;
                case "p24": result = typeof(Przelewy24); break;
                case "sdd": result = typeof(SepaDirectDebit); break;
                case "ddg": result = typeof(SepaDirectDebitGuaranteed); break;
                case "sft": result = typeof(Sofort); break;
                case "pis": result = typeof(Pis); break;
                default: throw new PaymentException("Type '" + shortTypeId + "' is currently not supported by the SDK");
            }

            return result;
        }

        /// <summary>
        /// fetch authorization as an asynchronous operation.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        private async Task<Authorization> FetchAuthorizationAsync(Payment payment)
        {
            var authTransactions = GetAuthorization(payment.Transactions);
            var result = new List<Authorization>();
            foreach (var authTransaction in authTransactions)
            {
                var auth = await ApiGetAsync<Authorization>(authTransaction.Url);
                auth.Payment = payment;
                auth.ResourceUrl = authTransaction.Url;
                auth.TransactionType = authTransaction.Type;
                auth.CancelList = GetCancelsForAuthorization(payment.CancelList);
                auth.Resources.BasketId = payment.Resources.BasketId;
                result.Add(auth);
            }
            return result.SingleOrDefault();
        }

        /// <summary>
        /// fetch cancel list as an asynchronous operation.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;IEnumerable&lt;Cancel&gt;&gt;.</returns>
        private async Task<IEnumerable<Cancel>> FetchCancelListAsync(Payment payment)
        {
            var cancelTransactions = GetCancels(payment.Transactions);

            var result = new List<Cancel>();
            foreach (var cancelTransaction in cancelTransactions)
            {
                var cancel = await ApiGetAsync<Cancel>(cancelTransaction.Url);
                cancel.Payment = payment;
                cancel.ResourceUrl = cancelTransaction.Url;
                cancel.TransactionType = cancelTransaction.Type;
                result.Add(cancel);
            }
            return result;
        }

        /// <summary>
        /// fetch charge list as an asynchronous operation.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;IEnumerable&lt;Charge&gt;&gt;.</returns>
        private async Task<IEnumerable<Charge>> FetchChargeListAsync(Payment payment)
        {
            var chargeTransactions = GetCharges(payment.Transactions);
            var result = new List<Charge>();
            foreach (var chargeTransaction in chargeTransactions)
            {
                var charge = await ApiGetAsync<Charge>(chargeTransaction.Url);
                charge.Payment = payment;
                charge.ResourceUrl = chargeTransaction.Url;
                charge.CancelList = GetCancelsForCharge(payment.CancelList);
                charge.TransactionType = chargeTransaction.Type;
                charge.Resources.BasketId = payment.Resources.BasketId;
                result.Add(charge);
            }
            return result;
        }

        /// <summary>
        /// Gets the cancels for authorization.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Cancel&gt;.</returns>
        private IEnumerable<Cancel> GetCancelsForAuthorization(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_AUTHORIZE.Equals(x.TransactionType, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets the cancels for charge.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Cancel&gt;.</returns>
        private IEnumerable<Cancel> GetCancelsForCharge(IEnumerable<Cancel> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_CHARGE.Equals(x.TransactionType, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets the cancels.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Transaction&gt;.</returns>
        private IEnumerable<Transaction> GetCancels(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CANCEL_CHARGE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase) 
                || TRANSACTION_TYPE_CANCEL_AUTHORIZE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets the charges.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Transaction&gt;.</returns>
        private IEnumerable<Transaction> GetCharges(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_CHARGE.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Gets the authorization.
        /// </summary>
        /// <param name="cancelList">The cancel list.</param>
        /// <returns>IEnumerable&lt;Transaction&gt;.</returns>
        private IEnumerable<Transaction> GetAuthorization(IEnumerable<Transaction> cancelList)
        {
            return cancelList?
                .Where(x => TRANSACTION_TYPE_AUTHORIZATION.Equals(x.Type, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<object> ApiGetAsync(Type resourceType, string id)
        {
            var result = await heidelpay.RestClient.HttpGetAsync(BuildApiEndpointUri(resourceType, id), resourceType);
            return PostProcessApiResource(result);
        }

        /// <summary>
        /// API get as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        private async Task<T> ApiGetAsync<T>(Uri uri)
             where T : class, IRestResource
        {
            var result = await heidelpay.RestClient.HttpGetAsync<T>(uri);
            return PostProcessApiResource(result);
        }

        /// <summary>
        /// API get as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        private async Task<T> ApiGetAsync<T>(string id)
             where T : class, IRestResource
        {
            var result = await heidelpay.RestClient.HttpGetAsync<T>(BuildApiEndpointUri<T>(id));
            return PostProcessApiResource(result);
        }

        /// <summary>
        /// API post as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="getAfterPost">if set to <c>true</c> [get after post].</param>
        /// <returns>Task&lt;T&gt;.</returns>
        private async Task<T> ApiPostAsync<T>(T resource, Uri uri = null, bool getAfterPost = true)
           where T : class, IRestResource
        {
            var posted = await heidelpay.RestClient.HttpPostAsync<T>(uri ?? BuildApiEndpointUri<T>(), resource);
            return getAfterPost 
                ? await ApiGetAsync<T>(posted.Id) 
                : PostProcessApiResource(posted);
        }

        /// <summary>
        /// API put as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="getAfterPut">if set to <c>true</c> [get after put].</param>
        /// <returns>Task&lt;T&gt;.</returns>
        private async Task<T> ApiPutAsync<T>(string id, T resource, bool getAfterPut = false)
           where T : class, IRestResource
        {
            var putted = await heidelpay.RestClient.HttpPutAsync<T>(BuildApiEndpointUri<T>(id), resource);
            return getAfterPut
                ? await ApiGetAsync<T>(putted.Id)
                : PostProcessApiResource(putted);
        }

        /// <summary>
        /// API delete as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        private async Task ApiDeleteAsync<T>(string id)
           where T : class, IRestResource
        {
            await heidelpay.RestClient.HttpDeleteAsync<T>(BuildApiEndpointUri<Customer>(id));
        }

        private Uri BuildApiEndpointUri<T>(string id = null)
            where T : class, IRestResource
        {
            return BuildApiEndpointUri(HeidelpayRegistry.ResolveResourceUrl<T>(), id);
        }

        private Uri BuildApiEndpointUri(Type resourceType, string id = null)
        {
            return BuildApiEndpointUri(HeidelpayRegistry.ResolveResourceUrl(resourceType), id);
        }

        /// <summary>
        /// Builds the API endpoint URI.
        /// </summary>
        /// <param name="resourceUrl">The resource URL.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Uri.</returns>
        private Uri BuildApiEndpointUri(string resourceUrl, string id)
        {
            var rootPath = new Uri(heidelpay.RestClient?.Options?.ApiEndpoint, heidelpay.RestClient?.Options?.ApiVersion.EnsureTrailingSlash());
            var combinedPaths = new Uri(rootPath, resourceUrl);

            if(!string.IsNullOrEmpty(id))
            {
                combinedPaths = new Uri(combinedPaths, id.EnsureTrailingSlash());
            }

            return combinedPaths;
        }


        /// <summary>
        /// Posts the process payment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        private async Task<Payment> PostProcessPayment(Payment payment)
        {
            payment.CancelList = await FetchCancelListAsync(payment);
            payment.ChargesList = await FetchChargeListAsync(payment);
            payment.Authorization = await FetchAuthorizationAsync(payment);
            return payment;
        }

        /// <summary>
        /// Posts the process API resource.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource">The resource.</param>
        /// <returns>T.</returns>
        private T PostProcessApiResource<T>(T resource)
        {
            if (resource is IHeidelpayProvider provider)
            {
                provider.Heidelpay = heidelpay;
            }

            return resource;
        }
    }
    /// <summary>
    /// Class IdResponse.
    /// Implements the <see cref="Heidelpay.Payment.Interfaces.IRestResource" />
    /// </summary>
    /// <seealso cref="Heidelpay.Payment.Interfaces.IRestResource" />
    internal class IdResponse : IRestResource
    {
        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        public string TypeUrl => null;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
    }
}
