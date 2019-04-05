using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class CustomerTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_Minimum_Customer()
        {
            var min = GetMinimumCustomer();
            var customer = await Heidelpay.CreateCustomerAsync(min);

            Assert.NotNull(customer?.Id);

            AssertEquals(min, customer);
        }

        [Fact]
        public async Task Create_Maximum_Customer()
        {
            var max = GetMaximumCustomer(GetRandomId());
            var customer = await Heidelpay.CreateCustomerAsync(max);

            Assert.NotNull(customer?.Id);

            AssertEquals(max, customer);
        }

        [Fact]
        public async Task Fetch_Minimum_Customer()
        {
            var min = GetMinimumCustomer();
            var customer = await Heidelpay.CreateCustomerAsync(min);

            Assert.NotNull(customer?.Id);

            var fetched = await Heidelpay.FetchCustomerAsync(customer.Id);

            AssertEquals(customer, fetched);
        }

        [Fact]
        public async Task Fetch_Maximum_Customer()
        {
            var max = GetMaximumCustomer(GetRandomId());
            var customer = await Heidelpay.CreateCustomerAsync(max);

            Assert.NotNull(customer?.Id);

            var fetched = await Heidelpay.FetchCustomerAsync(customer.Id);

            AssertEquals(customer, fetched);
        }

        [Fact]
        public async Task Update_Maximum_Customer()
        {
            var max = GetMaximumCustomer(GetRandomId());
            var customer = await Heidelpay.CreateCustomerAsync(max);

            Assert.NotNull(customer?.Id);

            customer.Firstname = "Mads";

            var fetched = await Heidelpay.UpdateCustomerAsync(customer);

            AssertEquals(customer, fetched);
        }

        [Fact(Skip="Customer cannot be deleted and this test fails.")]
        public async Task Delete_Customer()
        {
            var max = GetMaximumCustomer(GetRandomId());
            var customer = await Heidelpay.CreateCustomerAsync(max);

            Assert.NotNull(customer?.Id);

            await Heidelpay.DeleteCustomerAsync(customer.Id);

            await Assert.ThrowsAsync<PaymentException>(() => Heidelpay.FetchCustomerAsync(customer.Id));
        }
    }
}
