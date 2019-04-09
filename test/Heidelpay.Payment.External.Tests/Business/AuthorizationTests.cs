using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class AuthorizationTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Authorize_With_Authorization_Object()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var newAuth = new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false };

            var auth = await Heidelpay.AuthorizeAsync(newAuth);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_TypeId()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_Success()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Success, auth.Status);
        }

        [Fact]
        public async Task Authorize_Pending()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard3DS);

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.200.000", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Pending, auth.Status);
        }

        [Fact]
        public async Task Authorize_3DS_True_Override_False_Id()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard3DS);

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Success, auth.Status);
        }

        [Fact]
        public async Task Authorize_3DS_True_Override_False_Type()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard3DS);

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Success, auth.Status);
        }

        [Fact]
        public async Task Authorize_3DS_False_Override_True_Id()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri, card3ds: true);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.200.000", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Pending, auth.Status);
        }

        [Fact]
        public async Task Authorize_3DS_False_Override_True_Type()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card, TestReturnUri, card3ds: true);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.200.000", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Pending, auth.Status);
        }

        [Fact]
        public async Task Authorize_With_PaymentType()
        {
            var card = new Card(Heidelpay) { Number = "4444333322221111", ExpiryDate = "12/19" };

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_PaymentType_And_Customer()
        {
            var card = new Card(Heidelpay) { Number = "4444333322221111", ExpiryDate = "12/19" };
            var customer = new Customer("Max", "Musterperson");

            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card, TestReturnUri, customer, false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);

            var payment = auth.Payment;
            Assert.NotNull(payment);
            
            var paymentType = await payment.FetchPaymentTypeAsync();
            Assert.NotNull(paymentType);
            Assert.True(paymentType is Card);

            var cust = await payment.FetchCustomerAsync();
            Assert.NotNull(cust);

            Assert.Equal("Max", cust.Firstname);
            Assert.Equal("Musterperson", cust.Lastname);
        }

        [Fact]
        public async Task Authorize_With_CustomerId()
        {
            var customer = await Heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri, customer.Id, false);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_ReturnUrl_Id()
        {
            var customer = await Heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri, customer.Id, false);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_ReturnUrl_Type()
        {
            var customer = await Heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card, TestReturnUri, customer, card3ds: false);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Fetch_Authorization()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await Heidelpay.AuthorizeAsync(10m, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);

            var fetched = await Heidelpay.FetchAuthorizationAsync(auth.PaymentId);

            Assert.NotNull(fetched?.Id);
            Assert.Equal("COR.000.100.112", fetched.Message.Code);
            Assert.NotNull(fetched.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_OrderId()
        {
            var orderId = GetRandomId();
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var newAuth = new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, OrderId = orderId };
            var auth = await Heidelpay.AuthorizeAsync(newAuth);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(orderId, auth.OrderId);
        }
    }
}
