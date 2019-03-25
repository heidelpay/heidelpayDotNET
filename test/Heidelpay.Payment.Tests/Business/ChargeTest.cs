using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Tests.Business
{
    public class ChargeTest : PaymentTypeTestBase
    {
        [Fact]
        public async Task Charge_With_TypeId()
        {
            var heidelpay = BuildHeidelpay();
            var card = await CreatePaymentTypeCard(heidelpay);
            
            var charge = await heidelpay.ChargeAsync(decimal.One, "EUR", card, new Uri("https://www.google.at"));

            Assert.NotNull(charge);
            Assert.NotNull(charge.Id);
        }
    }
}
