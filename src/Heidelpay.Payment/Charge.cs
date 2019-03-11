using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heidelpay.Payment
{
    public class Charge : PaymentBase
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public Uri ReturnUrl { get; set; }
        public Uri RedirectUrl { get; set; }
        public string OrderId { get; set; }
        public string TypeId { get; set; }
        public string CustomerId { get; set; }
        public string MetadataId { get; set; }
        public string PaymentId { get; set; }
        public string RiskId { get; set; }
        public string BasketId { get; set; }
        public Processing Processing { get; set; } = new Processing();

        public IEnumerable<Cancel> CancelList { get; set; } = Enumerable.Empty<Cancel>();

        public Charge()
        {
        }

        public Charge(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public Cancel Cancel()
        {
            return Heidelpay.CancelCharge(Payment.Id, Id);
        }

        public Cancel Cancel(decimal amount)
        {
            return Heidelpay.CancelCharge(Payment.Id, Id, amount);
        }

        public Cancel GetCancel(string cancelId)
        {
            return CancelList?.FirstOrDefault(x => string.Equals(x.Id, cancelId, StringComparison.InvariantCultureIgnoreCase));
        }

        public override string TypeUrl => "payments/<paymentId>/charges";
    }
}
