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
            var max = await Heidelpay.CreateCustomerAsync(customer);

            Assert.NotNull(max?.Id);

            BusinessCustomerEquals(customer, max);
        }

        [Fact]
        public async Task Fetch_Registered_Maximum_Business_Customer()
        {
            var customer = GetMaximumBusinessCustomerRegistered();
            var max = await Heidelpay.CreateCustomerAsync(customer);

            Assert.NotNull(max?.Id);

            var fetched = await Heidelpay.FetchCustomerAsync(max.Id);

            BusinessCustomerEquals(customer, max);
        }

        [Fact(Skip = "Customer cannot be deleted and this test fails.")]
        public async Task Delete_Customer()
        {
            var max = GetMaximumBusinessCustomerRegistered();
            var customer = await Heidelpay.CreateCustomerAsync(max);

            Assert.NotNull(customer?.Id);

            await Heidelpay.DeleteCustomerAsync(customer.Id);

            await Assert.ThrowsAsync<PaymentException>(() => Heidelpay.FetchCustomerAsync(customer.Id));
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
