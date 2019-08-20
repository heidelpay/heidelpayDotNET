using Heidelpay.Payment.PaymentTypes;
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
        public async Task Rate_Retrieval()
        {
            decimal effectiveInterestRate = 5.5m;
            DateTime orderDate = new DateTime(2019, 6, 12);
            var rateList = Heidelpay.HirePurchaseRatesAsync()
        }
    }
}
