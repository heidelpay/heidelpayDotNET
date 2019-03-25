using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Interfaces
{
    internal interface IHeidelpayProvider
    {
        Heidelpay Heidelpay { get; set; }
    }
}
