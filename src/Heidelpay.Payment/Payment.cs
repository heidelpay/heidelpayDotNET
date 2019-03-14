using Heidelpay.Payment.PaymentTypes;
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

        Customer customer;
        public async Task<Customer> GetCustomerAsync()
        {
            if (customer == null && !string.IsNullOrEmpty(CustomerId))
                customer = await Heidelpay.FetchCustomerAsync(CustomerId);

            return customer;
        }

        PaymentType paymentType;
        public async Task<PaymentType> GetPaymentTypeAsync()
        {
            if (paymentType == null && !string.IsNullOrEmpty(PaymentTypeId))
                paymentType = await Heidelpay.FetchPaymentTypeAsync(PaymentTypeId);

            return paymentType;
        }

        MetaData metaData;
        public async Task<MetaData> GetMetaDataAsync()
        {
            if (metaData == null && !string.IsNullOrEmpty(MetaDataId))
                metaData = await Heidelpay.FetchMetaDataAsync(MetaDataId);

            return metaData;
        }

        Basket basket;
        public async Task<Basket> GetBasketAsync()
        {
            if (basket == null && !string.IsNullOrEmpty(BasketId))
                basket = await Heidelpay.FetchBasketAsync(BasketId);

            return basket;
        }

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
