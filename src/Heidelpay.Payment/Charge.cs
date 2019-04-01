using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    public class Charge : PaymentBase
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Uri ReturnUrl { get; set; }
        public Uri RedirectUrl { get; set; }
        public string OrderId { get; set; }
        public string InvoiceId { get; set; }
        public bool? Card3ds { get; set; }

        [JsonProperty]
        internal bool IsSuccess { get; set; }

        [JsonProperty]
        internal bool IsPending { get; set; }

        [JsonProperty]
        internal bool IsError { get; set; }

        [JsonIgnore]
        public Status Status
        {
            get
            {
                if (IsSuccess)
                    return Status.Success;

                if (IsPending)
                    return Status.Pending;

                if (IsError)
                    return Status.Error;

                return Status.Undefined;
            }
        }

        public string PaymentId
        {
            get
            {
                return Payment?.Id ?? Resources?.PaymentId;
            }
        }

        public string TypeId
        {
            get
            {
                return Resources?.TypeId;
            }
        }

        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();

        [JsonProperty]
        internal Processing Processing { get; set; } = new Processing();

        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

        public Charge()
        {
        }
        public Charge(string typeId)
        {
            Resources.TypeId = typeId;
        }

        public Charge(string typeId, string orderId)
        {
            Resources.TypeId = typeId;
            OrderId = orderId;
        }

        internal Charge(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public async Task<Cancel> CancelAsync(decimal? amount = null)
        {
            return await Heidelpay.CancelChargeAsync(Payment.Id, Id, amount);
        }

        public Cancel GetCancel(string cancelId)
        {
            return CancelList?.FirstOrDefault(x => string.Equals(x.Id, cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        public override string TypeUrl => "payments/<paymentId>/charges";
    }

    public enum Status
    {
        Success,
        Pending,
        Error,
        Undefined,
    }
}
