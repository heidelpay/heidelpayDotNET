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
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var newAuth = new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false };

            var auth = await BuildHeidelpay().AuthorizeAsync(newAuth);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_TypeId()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCardNo3DS);

            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_Success()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCardNo3DS);

            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Success, auth.Status);
        }

        [Fact]
        public async Task Authorize_Pending()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard3DS);

            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.200.000", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Pending, auth.Status);
        }

        [Fact]
        public async Task Authorize_3DS_Override_Id()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard3DS);

            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.200.000", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Success, auth.Status);
        }

        [Fact]
        public async Task Authorize_3DS_Override_Type()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard3DS);

            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.200.000", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(Status.Success, auth.Status);
        }

        [Fact]
        public async Task Authorize_With_PaymentType()
        {
            var card = new Card(BuildHeidelpay()) { Number = "4444333322221111", ExpiryDate = "12/19" };

            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_PaymentType_And_Customer()
        {
            var card = new Card(BuildHeidelpay()) { Number = "4444333322221111", ExpiryDate = "12/19" };
            var customer = new Customer("Max", "Musterperson");

            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card, TestReturnUri, customer, false);

            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);

            var payment = auth.Payment;
            Assert.NotNull(payment);
            
            var paymentType = await payment.GetPaymentTypeAsync();
            Assert.NotNull(paymentType);
            Assert.True(paymentType is Card);

            var cust = await payment.GetCustomerAsync();
            Assert.NotNull(cust);

            Assert.Equal("Max", cust.Firstname);
            Assert.Equal("Musterperson", cust.Lastname);
        }

        [Fact]
        public async Task Authorize_With_CustomerId()
        {
            var customer = await BuildHeidelpay().CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri, customer.Id, false);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_ReturnUrl_Id()
        {
            var customer = await BuildHeidelpay().CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card.Id, TestReturnUri, customer.Id, false);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_ReturnUrl_Type()
        {
            var customer = await BuildHeidelpay().CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card, TestReturnUri, customer, card3ds: false);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
        }

        public async Task Fetch_Authorization()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard3DS);
            var auth = await BuildHeidelpay().AuthorizeAsync(10m, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(auth?.Id);

            var fetched = await BuildHeidelpay().FetchAuthorizationAsync(auth.PaymentId);

            Assert.NotNull(fetched?.Id);
            Assert.Equal("COR.000.100.112", fetched.Message.Code);
            Assert.NotNull(fetched.Message.Customer);
        }

        [Fact]
        public async Task Authorize_With_OrderId()
        {
            var orderId = GetRandomId();
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var newAuth = new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, OrderId = orderId };
            var auth = await BuildHeidelpay().AuthorizeAsync(newAuth);
            Assert.NotNull(auth?.Id);
            Assert.Equal("COR.000.100.112", auth.Message.Code);
            Assert.NotNull(auth.Message.Customer);
            Assert.Equal(orderId, auth.OrderId);
        }
    }
}
