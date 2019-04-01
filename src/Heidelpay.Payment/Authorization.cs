using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    public class Authorization : PaymentBase
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Uri ReturnUrl { get; set; }
        public Uri RedirectUrl { get; set; }

        public string OrderId { get; set; }

        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();

        [JsonProperty]
        internal Processing Processing { get; set; } = new Processing();

        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

        public Authorization()
        {
        }

        internal Authorization(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public async Task<Cancel> CancelAsync(decimal? amount = null)
        {
            return await Heidelpay.CancelAuthorizationAsync(Payment?.Id ?? Resources?.PaymentId, amount);
        }

        public async Task<Charge> ChargeAsync(decimal? amount = null)
        {
            return await Heidelpay.ChargeAuthorizationAsync(Payment?.Id ?? Resources?.PaymentId, amount);
        }

        public override string TypeUrl => "payments/<paymentId>/authorize";
    }
}
