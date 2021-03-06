﻿using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class AlipayTests : PaymentTypeTestsBase
    {
        private Action<Alipay> ConfigurePaymentType { get; } = new Action<Alipay>(x =>
        {
            
        });

        [Fact]
        public async Task Create_PaymentType_Via_Config()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_PaymentType_Via_Instance()
        {
            var instance = new Alipay(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var typeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            var charge = await typeInstance.ChargeAsync(decimal.One, Currencies.EUR, ShopReturnUri);
            Assert.Equal(typeInstance.Id, charge.TypeId);
            AssertCharge(charge, decimal.One, Status.Pending);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var createdTypeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);

            Assert.NotNull(createdTypeInstance?.Id);

            var fetchedCard = await Heidelpay.FetchPaymentTypeAsync<Alipay>(createdTypeInstance.Id);

            Assert.NotNull(fetchedCard?.Id);
        }
    }
}
