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
            
            var charge = await heidelpay.ChargeAsync(decimal.One, "EUR", card, new Uri("https://www.google.at"));

            Assert.NotNull(charge);
            Assert.NotNull(charge.Id);
        }
    }
}
