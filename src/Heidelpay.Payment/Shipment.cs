using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    public class Shipment : PaymentBase
    {
        public override string TypeUrl => "payments/<paymentId>/shipments";
    }
}
