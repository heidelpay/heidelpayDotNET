using Heidelpay.Payment.PaymentTypes;
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

        [Fact]
        public async Task Recurring_Card_With_Customer_And_Metadata()
        {
            var heidelpay = Heidelpay;
            var customer = await heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
            var metadata = await heidelpay.CreateMetadataAsync(TestMetaData);
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCard);
            var recurring = await heidelpay.RecurringAsync(card, TestReturnUri, customerId: customer.Id, metadataId: metadata.Id);

            Assert.Equal(Status.Pending, recurring.Status);
            Assert.NotNull(recurring.RedirectUrl);

            var fetchedCard = await heidelpay.FetchPaymentTypeAsync<Card>(card.Id);
            Assert.False(fetchedCard.Recurring);
        }

        [Fact]
        public async Task Recurring_Card_During_Charge()
        {
            var heidelpay = Heidelpay;
            var card = await heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            Assert.False(card.Recurring);

            var charge = await card.ChargeAsync(decimal.One, Currencies.EUR, TestReturnUri);
            Assert.Null(charge.RedirectUrl);

            var fetchedCard = await heidelpay.FetchPaymentTypeAsync<Card>(card.Id);
            Assert.True(fetchedCard.Recurring);
        }

        [Fact]
        public async Task Recurring_SepaDirectDebit_During_Charge()
        {
            var heidelpay = Heidelpay;
            var result = await Heidelpay.CreatePaymentTypeAsync<SepaDirectDebit>(x => x.Iban = "DE89370400440532013000");
            Assert.False(result.Recurring);

            var charge = await result.ChargeAsync(decimal.One, Currencies.EUR, TestReturnUri);
            Assert.Null(charge.RedirectUrl);

            var fetchedCard = await heidelpay.FetchPaymentTypeAsync<SepaDirectDebit>(result.Id);
            Assert.True(fetchedCard.Recurring);
        }
    }
}
