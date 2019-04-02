using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class BasketTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_Fetch_Max_Basket()
        {
            var basket = GetMaximumBasket();
            var created = await BuildHeidelpay().CreateBasketAsync(basket);
            var fetched = await BuildHeidelpay().FetchBasketAsync(created.Id);

            Assert.NotNull(created?.Id);
            Assert.NotNull(fetched?.Id);

            AssertEqual(basket, fetched);
        }

        [Fact]
        public async Task Create_Fetch_Min_Basket()
        {
            var basket = GetMinimumBasket();
            var created = await BuildHeidelpay().CreateBasketAsync(basket);
            var fetched = await BuildHeidelpay().FetchBasketAsync(created.Id);

            Assert.NotNull(created?.Id);
            Assert.NotNull(fetched?.Id);

            AssertEqual(basket, fetched);
        }

        [Fact]
        public async Task Update_Basket()
        {
            var minbasket = GetMinimumBasket();
            var created = await BuildHeidelpay().CreateBasketAsync(minbasket);
            var fetched = await BuildHeidelpay().FetchBasketAsync(created.Id);

            var maxbasket = GetMaximumBasket();
            maxbasket.Id = fetched.Id;
            maxbasket.OrderId = fetched.OrderId;

            var updated = await BuildHeidelpay().UpdateBasketAsync(maxbasket);

            Assert.NotNull(updated?.Id);

            AssertEqual(maxbasket, updated);
        }

        [Fact]
        public async Task Authorization_With_Basket()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var maxbasket = await BuildHeidelpay().CreateBasketAsync(GetMaximumBasket());
            var newAuth = new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, BasketId = maxbasket.Id };

            var auth = await BuildHeidelpay().AuthorizeAsync(newAuth);
            var payment = await BuildHeidelpay().FetchPaymentAsync(auth.PaymentId);

            Assert.NotNull(payment?.Authorization?.Id);
            Assert.Equal(maxbasket.Id, payment.BasketId);
        }

        [Fact]
        public async Task Charge_With_Basket()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var maxbasket = await BuildHeidelpay().CreateBasketAsync(GetMaximumBasket());
            var newCharge = new Charge(card)
            {
                Amount = 10m,
                Currency = "EUR",
                ReturnUrl = TestReturnUri,
                BasketId = maxbasket.Id,
            };

            var charge = await BuildHeidelpay().ChargeAsync(newCharge);
            var payment = await BuildHeidelpay().FetchPaymentAsync(charge.PaymentId);

            Assert.NotNull(payment?.Id);
            Assert.Single(payment.ChargesList);
            Assert.Equal(maxbasket.Id, payment.BasketId);
            Assert.Equal(maxbasket.Id, payment.ChargesList.First().BasketId);
        }

        private void AssertEqual(Basket expected, Basket actual)
        {
            Assert.Equal(expected.AmountTotal, actual.AmountTotal);
            Assert.Equal(expected.AmountTotalDiscount, actual.AmountTotalDiscount);
            Assert.Equal(expected.CurrencyCode, actual.CurrencyCode);
            Assert.Equal(expected.Note, actual.Note);
            Assert.Equal(expected.OrderId, actual.OrderId);
            Assert.Single(actual.BasketItems);

            AssertEqual(expected.BasketItems.First(), actual.BasketItems.First());
        }

        private void AssertEqual(BasketItem expected, BasketItem actual)
        {
            Assert.Equal(expected.AmountDiscount, actual.AmountDiscount);
            Assert.Equal(expected.AmountGross, actual.AmountGross);
            Assert.Equal(expected.AmountNet, actual.AmountNet);
            Assert.Equal(expected.AmountPerUnit, actual.AmountPerUnit);
            Assert.Equal(expected.AmountVat, actual.AmountVat);
            Assert.Equal(expected.BasketItemReferenceId, actual.BasketItemReferenceId);
            Assert.Equal(expected.Quantity, actual.Quantity);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Unit, actual.Unit);
            Assert.Equal(expected.Vat, actual.Vat);
        }
    }
}
