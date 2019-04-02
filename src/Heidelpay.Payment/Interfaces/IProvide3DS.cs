using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Interfaces
{
    internal interface IProvide3DS
    {
        bool? ThreeDs { get; }
    }
}
