using Heidelpay.Payment.PaymentTypes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.Errors
{
    public class ErrorTests : PaymentTypeTestsBase
    {
        [Fact(Skip = "This is no longer possible as this will be checked by the SDK already")]
        public async Task Key_Missing()
        {
            var heidelpay = BuildHeidelpay("");
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.AuthorizeAsync(10m, "EUR", "s-crd-200"));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.000.000.001", error.Code);
            Assert.Equal("PrivateKey/PublicKey is missing", error.MerchantMessage);
        }

        [Fact]
        public async Task Key_Invalid()
        {
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => BuildHeidelpay("s-priv-123").CreatePaymentTypeAsync(PaymentTypeCard));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.710.000.002", error.Code);
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
            var card = await Heidelpay
                .CreatePaymentTypeAsync(PaymentTypeCard);

            var heidelpay = BuildHeidelpay("s-priv-2a1095rIVXy4IrNFXG6yQiguSAqNjciC");
            var exception = await Assert.ThrowsAsync<PaymentException>(
                () => heidelpay.FetchPaymentTypeAsync<PaymentTypeBase>(card.Id));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.710.300.003", error.Code);
            Assert.Equal("You do not have the permission to access the paymentmethod with the id " + card.Id + ".", error.MerchantMessage);
        }

        [Fact]
        public async Task Missing_Return_Url()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.AuthorizeAsync(10m, "EUR", card.Id));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.320.100.203", error.Code);
            Assert.Equal("Return URL is missing", error.MerchantMessage);
        }

        [Fact]
        public async Task Fetch_Non_Existing_Payment()
        {
            var heidelpay = Heidelpay;

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.FetchAuthorizationAsync("213"));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            // This is different from the Java equivalent, as we do not allow to pass in empty strings as paymentId, like the Java SDK does
            // Therefore we actually call the API with an invalid but not null code and receive a different error code
            Assert.Equal("API.310.100.003", error.Code);
            Assert.Equal("Payment not found with key 213", error.MerchantMessage);
            Assert.Equal("Payment is not found. Please contact us for more information.", error.CustomerMessage);
        }

        [Fact]
        public async Task Fetch_Non_Existing_Charge()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await heidelpay.ChargeAsync(new Charge(card) { Amount = decimal.One, Currency = "EUR", ReturnUrl = TestReturnUri });
            var chargeFetched = await heidelpay.FetchChargeAsync(charge.PaymentId, "s-chg-200");
            Assert.Null(chargeFetched);
        }

        [Fact]
        public async Task Invalid_PUT_Customer()
        {
            var heidelpay = Heidelpay;
            var customer = await heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));

            Assert.NotNull(customer.Id);

            var customerUpdate = new Customer(customer.Firstname, customer.Lastname)
            {
                Id = customer.Id,
                Email = "max",
            };

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.UpdateCustomerAsync(customerUpdate));

            Assert.NotNull(exception);
            Assert.Single(exception.PaymentErrorList);

            var error = exception.PaymentErrorList.First();

            Assert.Equal("API.410.200.013", error.Code);
            Assert.Equal("Email max has invalid format", error.MerchantMessage);
        }

        [Fact]
        public async Task Create_Invalid_Customer()
        {
            TryParseDateTime("1944-01-01", out DateTime dt);
            var customer = new Customer(
                "This is a very long first name because someone put the wrong content into the field" ,
                "This is a very long last name because someone put the wrong content into the field")
            {
                BirthDate = dt,
                Email = "max",
                Mobile = "xxx",
            };

            var heidelpay = Heidelpay;

            var exception = await Assert.ThrowsAsync<PaymentException>(() => heidelpay.CreateCustomerAsync(customer));

            Assert.NotNull(exception);
            Assert.Equal(4, exception.PaymentErrorList.Count());

            var ex1 = exception.PaymentErrorList.FirstOrDefault(x => x.Code == "API.410.200.005");
            var ex2 = exception.PaymentErrorList.FirstOrDefault(x => x.Code == "API.410.200.002");
            var ex3 = exception.PaymentErrorList.FirstOrDefault(x => x.Code == "API.410.200.015");
            var ex4 = exception.PaymentErrorList.FirstOrDefault(x => x.Code == "API.410.200.013");

            Assert.NotNull(ex1);
            Assert.Equal("First name This is a very long first name because someone put the wrong content into the field has invalid length", ex1.MerchantMessage);
            Assert.NotNull(ex2);
            Assert.Equal("Last name This is a very long last name because someone put the wrong content into the field has invalid length", ex2.MerchantMessage);
            Assert.NotNull(ex3);
            Assert.Equal("Phone xxx has invalid format", ex3.MerchantMessage);
            Assert.NotNull(ex4);
            Assert.Equal("Email max has invalid format", ex4.MerchantMessage);
        }
    }
}
