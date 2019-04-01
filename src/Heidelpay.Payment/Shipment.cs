using Newtonsoft.Json;

namespace Heidelpay.Payment
{
    public class Shipment : PaymentBase
    {
        public string InvoiceId { get; set; }

        [JsonProperty]
        public Payment Payment { get; internal set; }

        [JsonProperty]
        internal Resources Resources { get; set; } = new Resources();

        public override string TypeUrl => "payments/<paymentId>/shipments";
    }
}
