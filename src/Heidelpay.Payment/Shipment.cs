namespace Heidelpay.Payment
{
    public class Shipment : PaymentBase
    {
        public string InvoiceId { get; set; }

        public override string TypeUrl => "payments/<paymentId>/shipments";
    }
}
