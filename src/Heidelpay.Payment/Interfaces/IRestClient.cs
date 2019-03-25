using Heidelpay.Payment.Options;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Interfaces
{
    public interface IRestClient
    {
        HeidelpayApiOptions Options { get; } 

        Task<string> HttpGetAsync(Uri uri, string privateKey);

        Task<string> HttpPostAsync(Uri uri, string privateKey, object content);

        Task<string> HttpPutAsync(Uri uri, string privateKey, object content);

        Task<string> HttpDeleteAsync(Uri uri, string privateKey);
    }
}
