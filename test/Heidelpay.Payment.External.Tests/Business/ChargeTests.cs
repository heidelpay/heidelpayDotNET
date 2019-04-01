using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class ChargeTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Charge_With_TypeId()
        {
            var heidelpay = BuildHeidelpay();
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            var charge = await heidelpay.ChargeAsync(decimal.One, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(charge?.Id);
        }

        [Fact]
        public async Task Charge_Is_Success()
        {
            var heidelpay = BuildHeidelpay();
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);

            var charge = await heidelpay.ChargeAsync(decimal.One, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(charge?.Id);
            Assert.Equal("COR.000.100.112", charge.Message.Code);
            Assert.Equal(Status.Success, charge.Status);
        }

        [Fact]
        public async Task Charge_With_TypeId_Ensure_Payment_Type()
        {
            var heidelpay = BuildHeidelpay();
            var card = PaymentTypeCard;

            var charge = await heidelpay.ChargeAsync(decimal.One, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(charge?.Id);
        }

        [Fact]
        public async Task Charge_With_Payment_Type()
        {
            var card = new Card { Number = "4444333322221111", ExpiryDate = "12/19" };
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", card, TestReturnUri, card3ds: false);

            Assert.NotNull(charge?.Id);
            Assert.Equal("COR.000.100.112", charge.Message.Code);
            Assert.NotNull(charge.Message.Customer);
        }

        [Fact]
        public async Task Charge_With_Customer_Type_ReturnUrl()
        {
            var card = new Card { Number = "4444333322221111", ExpiryDate = "12/19" };
            var customer = GetMinimumCustomer();

            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", card, TestReturnUri, customer: customer, card3ds: false);

            Assert.NotNull(charge?.Id);
            Assert.Equal("COR.000.100.112", charge.Message.Code);
            Assert.NotNull(charge.Message.Customer);
        }

        [Fact]
        public async Task Charge_With_CustomerId_ReturnUrl()
        {
            var customer = await BuildHeidelpay().CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);

            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", card.Id, TestReturnUri, customer.Id, false);

            Assert.NotNull(charge?.Id);
            Assert.Equal("COR.000.100.112", charge.Message.Code);
            Assert.NotNull(charge.Message.Customer);
            Assert.NotNull(charge.Payment?.Id);
            Assert.Equal(charge.PaymentId, charge.Payment.Id);
        }

        [Fact]
        public async Task Charge_Sofort()
        {
            var charge = await BuildHeidelpay().ChargeAsync(decimal.One, "EUR", new Sofort(), TestReturnUri);

            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge?.RedirectUrl);
            Assert.Equal("COR.000.200.000", charge.Message.Code);
            Assert.NotNull(charge.Message.Customer);
            Assert.NotNull(charge.Payment?.Id);
            Assert.Equal(charge.PaymentId, charge.Payment.Id);
        }

        [Fact]
        public async Task Charge_OrderId()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var builtCharge = new Charge(card)
            {
                Amount = decimal.One,
                Currency = "EUR",
                ReturnUrl = TestReturnUri,
                OrderId = GetRandomId(),
                Card3ds = false
            };

            var charge = await BuildHeidelpay().ChargeAsync(builtCharge);

            Assert.NotNull(charge?.Id);
            Assert.Equal("COR.000.100.112", charge.Message.Code);
            Assert.NotNull(charge.Message.Customer);
            Assert.NotNull(charge.Payment?.Id);
            Assert.Equal(charge.PaymentId, charge.Payment.Id);

            var payment = await BuildHeidelpay().FetchPaymentAsync(charge.PaymentId);

            Assert.NotNull(payment?.Id);
            Assert.Equal(charge.PaymentId, payment.Id);
            Assert.Equal(builtCharge.OrderId, charge.OrderId);
        }

        [Fact(Skip = "Bug in API, Ticket AHC-1197")]
        public async Task Charge_With_3ds_False()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await BuildHeidelpay().ChargeAsync(new Charge(card)
            {
                Amount = decimal.One,
                Currency = "EUR",
                ReturnUrl = TestReturnUri,
                OrderId = GetRandomId(),
                Card3ds = false
            });

            Assert.NotNull(charge?.Id);
            Assert.Equal("COR.000.100.112", charge.Message.Code);
            Assert.NotNull(charge.Message.Customer);
            Assert.Equal(Status.Success, charge.Status);
            Assert.False(charge.Card3ds);
        }

        [Fact(Skip = "Bug in API, Ticket AHC-1197")]
        public async Task Charge_With_3ds_True()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await BuildHeidelpay().ChargeAsync(new Charge(card)
            {
                Amount = decimal.One,
                Currency = "EUR",
                ReturnUrl = TestReturnUri,
                OrderId = GetRandomId(),
                Card3ds = true
            });

            Assert.NotNull(charge?.Id);
            Assert.Equal("COR.000.200.000", charge.Message.Code);
            Assert.NotNull(charge.Message.Customer);
            Assert.Equal(Status.Pending, charge.Status);
            Assert.True(charge.Card3ds);
        }
    }
}
