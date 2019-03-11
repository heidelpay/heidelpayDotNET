using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.RestClient
{
    public interface IRestResource
    {
        string TypeUrl { get; }
        string Id { get; }
    }
}
