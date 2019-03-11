using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heidelpay.Payment
{
    public class Authorization : PaymentBase
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

        public Authorization()
        {
        }

        public Authorization(Heidelpay heidelpay)
            : base(heidelpay)
        {
        }

        public override string TypeUrl => "payments/<paymentId>/authorize";
    }
}
