using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class CardTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType_Via_Config()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            Assert.NotNull(result?.Id);
            Assert.NotNull(result?.CardDetails);
            Assert.NotNull(result?.CardDetails?.CountryName);
            Assert.NotNull(result?.CardDetails?.CountryIsoA2);
        }

        [Fact]
        public async Task Create_PaymentType_Via_Instance()
        {
            var card = new Card(Heidelpay);
            PaymentTypeCard(card);

            var result = await Heidelpay.CreatePaymentTypeAsync(card);
            Assert.NotNull(result?.Id);
            Assert.NotNull(result?.CardDetails);
            Assert.NotNull(result?.CardDetails?.CountryName);
            Assert.NotNull(result?.CardDetails?.CountryIsoA2);
        }

        [Fact]
        public async Task Authorize_PaymentType()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var authorization = await card.AuthorizeAsync(decimal.One, Currencies.EUR, ShopReturnUri);
            AssertAuthorizationSimple(authorization, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Authorize_And_Payment_PaymentType()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var authorization = await card.AuthorizeAsync(decimal.One, Currencies.EUR, ShopReturnUri);
            var payment = authorization.Payment;
            AssertAuthorizationSimple(authorization, decimal.One, Status.Pending);
            Assert.NotNull(payment?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await card.ChargeAsync(decimal.One, Currencies.EUR, ShopReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
            Assert.Equal(card.Id, charge.TypeId);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var createdCard = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            Assert.NotNull(createdCard?.Id);
            Assert.NotNull(createdCard.CVC);
            Assert.Equal("03/2020", createdCard.ExpiryDate);
            Assert.Equal("444433******1111", createdCard.Number);

            var fetchedCard = await Heidelpay.FetchPaymentTypeAsync<Card>(createdCard.Id);

            Assert.NotNull(fetchedCard?.Id);
            Assert.NotNull(fetchedCard.CVC);
            Assert.Equal("03/2020", fetchedCard.ExpiryDate);
            Assert.Equal("444433******1111", fetchedCard.Number);
        }
    }
}
