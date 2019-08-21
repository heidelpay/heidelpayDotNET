﻿using Heidelpay.Payment.PaymentTypes;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class GiropayTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Giropay>();
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Giropay>();
            var charge = await Heidelpay.ChargeAsync(decimal.One, "EUR", result, TestReturnUri);
            AssertCharge(charge, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync<Giropay>();
            var fetched = await Heidelpay.FetchPaymentTypeAsync<Giropay>(result.Id);
            Assert.NotNull(fetched?.Id);
        }
    }
}
