using System;
using System.Threading.Tasks;

namespace Heidelpay.Payment.Interfaces
{
    public interface IRestClient
    {
        Task<string> HttpGetAsync(Uri uri, string privateKey);

        Task<string> HttpPostAsync(Uri uri, string privateKey, object content);

        Task<string> HttpPutAsync(Uri uri, string privateKey, object content);

        Task<string> HttpDeleteAsync<TData>(Uri uri, string privateKey);
    }
}
