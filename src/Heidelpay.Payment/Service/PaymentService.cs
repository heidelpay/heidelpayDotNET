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
            Check.NotNull(heidelpay, nameof(heidelpay));

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
        /// create customer as an asynchronous operation.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Check.NotNull(customer, nameof(customer));

            return await ApiPostAsync(customer);
        }

        /// <summary>
        /// update customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> UpdateCustomerAsync(string id, Customer customer)
        {
            Check.NotNullOrEmpty(id, nameof(id));
            Check.NotNull(customer, nameof(customer));

            return await ApiPutAsync(id, customer);
        }

        /// <summary>
        /// delete customer as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        public async Task DeleteCustomerAsync(string id)
        {
            Check.NotNullOrEmpty(id, nameof(id));

            await ApiDeleteAsync<Customer>(id);
        }

        /// <summary>
        /// charge as an asynchronous operation.
        /// </summary>
        /// <param name="charge">The charge.</param>
        /// <returns>Task&lt;Charge&gt;.</returns>
        public async Task<Charge> ChargeAsync(Charge charge)
        {
            Check.NotNull(charge, nameof(charge));

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
            Check.NotNull(authorization, nameof(authorization));

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
            Check.NotNullOrEmpty(paymentTypeId, nameof(paymentTypeId));
            var paymentType = ResolvePaymentTypeFromTypeId(paymentTypeId, postProcess: true);

            return await ApiGetAsync(paymentTypeId, paymentType) as TPaymentBase;
        }

        /// <summary>
        /// fetch authorization as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Authorization&gt;.</returns>
        public async Task<Authorization> FetchAuthorizationAsync(string paymentId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
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
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));

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
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));
            Check.NotNullOrEmpty(cancelId, nameof(cancelId));

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

        /// <summary>
        /// fetch payment as an asynchronous operation.
        /// </summary>
        /// <param name="paymentId">The payment identifier.</param>
        /// <returns>Task&lt;Payment&gt;.</returns>
        public async Task<Payment> FetchPaymentAsync(string paymentId)
        {
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            var payment = await ApiGetAsync(new Payment { Id = paymentId });
            return await PostProcessPayment(payment);
        }

        /// <summary>
        /// fetch basket as an asynchronous operation.
        /// </summary>
        /// <param name="basketId">The basket identifier.</param>
        /// <returns>Task&lt;Basket&gt;.</returns>
        public async Task<Basket> FetchBasketAsync(string basketId)
        {
            Check.NotNullOrEmpty(basketId, nameof(basketId));

            return await ApiGetAsync(new Basket { Id = basketId });
        }

        /// <summary>
        /// fetch customer as an asynchronous operation.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>Task&lt;Customer&gt;.</returns>
        public async Task<Customer> FetchCustomerAsync(string customerId)
        {
            Check.NotNullOrEmpty(customerId, nameof(customerId));

            return await ApiGetAsync(new Customer { Id = customerId });
        }

        /// <summary>
        /// fetch meta data as an asynchronous operation.
        /// </summary>
        /// <param name="metaDataId">The meta data identifier.</param>
        /// <returns>Task&lt;MetaData&gt;.</returns>
        public async Task<MetaData> FetchMetaDataAsync(string metaDataId)
        {
            Check.NotNullOrEmpty(metaDataId, nameof(metaDataId));

            return await ApiGetAsync(new MetaData { Id = metaDataId });
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
            var response = (await heidelpay.RestClient.HttpPostAsync(BuildApiEndpointUri(resource), resource, typeof(IdResponse))) as IdResponse;
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

            var paymentUri = BuildApiEndpointUri(shipment.ResolvePaymentUrl(paymentId), null);
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
            Check.NotNull(charge, nameof(charge));
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(charge, BuildApiEndpointUri(charge.ResolvePaymentUrl(paymentId), null), getAfterPost: false);

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
            Check.NotNull(cancel, nameof(cancel));
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(cancel, BuildApiEndpointUri(cancel.ResolvePaymentUrl(paymentId), null), getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

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
            Check.NotNull(cancel, nameof(cancel));
            Check.NotNullOrEmpty(chargeId, nameof(chargeId));
            Check.NotNullOrEmpty(paymentId, nameof(paymentId));

            var result = await ApiPostAsync(cancel, BuildApiEndpointUri(cancel.ResolveRefundUrl(paymentId, chargeId), null), getAfterPost: false);

            result.Payment = await FetchPaymentAsync(result.Resources.PaymentId);

            return result;
        }

        /// <summary>
        /// Resolves the payment type from type identifier.
        /// </summary>
        /// <param name="typeId">The type identifier.</param>
        /// <param name="postProcess">if set to <c>true</c> [post process].</param>
        /// <returns>PaymentTypeBase.</returns>
        /// <exception cref="PaymentException">Type '" + shortTypeId + "' is currently not supported by the SDK</exception>
        internal PaymentTypeBase ResolvePaymentTypeFromTypeId(string typeId, bool postProcess)
        {
            Check.NotNullOrEmpty(typeId, nameof(typeId));
            Check.ThrowIfTrue(typeId.Length < 5, "TypeId '" + typeId + "' is invalid");

            var shortTypeId = typeId
                .Substring(2, 3)
                .ToLower();

            PaymentTypeBase result = null;

            switch (shortTypeId)
            {
                case "crd": result = new Card(); break;
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

            if (postProcess) PostProcessApiResource(result);

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

        /// <summary>
        /// API get as an asynchronous operation.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>Task&lt;System.Object&gt;.</returns>
        private async Task<object> ApiGetAsync(string id, IRestResource resource)
        {
            var result = await heidelpay.RestClient.HttpGetAsync(BuildApiEndpointUri(resource, id), resource.GetType());
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
        private async Task<T> ApiGetAsync<T>(T resource)
             where T : class, IRestResource
        {
            var result = await heidelpay.RestClient.HttpGetAsync<T>(BuildApiEndpointUri(resource, resource.Id));
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
            var posted = await heidelpay.RestClient.HttpPostAsync<T>(uri ?? BuildApiEndpointUri(resource), resource);
            return getAfterPost 
                ? await ApiGetAsync(posted) 
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
            var putted = await heidelpay.RestClient.HttpPutAsync<T>(BuildApiEndpointUri(resource, id), resource);
            return getAfterPut
                ? await ApiGetAsync(putted)
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
            await heidelpay.RestClient.HttpDeleteAsync<T>(BuildApiEndpointUri(default(T), id));
        }

        /// <summary>
        /// Builds the API endpoint URI.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>Uri.</returns>
        private Uri BuildApiEndpointUri(IRestResource resource, string id = null)
        {
            return BuildApiEndpointUri(resource.ResolveResourceUrl(), id);
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
