using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class CancelAfterAuthorizationTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Fetch_Authorization()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri });
            var fetched = await BuildHeidelpay().FetchAuthorizationAsync(auth.PaymentId);
            Assert.NotNull(fetched?.Id);
        }

        [Fact]
        public async Task Cancel_After_Authorization()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false });
            var fetched = await BuildHeidelpay().FetchAuthorizationAsync(auth.PaymentId);
            var cancel = await fetched.CancelAsync();

            Assert.NotNull(cancel?.Id);
        }

        [Fact]
        public async Task Partial_Cancel_Payment()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false });
            var fetched = await BuildHeidelpay().FetchPaymentAsync(auth.PaymentId);
            var cancel = await fetched.CancelAsync(amount: 1m);

            Assert.NotNull(cancel?.Id);
            Assert.Equal(1m, cancel.Amount);
        }

        [Fact]
        public async Task Partial_Cancel_After_Authorization()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false });
            var fetched = await BuildHeidelpay().FetchAuthorizationAsync(auth.PaymentId);
            var cancel = await fetched.CancelAsync(amount: 1m);

            Assert.NotNull(cancel?.Id);
            Assert.Equal(1m, cancel.Amount);
        }

        [Fact]
        public async Task Partial_Cancel_Heidelpay()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false });
            var fetched = await BuildHeidelpay().FetchAuthorizationAsync(auth.PaymentId);
            var cancel = await BuildHeidelpay().CancelAuthorizationAsync(fetched.PaymentId, 1m);

            Assert.NotNull(cancel?.Id);
            Assert.Equal(1m, cancel.Amount);
        }

        [Fact]
        public async Task Fetch_Cancel_Heidelpay()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card) { Amount = 10.01m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false });
            var cancel = await BuildHeidelpay().CancelAuthorizationAsync(auth.PaymentId);

            Assert.NotNull(cancel?.Id);
            Assert.Equal(10.01m, cancel.Amount);

            var fetched = BuildHeidelpay().FetchCancelAsync(auth.PaymentId, cancel.Id); 
        }

        [Fact]
        public async Task Cancel_After_Authorization_Is_List_Filled()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(PaymentTypeCard);
            var auth = await BuildHeidelpay().AuthorizeAsync(new Authorization(card) { Amount = 10m, Currency = "EUR", ReturnUrl = TestReturnUri, Card3ds = false });
            var fetched = await BuildHeidelpay().FetchAuthorizationAsync(auth.PaymentId);
            var cancel = await fetched.CancelAsync(1m);

            Assert.NotNull(cancel?.Id);
            Assert.Equal(1m, cancel.Amount);

            var cancel2 = await fetched.CancelAsync(1m);

            Assert.NotNull(cancel2?.Id);
            Assert.Equal(1m, cancel2.Amount);

            var check = await BuildHeidelpay().FetchAuthorizationAsync(auth.PaymentId);

            Assert.NotNull(check?.CancelList);
            Assert.Equal(2, check.CancelList.Count());
            Assert.Equal(2, check.Payment.Authorization.CancelList.Count());
        }
    }
}
