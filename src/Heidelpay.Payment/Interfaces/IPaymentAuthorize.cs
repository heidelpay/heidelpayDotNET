using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Interfaces
{
    public interface IPaymentAuthorize : IPaymentType
    {
        Heidelpay Heidelpay { get; }
    }
}
