using Heidelpay.Payment.Communication.Internal;
using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    public class Payment : PaymentBase
    {
        [JsonProperty(PropertyName = "State")]
        internal StateValue StateValue { get; set; }

        [JsonIgnore]
        public State State
        {
            get
            {
                return (State)StateValue.Id;
            }
        }

        public string Currency { get; set; }
        public string OrderId { get; set; }

        public Authorization Authorization { get; set; }
        public IEnumerable<Charge> ChargesList { get; set; } = Enumerable.Empty<Charge>();
        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

        public Resources Resources { get; set; } = new Resources();

        public decimal AmountTotal { get => Amount.Total; }
        public decimal AmountCharged { get => Amount.Charged; }
        public decimal AmountCanceled { get => Amount.Canceled; }
        public decimal AmountRemaining { get => Amount.Remaining; }

        [JsonProperty]
        internal Amount Amount { get; set; } = new Amount();

        [JsonProperty]
        internal IEnumerable<Transaction> Transactions { get; set; } = Enumerable.Empty<Transaction>();

        public Payment(Heidelpay heidelpay)
            : base(heidelpay)
        {

        }

        internal Payment()
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
            Check.ThrowIfTrue(Authorization == null, 
                merchantMessage: "Cancel is only possible for an Authorization", 
                customerMessage: "Payment cancelation not possible");

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
            if (customer == null && IsNotEmpty(Resources?.CustomerId))
                customer = await Heidelpay.FetchCustomerAsync(Resources?.CustomerId);

            return customer;
        }

        IPaymentType paymentType;
        public async Task<IPaymentType> GetPaymentTypeAsync()
        {
            if (paymentType == null && IsNotEmpty(Resources?.TypeId))
                paymentType = await Heidelpay.FetchPaymentTypeAsync(Resources?.TypeId);

            return paymentType;
        }

        MetaData metaData;
        public async Task<MetaData> GetMetaDataAsync()
        {
            if (metaData == null && IsNotEmpty(Resources?.MetadataId))
                metaData = await Heidelpay.FetchMetaDataAsync(Resources?.MetadataId);

            return metaData;
        }

        Basket basket;
        public async Task<Basket> GetBasketAsync()
        {
            if (basket == null && IsNotEmpty(Resources?.BasketId))
                basket = await Heidelpay.FetchBasketAsync(Resources?.BasketId);

            return basket;
        }

        static readonly Func<string, bool> IsNotEmpty = CoreExtensions.IsNotEmpty;

        public override string TypeUrl => "payments";
    }

    public enum State
    {
        Pending = 0,
        Completed = 1,
        Canceled = 2,
        Partly = 3,
        Payment_review = 4,
        Chargeback = 5,
    }

    internal class StateValue
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
