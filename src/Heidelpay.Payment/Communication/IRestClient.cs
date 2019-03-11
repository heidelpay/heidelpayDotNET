using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Communication
{
    public interface IRestClient
    {
        Task<string> HttpGetAsync(Uri uri, string privateKey);
        Task<TResponse> HttpGetAsync<TResponse>(Uri uri, string privateKey);
    }
}
