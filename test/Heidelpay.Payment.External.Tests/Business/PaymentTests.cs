using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class PaymentTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Fetch_Payment_With_Authorization()
        {
            var heidelpay = BuildHeidelpay();
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = "EUR",
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
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card)
            {
                Amount = 10m,
                Currency = "EUR",
                Card3ds = false,
                ReturnUrl = new Uri("https://www.heidelpay.com")
            });
            var payment = await BuildHeidelpay().FetchPaymentAsync(auth.PaymentId);
            var charge = await payment.ChargeAsync();

            Assert.NotNull(charge?.Id);
        }
    }
}
