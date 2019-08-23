using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class PaymentTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Fetch_Payment_With_Authorization()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                ReturnUrl = new Uri("https://www.heidelpay.com")
            });

            Assert.NotNull(auth?.Payment?.Id);
            Assert.Equal(auth.PaymentId, auth.Payment.Id);

            var payment = await heidelpay.FetchPaymentAsync(auth.PaymentId);

            Assert.NotNull(payment?.Id);
            Assert.NotNull(payment?.Authorization?.Id);
            Assert.NotNull(payment?.State);
        }

        [Fact]
        public async Task Full_Charge_After_Authorize()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                Card3ds = false,
                ReturnUrl = new Uri("https://www.heidelpay.com")
            });
            var payment = await Heidelpay.FetchPaymentAsync(auth.PaymentId);
            var charge = await payment.ChargeAsync();

            Assert.NotNull(charge?.Id);
        }

        [Fact]
        public async Task Fetch_Payment_With_Charges()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                Card3ds = false,
                ReturnUrl = new Uri("https://www.heidelpay.com")
            });
            var payment = await Heidelpay.FetchPaymentAsync(auth.PaymentId);

            Assert.NotNull(payment?.Id);
            Assert.NotNull(payment?.Authorization?.Id);

            var charge = await payment.ChargeAsync();
            var payment2 = charge.Payment;

            Assert.NotNull(payment2.ChargesList);
            Assert.Single(payment2.ChargesList);

            Assert.Equal(charge.Amount, payment2.ChargesList.First().Amount);
            Assert.Equal(charge.Currency, payment2.ChargesList.First().Currency);
            Assert.Equal(charge.Id, payment2.ChargesList.First().Id);
            Assert.Equal(charge.ReturnUrl, payment2.ChargesList.First().ReturnUrl);
        }

        [Fact]
        public async Task Partial_Charge_After_Authorize()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                Card3ds = false,
                ReturnUrl = new Uri("https://www.heidelpay.com")
            });
            var payment = await Heidelpay.FetchPaymentAsync(auth.PaymentId);
            var charge = await payment.ChargeAsync(decimal.One);

            Assert.NotNull(charge?.Id);
            Assert.Equal("s-chg-1", charge.Id);
            Assert.Equal(decimal.One, charge.Amount);
        }

        [Fact]
        public async Task Full_Cancel_Authorize()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                Card3ds = false,
                ReturnUrl = new Uri("https://www.heidelpay.com")
            });
            var payment = await Heidelpay.FetchPaymentAsync(auth.PaymentId);
            var cancel = await payment.Authorization.CancelAsync();
            Assert.NotNull(cancel?.Id);
            Assert.Equal("s-cnl-1", cancel.Id);
            Assert.Equal(auth.Amount, cancel.Amount);

            await Assert.ThrowsAsync<PaymentException>(() => payment.CancelAsync());
        }

        [Fact]
        public async Task Partial_Cancel_Authorize()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                Card3ds = false,
                ReturnUrl = new Uri("https://www.heidelpay.com")
            });
            var payment = await Heidelpay.FetchPaymentAsync(auth.PaymentId);
            var cancel = await payment.Authorization.CancelAsync(decimal.One);
            Assert.NotNull(cancel?.Id);
            Assert.Equal("s-cnl-1", cancel.Id);
            Assert.Equal(decimal.One, cancel.Amount);
        }

        [Fact]
        public async Task Full_Cancel_On_Charge()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(new Charge(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                Card3ds = false,
                ReturnUrl = TestReturnUri
            });
            var payment = await Heidelpay.FetchPaymentAsync(charge.PaymentId);
            var cancel = await payment.GetCharge("s-chg-1").CancelAsync();
            Assert.NotNull(cancel?.Id);
            Assert.Equal(10m, cancel.Payment.AmountCanceled);
        }

        [Fact]
        public async Task Partial_Cancel_On_Charge()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var charge = await Heidelpay.ChargeAsync(new Charge(card)
            {
                Amount = 10m,
                Currency = Currencies.EUR,
                Card3ds = false,
                ReturnUrl = TestReturnUri
            });
            var payment = await Heidelpay.FetchPaymentAsync(charge.PaymentId);
            var cancel = await payment.GetCharge("s-chg-1").CancelAsync(decimal.One);
            Assert.NotNull(cancel?.Id);
            Assert.Equal(decimal.One, cancel.Amount);
            Assert.Equal(decimal.One, cancel.Payment.AmountCanceled);
        }

        [Fact]
        public async Task Authorize()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var payment = new Payment(card);

            var authUsingPayment = await payment.AuthorizeAsync(decimal.One, Currencies.EUR, returnUrl: TestReturnUri);

            var authUsingHeidelpay = await Heidelpay.AuthorizeAsync(decimal.One, Currencies.EUR, card, TestReturnUri);

            Assert.NotNull(authUsingPayment);
            Assert.NotNull(authUsingHeidelpay);
        }

        [Fact]
        public async Task Charge_Without_Authorize()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var chargeUsingPayment = await new Payment(card).ChargeAsync(decimal.One, Currencies.EUR, TestReturnUri);
            var chargeUsingHeidelpay = await Heidelpay.ChargeAsync(decimal.One, Currencies.EUR, card, TestReturnUri);

            Assert.NotNull(chargeUsingPayment);
            Assert.NotNull(chargeUsingHeidelpay);
        }

        [Fact]
        public async Task Charge_Payment_Amount()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var chargeUsingPayment = await new Payment(card).ChargeAsync(decimal.One, Currencies.EUR, TestReturnUri);
            Assert.Equal(decimal.One, chargeUsingPayment.Payment.AmountCharged);
        }

        [Fact]
        public async Task Charge_Heidelpay_Amount()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var chargeUsingHeidelpay = await Heidelpay.ChargeAsync(decimal.One, Currencies.EUR, card, TestReturnUri);
            Assert.Equal(decimal.One, chargeUsingHeidelpay.Payment.AmountCharged);
        }
    }
}
