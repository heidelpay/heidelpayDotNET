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

        private void AddIbanInvoiceParameter(HirePurchaseRatePlan ratePlan)
        {
            ratePlan.Iban = "DE46940594210000012345";
            ratePlan.Bic = "COBADEFFXXX";
            ratePlan.AccountHolder = "Rene Felder";
            ratePlan.InvoiceDate = DateTime.Now.AddDays(-1);
            ratePlan.InvoiceDueDate = DateTime.Now.AddDays(10);
        }
    }
}
