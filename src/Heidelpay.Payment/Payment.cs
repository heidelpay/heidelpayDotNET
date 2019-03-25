using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    public class Payment : PaymentBase
    {
        public State PaymentState { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal AmountCharged { get; set; }
        public decimal AmountCanceled { get; set; }
        public decimal AmountRemaining { get; set; }

        public string Currency { get; set; }
        public string OrderId { get; set; }

        public string CustomerId { get; set; }
        public string PaymentTypeId { get; set; }
        public string MetaDataId { get; set; }
        public string BasketId { get; set; }

        public Authorization Authorization { get; set; }
        public IEnumerable<Charge> ChargesList { get; set; } = Enumerable.Empty<Charge>();
        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

        public Payment(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        public async Task<Charge> ChargeAsync(decimal? amount = null)
        {
            return await Heidelpay.ChargeAuthorizationAsync(Payment.Id, amount);
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            return await Heidelpay.ChargeAsync(amount, currency, typeId, returnUrl, customerId);
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType)
        {
            return await Heidelpay.ChargeAsync(amount, currency, paymentType);
        }

        public async Task<Charge> ChargeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            return await Heidelpay.ChargeAsync(amount, currency, paymentType, returnUrl, customerId);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, string typeId, Uri returnUrl = null, string customerId = null)
        {
            return await Heidelpay.AuthorizeAsync(amount, currency, typeId, returnUrl, customerId);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType)
        {
            return await Heidelpay.AuthorizeAsync(amount, currency, paymentType);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, string customerId)
        {
            return await Heidelpay.AuthorizeAsync(amount, currency, paymentType, returnUrl, customerId);
        }

        public async Task<Authorization> AuthorizeAsync(decimal amount, string currency, IPaymentType paymentType, Uri returnUrl, Customer customer = null)
        {
            return await Heidelpay.AuthorizeAsync(amount, currency, paymentType, returnUrl, customer);
        }

        public async Task<Cancel> CancelAsync(decimal? amount = null)
        {
            if(Authorization == null)
            {
                throw new PaymentException("Cancel is only possible for an Authorization", "Payment cancelation not possible", null, null);
            }

            return await Authorization.CancelAsync(amount);
        }

        public Charge GetCharge(string chargeId)
        {
            return ChargesList?.FirstOrDefault(x => string.Equals(x.Id, chargeId, StringComparison.InvariantCultureIgnoreCase));
        }

        public Cancel GetCancel(string cancelId)
        {
            return CancelList?.FirstOrDefault(x => string.Equals(x.Id, cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        Customer customer;
        public async Task<Customer> GetCustomerAsync()
        {
            if (customer == null && IsNotEmpty(CustomerId))
                customer = await Heidelpay.FetchCustomerAsync(CustomerId);

            return customer;
        }

        IPaymentType paymentType;
        public async Task<IPaymentType> GetPaymentTypeAsync()
        {
            if (paymentType == null && IsNotEmpty(PaymentTypeId))
                paymentType = await Heidelpay.FetchPaymentTypeAsync(PaymentTypeId);

            return paymentType;
        }

        MetaData metaData;
        public async Task<MetaData> GetMetaDataAsync()
        {
            if (metaData == null && IsNotEmpty(MetaDataId))
                metaData = await Heidelpay.FetchMetaDataAsync(MetaDataId);

            return metaData;
        }

        Basket basket;
        public async Task<Basket> GetBasketAsync()
        {
            if (basket == null && IsNotEmpty(BasketId))
                basket = await Heidelpay.FetchBasketAsync(BasketId);

            return basket;
        }

        static readonly Func<string, bool> IsNotEmpty = CoreExtensions.IsNotEmpty;

        public override string TypeUrl => "payments";
    }

    public enum State
    {
        Completed,
        Pending,
        Canceled,
        Partly,
        Payment_review,
        Chargeback,
    }
}
