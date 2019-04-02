using Heidelpay.Payment.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heidelpay.Payment
{
    public class Authorization : PaymentTransactionBase
    {
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public Uri ReturnUrl { get; set; }
        public Uri RedirectUrl { get; set; }

        public string OrderId { get; set; }

        public bool? Card3ds { get; set; }

        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

        [JsonConstructor]
        internal Authorization()
        {
        }

        internal Authorization(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public Authorization(IAuthorizedPaymentType paymentAuthorizable)
            : this (paymentAuthorizable.Heidelpay)
        {
            Resources.TypeId = paymentAuthorizable.Id;
        }

        [JsonProperty]
        internal Processing Processing { get; set; } = new Processing();

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
