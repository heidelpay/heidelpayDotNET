using Heidelpay.Payment.Communication.Internal;

namespace Heidelpay.Payment
{
    public class Shipment : PaymentBase
    {
        public override string TypeUrl => "payments/<paymentId>/shipments";
    }
}
