using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class BusinessCustomerTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_Registered_Business_Customer()
        {
            var customer = GetMinimumBusinessCustomerRegistered();
            var min = await Heidelpay.CreateCustomerAsync(customer);

            Assert.NotNull(min?.Id);

            BusinessCustomerEquals(customer, min);
        }

        [Fact]
        public async Task Create_Registered_Maximum_Business_Customer()
        {
            var customer = GetMaximumBusinessCustomerRegistered();
            var min = await Heidelpay.CreateCustomerAsync(customer);

            Assert.NotNull(min?.Id);

            BusinessCustomerEquals(customer, min);
        }

        private void BusinessCustomerEquals(Customer expected, Customer actual)
        {
            AssertEquals(expected, actual);

            Assert.Equal(expected.CompanyName, actual.CompanyName);
            Assert.Equal(expected.CompanyInfo.CommercialRegisterNumber, actual.CompanyInfo.CommercialRegisterNumber);
            Assert.Equal(expected.CompanyInfo.CommercialSector, actual.CompanyInfo.CommercialSector);
        }
    }
}
