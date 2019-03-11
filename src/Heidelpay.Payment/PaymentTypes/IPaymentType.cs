using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.PaymentTypes
{
    public interface IPaymentType
    {
        string TypeUrl { get; }
        string Id { get; }
    }
}
