using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Internal.Tests.Business.Errors
{
    public class ErrorTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Key_Missing()
        {
            var heidelpay = BuildHeidelpay("");
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.AuthorizeAsync(GetAuthorization("")));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.000.000.001", error.Code);
            Assert.Equal("PrivateKey/PublicKey is missing", error.MerchantMessage);
        }

        [Fact]
        public async Task Key_Invalid()
        {
            var heidelpay = BuildHeidelpay("s-priv-123");
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.AuthorizeAsync(GetAuthorization("")));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.320.000.002", error.Code);
            Assert.Equal("The given key s-priv-123 is unknown or invalid.", error.MerchantMessage);
        }

        [Fact]
        public async Task PCI_Level_Saq_A()
        {
            var heidelpay = BuildHeidelpay("s-pub-2a10xITCUtmO2FlTP8RKB3OhdnKI4RmU"); // Prod Sandbox
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.CreatePaymentTypeAsync(PaymentTypeCard));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.710.000.005", error.Code);
            Assert.Equal("You do not have the permission to access this resource. Please contact the owner of the shop.", error.CustomerMessage);
        }

        [Fact]
        public async Task Invalid_Access()
        {
            var card = await BuildHeidelpay()
                .CreatePaymentTypeAsync(PaymentTypeCard);

            var heidelpay = BuildHeidelpay("s-priv-2a1095rIVXy4IrNFXG6yQiguSAqNjciC");
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.FetchPaymentTypeAsync(card.Id));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.710.300.003", error.Code);
            Assert.Equal("You do not have the permission to access the paymentmethod with the id " + card.Id + ".", error.MerchantMessage);
        }

        [Fact]
        public async Task Missing_Return_Url()
        {
            var heidelpay = BuildHeidelpay();
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            var auth = GetAuthorization(card.Id);
            auth.ReturnUrl = null;

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.AuthorizeAsync(auth));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.320.100.203", error.Code);
            Assert.Equal("Return URL is missing", error.MerchantMessage);
        }

        [Fact]
        public async Task PaymentTypeId_Missing()
        {
            var heidelpay = BuildHeidelpay();
            var auth = GetAuthorization("");

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.AuthorizeAsync(auth));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.320.200.143", error.Code);
            Assert.Equal("Resources type id is missing", error.MerchantMessage);
        }

        [Fact]
        public async Task Fetch_Non_Existing_Payment()
        {
            var heidelpay = BuildHeidelpay();

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.FetchAuthorizationAsync(""));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.310.000.006", error.Code);
            Assert.Equal("Http GET method is not supported", error.MerchantMessage);
        }
    }
}
