using Heidelpay.Payment.Options;
using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Interfaces
{
    public interface IRestClient
    {
        HeidelpayApiOptions Options { get; }

        Task<object> HttpGetAsync(Uri uri, Type type);

        Task<T> HttpGetAsync<T>(Uri uri);

        Task<T> HttpPostAsync<T>(Uri uri, object content);

        Task<T> HttpPutAsync<T>(Uri uri, object content);

        Task<T> HttpDeleteAsync<T>(Uri uri);
    }
}
