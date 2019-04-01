using Heidelpay.Payment.Options;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Interfaces
{
    public interface IRestClient
    {
        HeidelpayApiOptions Options { get; }

        Task<object> HttpGetAsync(Uri uri, Type responseType);
        Task<object> HttpPostAsync(Uri uri, object content, Type responseType);

        Task<T> HttpGetAsync<T>(Uri uri) where T : class;
        Task<T> HttpPostAsync<T>(Uri uri, object content) where T : class;
        Task<T> HttpPutAsync<T>(Uri uri, object content) where T : class;

        Task<bool> HttpDeleteAsync<T>(Uri uri) where T : class;
    }
}
