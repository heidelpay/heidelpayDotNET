using Heidelpay.Payment.PaymentTypes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class RecurringTest : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Recurring_Card_Without_Customer()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var recurring = await heidelpay.RecurringAsync(card, TestReturnUri);

            Assert.Equal(Status.Pending, recurring.Status);

            var fetchedCard = await heidelpay.FetchPaymentTypeAsync<Card>(card.Id);
            Assert.False(fetchedCard.Recurring);
        }

        [Fact]
        public async Task Recurring_Card_With_CustomerId()
        {
            var heidelpay = Heidelpay;
            var customer = await heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var recurring = await heidelpay.RecurringAsync(card, TestReturnUri, customerId: customer.Id);

            Assert.Equal(Status.Pending, recurring.Status);
            Assert.NotNull(recurring.RedirectUrl);

            var fetchedCard = await heidelpay.FetchPaymentTypeAsync<Card>(card.Id);
            Assert.False(fetchedCard.Recurring);
        }
    }
}
