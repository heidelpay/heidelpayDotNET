using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class PaypageTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Paypage_Minimal()
        {
            var heidelpay = Heidelpay;
            var built = new Paypage(heidelpay)
            {
                Amount = decimal.One,
                Currency = "EUR",
                ReturnUrl = TestReturnUri,
            };
                        
            var paypage = await heidelpay.PaypageAsync(built);

            var client = new HttpClient();
            var page = await client.GetAsync(paypage.RedirectUrl);
        }
    }
}
