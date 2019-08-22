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
                Currency = Currencies.EUR,
                ReturnUrl = TestReturnUri,
            };
                        
            var paypage = await heidelpay.PaypageAsync(built);

            Assert.NotNull(paypage?.RedirectUrl);
        }

        [Fact]
        public async Task Paypage_Maximum()
        {
            var customer = await Heidelpay.CreateCustomerAsync(GetMaximumCustomerSameAddress(GetRandomId()));
            var basket = await Heidelpay.CreateBasketAsync(GetMaximumBasket());
            var metadata = await Heidelpay.CreateMetadataAsync(TestMetaData);

            var built = new Paypage(Heidelpay)
            {
                Amount = 123,
                Currency = Currencies.EUR,
                ReturnUrl = TestReturnUri,
                CustomerId = customer.Id,
                BasketId = basket.Id,
                MetadataId = metadata.Id,

                ShopDescription = "My personal shop",
                ShopName = "Shop.de",
                ContactUri = new Uri("mailto:rene.felder@heidelpay.com"),
                HelpUri = new Uri("https://www.heidelpay.com/de/support/"),
                ImpressumUri = new Uri("https://www.heidelpay.com/de/impressum/"),
                PrivacyPolicyUri = new Uri("https://www.heidelpay.com/de/datenschutz/"),
                TermsAndConditionUri = new Uri("https://www.heidelpay.com/de/datenschutz/"),
            };

            var paypage = await Heidelpay.PaypageAsync(built);

            Assert.NotNull(paypage?.RedirectUrl);
        }
    }
}
