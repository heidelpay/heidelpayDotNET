using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class PayoutTest : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Payout_Card_Minimal()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var payout = await heidelpay.PayoutAsync(decimal.One, Currencies.EUR, card, new Uri("https://www.heidelpay.com"));

            Assert.NotNull(payout?.Id);

            var payoutFetched = await heidelpay.FetchPayoutAsync(payout.PaymentId, payout.Id);

            AssertEqual(payout, payoutFetched);
        }

        [Fact]
        public async Task Payout_Card_With_All_Data()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var basket = await heidelpay.CreateBasketAsync(GetMaximumBasket());
            var customer = await heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var metadata = await heidelpay.CreateMetadataAsync(TestMetaData);

            var oot = new Payout(heidelpay, card)
            {
                Amount = 2,
                Currency = Currencies.EUR,
                OrderId = GetRandomId(),
                PaymentReference = "My Payment Reference",
                ReturnUrl = new Uri("https://www.heidelpay.com"),
                BasketId = basket.Id,
                CustomerId = customer.Id,
                MetadataId = metadata.Id,
            };

            var payout = await heidelpay.PayoutAsync(oot);

            Assert.NotNull(payout?.Id);

            var payoutFetched = await heidelpay.FetchPayoutAsync(payout.PaymentId, payout.Id);

            AssertEqual(payout, payoutFetched);
        }


        private void AssertEqual(Payout expected, Payout actual)
        {
            Assert.Equal(expected.Amount, actual.Amount);
            Assert.Equal(expected.BasketId, actual.BasketId);
            Assert.Equal(expected.Currency, actual.Currency);
            Assert.Equal(expected.CustomerId, actual.CustomerId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.MetadataId, actual.MetadataId);
            Assert.Equal(expected.OrderId, actual.OrderId);
            Assert.Equal(expected.PaymentId, actual.PaymentId);
            Assert.Equal(expected.PaymentReference, actual.PaymentReference);
            Assert.Equal(expected.RedirectUrl, actual.RedirectUrl);
            Assert.Equal(expected.ReturnUrl, actual.ReturnUrl);
            Assert.Equal(expected.Status, actual.Status);
            Assert.Equal(expected.TypeId, actual.TypeId);
        }
    }
}
