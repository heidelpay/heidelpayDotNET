using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Interfaces
{
    public interface IMessage
    {
        string Code { get; }
        string Customer { get; }
        string Merchant { get; }
    }
}
