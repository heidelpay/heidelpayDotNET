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
    }
}
