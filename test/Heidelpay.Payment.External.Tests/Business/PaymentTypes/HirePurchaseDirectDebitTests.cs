using Heidelpay.Payment.PaymentTypes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class HirePurchaseDirectDebitTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Rate_Retrieval()
        {
            decimal effectiveInterestRate = 5.5m;
            DateTime orderDate = new DateTime(2019, 6, 12);

            var rateList = await Heidelpay.HirePurchaseRatesAsync(10, "EUR", effectiveInterestRate, orderDate);
            
            Assert.NotNull(rateList);
            Assert.Equal(6, rateList.Count());

            AssertRatePlan(effectiveInterestRate, orderDate, rateList.First());
        }

        [Fact]
        public async Task Create_Hire_Purchase_Type_With_Iban_Invoice_Date()
        {
            decimal effectiveInterestRate = 5.5m;
            DateTime orderDate = new DateTime(2019, 6, 12);

            var rateList = await Heidelpay.HirePurchaseRatesAsync(10, "EUR", effectiveInterestRate, orderDate);

            var plan = rateList.First();

            AddIbanInvoiceParameter(plan);

            var created = await Heidelpay.CreatePaymentTypeAsync(plan);

            Assert.NotNull(created);
            AssertRatePlan(plan, created);
        }

        [Fact]
        public async Task Create_Hire_Purchase_Type_Iban_Later()
        {
            var created = await CreatePlan();

            AddIbanInvoiceParameter(created);

            var updated = await Heidelpay.UpdatePaymentTypeAsync(created);

            Assert.NotNull(updated);
            AssertRatePlan(created, updated);
        }

        [Fact]
        public async Task Authorize_Via_Type_With_Iban()
        {
            var customer = await Heidelpay.CreateCustomerAsync(GetMaximumCustomerSameAddress(GetRandomId()));
            var basket = await Heidelpay.CreateBasketAsync(GetMaximumBasket(amount: 866.49m, discount: 0m));
            var plan = await CreatePlanWithIban();

            var authorization = await plan.AuthorizeAsync(866.49m, "EUR", TestReturnUri, customer, basket, plan.EffectiveInterestRate.Value);

            AssertAuthorization(plan, authorization);
        }

        [Fact]
        public async Task Authorize_Via_Heidelpay_TypeId_With_Iban()
        {
            var customer = GetMaximumCustomerSameAddress(GetRandomId());
            var basket = GetMaximumBasket(amount: 866.49m, discount: 0m);

            decimal effectiveInterestRate = 5.5m;
            DateTime orderDate = new DateTime(2019, 6, 12);
            var rateList = await Heidelpay.HirePurchaseRatesAsync(10, "EUR", effectiveInterestRate, orderDate);
            var plan = rateList.First();
            AddIbanInvoiceParameter(plan);

            var authorization = await Heidelpay.AuthorizeAsync(866.49m, "EUR", plan, TestReturnUri, customer, basket, plan.EffectiveInterestRate.Value);

            AssertAuthorization(plan, authorization);
        }

        [Fact]
        public async Task Charge_Via_Authorize()
        {
            var customer = GetMaximumCustomerSameAddress(GetRandomId());
            var basket = GetMaximumBasket(amount: 866.49m, discount: 0m);
            var plan = await CreatePlan();

            var authorization = await Heidelpay.AuthorizeAsync(866.49m, "EUR", plan, TestReturnUri, customer, basket, plan.EffectiveInterestRate.Value);

            var charge = await authorization.ChargeAsync();
            Assert.NotNull(charge.Processing.ExternalOrderId);
            AssertCharge(charge);
        }

        [Fact]
        public async Task Full_Cancellation_Before_Shipment()
        {
            var customer = GetMaximumCustomerSameAddress(GetRandomId());
            var basket = GetMaximumBasket(amount: 866.49m, discount: 0m);
            var plan = await CreatePlan();

            var authorization = await Heidelpay.AuthorizeAsync(866.49m, "EUR", plan, TestReturnUri, customer, basket, plan.EffectiveInterestRate.Value);

            var charge = await authorization.ChargeAsync();
            
            var cancel = await charge.CancelAsync();
            AssertCancel(cancel);
        }

        [Fact]
        public async Task Partial_Cancellation_Before_Shipment()
        {
            var customer = GetMaximumCustomerSameAddress(GetRandomId());
            var basket = GetMaximumBasket(amount: 866.49m, discount: 0m);
            var plan = await CreatePlan();

            var authorization = await Heidelpay.AuthorizeAsync(866.49m, "EUR", plan, TestReturnUri, customer, basket, plan.EffectiveInterestRate.Value);

            var charge = await authorization.ChargeAsync();

            var cancel = await charge.CancelAsync(decimal.One);
            AssertCancel(cancel, decimal.One);
        }

        [Fact]
        public async Task Test_Shipment()
        {
            var customer = GetMaximumCustomerSameAddress(GetRandomId());
            var basket = GetMaximumBasket(amount: 866.49m, discount: 0m);
            var plan = await CreatePlan();

            var authorization = await Heidelpay.AuthorizeAsync(866.49m, "EUR", plan, TestReturnUri, customer, basket, plan.EffectiveInterestRate.Value);

            var charge = await authorization.ChargeAsync();

            var shipment = await Heidelpay.ShipmentAsync(charge.PaymentId);
            AssertShipment(shipment);
        }

        [Fact]
        public async Task Test_Full_Cancel_After_Shipment()
        {
            var customer = GetMaximumCustomerSameAddress(GetRandomId());
            var basket = GetMaximumBasket(amount: 866.49m, discount: 0m);
            var plan = await CreatePlan();

            var authorization = await Heidelpay.AuthorizeAsync(866.49m, "EUR", plan, TestReturnUri, customer, basket, plan.EffectiveInterestRate.Value);

            var charge = await authorization.ChargeAsync();

            var shipment = await Heidelpay.ShipmentAsync(charge.PaymentId);
            AssertShipment(shipment);

            var cancel = await charge.CancelAsync();
            AssertCancel(cancel);
        }

        private void AssertAuthorization(HirePurchaseRatePlan ratePlan, Authorization authorization, decimal? authAmount = 866.49m)
        {
            Assert.Equal(ratePlan.EffectiveInterestRate, authorization.EffectiveInterestRate);
            AssertAuthorizationFull(authorization, authAmount);
        }

        private void AssertRatePlan(decimal effectiveInterestRate, DateTime orderDate, HirePurchaseRatePlan ratePlan)
        {
            Assert.Equal(3, ratePlan.NumberOfRates);
            Assert.Equal(effectiveInterestRate, ratePlan.EffectiveInterestRate);
            Assert.Equal(10, ratePlan.TotalPurchaseAmount);
            Assert.Equal(0.08m, ratePlan.TotalInterestAmount);
            Assert.Equal(ratePlan.TotalAmount, ratePlan.TotalInterestAmount  + ratePlan.TotalPurchaseAmount);
            Assert.Equal(3.37m, ratePlan.MonthlyRate);
            Assert.Equal(3.34m, ratePlan.LastRate);
            //Assert.Equal(1.35m, ratePlan.NominalInterestRate);
            Assert.Equal(orderDate, ratePlan.DayOfPurchase);
        }

        private void AssertRatePlan(HirePurchaseRatePlan expected, HirePurchaseRatePlan actual)
        {
            Assert.Equal(expected.AccountHolder, actual.AccountHolder);
            Assert.Equal(expected.Bic, actual.Bic);
            Assert.Equal(expected.EffectiveInterestRate, actual.EffectiveInterestRate);
            Assert.Equal(expected.FeeFirstRate, actual.FeeFirstRate);
            Assert.Equal(expected.FeePerRate, actual.FeePerRate);
            Assert.Equal(expected.InvoiceDate, actual.InvoiceDate);
            Assert.Equal(expected.InvoiceDueDate, actual.InvoiceDueDate);
            Assert.Equal(expected.LastRate, actual.LastRate);
            Assert.Equal(expected.MonthlyRate, actual.MonthlyRate);
            Assert.Equal(expected.NominalInterestRate, actual.NominalInterestRate);
            Assert.Equal(expected.NumberOfRates, actual.NumberOfRates);
            Assert.Equal(expected.DayOfPurchase, actual.DayOfPurchase);
            Assert.Equal(expected.RateList, actual.RateList);
            Assert.Equal(expected.TotalAmount, actual.TotalAmount);
            Assert.Equal(expected.TotalPurchaseAmount, actual.TotalPurchaseAmount);
        }
        private async Task<HirePurchaseRatePlan> CreatePlanWithIban()
        {
            decimal effectiveInterestRate = 5.5m;
            DateTime orderDate = new DateTime(2019, 6, 12);
            var rateList = await Heidelpay.HirePurchaseRatesAsync(10, "EUR", effectiveInterestRate, orderDate);
            var plan = rateList.First();
            AddIbanInvoiceParameter(plan);
            return await Heidelpay.CreatePaymentTypeAsync(plan);
        }

        private async Task<HirePurchaseRatePlan> CreatePlan()
        {
            decimal effectiveInterestRate = 5.5m;
            DateTime orderDate = new DateTime(2019, 6, 12);
            var rateList = await Heidelpay.HirePurchaseRatesAsync(10, "EUR", effectiveInterestRate, orderDate);
            var plan = rateList.First();
            return await Heidelpay.CreatePaymentTypeAsync(plan);
        }
        private void AddIbanInvoiceParameter(HirePurchaseRatePlan ratePlan)
        {
            ratePlan.Iban = "DE46940594210000012345";
            ratePlan.Bic = "COBADEFFXXX";
            ratePlan.AccountHolder = "Rene Felder";
            ratePlan.InvoiceDate = DateTime.Now.Date.AddDays(-1);
            ratePlan.InvoiceDueDate = DateTime.Now.Date.AddDays(10);
        }
    }
}
