using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    public class Cancel : PaymentBase
    {
        public decimal Amount { get; set; }

        [JsonProperty]
        internal Processing Processing { get; set; } = new Processing();

        public Cancel()
        {
        }

        internal Cancel(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public override string TypeUrl => "payments/<paymentId>/authorize/cancels";
    }
}
