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
            var created = await Heidelpay.CreateBasketAsync(basket);
            var fetched = await Heidelpay.FetchBasketAsync(created.Id);

            Assert.NotNull(created?.Id);
            Assert.NotNull(fetched?.Id);

            AssertEqual(basket, fetched);
        }

        [Fact]
        public async Task Create_Fetch_Min_Basket()
        {
            var basket = GetMinimumBasket();
            var created = await Heidelpay.CreateBasketAsync(basket);
            var fetched = await Heidelpay.FetchBasketAsync(created.Id);

            Assert.NotNull(created?.Id);
            Assert.NotNull(fetched?.Id);

            AssertEqual(basket, fetched);
        }

        [Fact]
        public async Task Update_Basket()
        {
            var minbasket = GetMinimumBasket();
            var created = await Heidelpay.CreateBasketAsync(minbasket);
            var fetched = await Heidelpay.FetchBasketAsync(created.Id);

            var maxbasket = GetMaximumBasket();
            maxbasket.Id = fetched.Id;
            maxbasket.OrderId = fetched.OrderId;

            var updated = await Heidelpay.UpdateBasketAsync(maxbasket);

            Assert.NotNull(updated?.Id);

            AssertEqual(maxbasket, updated);
        }

        [Fact]
        public async Task Authorization_With_Basket()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var maxbasket = await Heidelpay.CreateBasketAsync(GetMaximumBasket());
            var newAuth = new Authorization(card) 
            { 
                Amount = StandardChargedBasketResult, 
                Currency = Currencies.EUR,
                ReturnUrl = TestReturnUri,
                BasketId = maxbasket.Id
            };

            var auth = await Heidelpay.AuthorizeAsync(newAuth);
            var payment = await Heidelpay.FetchPaymentAsync(auth.PaymentId);

            Assert.NotNull(payment?.Authorization?.Id);
            Assert.Equal(maxbasket.Id, payment.BasketId);
        }

        [Fact]
        public async Task Charge_With_Basket()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var maxbasket = await Heidelpay.CreateBasketAsync(GetMaximumBasket());
            var newCharge = new Charge(card)
            {
                Amount = StandardChargedBasketResult,
                Currency = Currencies.EUR,
                ReturnUrl = TestReturnUri,
                BasketId = maxbasket.Id,
            };

            var charge = await Heidelpay.ChargeAsync(newCharge);
            var payment = await Heidelpay.FetchPaymentAsync(charge.PaymentId);

            Assert.NotNull(payment?.Id);
            Assert.Single(payment.ChargesList);
            Assert.Equal(maxbasket.Id, payment.BasketId);
            Assert.Equal(maxbasket.Id, payment.ChargesList.First().BasketId);
        }
    }
}
