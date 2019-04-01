using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class CardTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await BuildHeidelpay().CreatePaymentTypeAsync(TestCard);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Authorize_PaymentType()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(TestCard);
            var authorization = await card.AuthorizeAsync(decimal.One, "EUR", ShopReturnUri);
            Assert.NotNull(authorization?.Id);
        }

        [Fact]
        public async Task Authorize_And_Payment_PaymentType()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(TestCard);
            var authorization = await card.AuthorizeAsync(decimal.One, "EUR", ShopReturnUri);
            var payment = authorization.Payment;
            Assert.NotNull(authorization?.Id);
            Assert.NotNull(payment?.Id);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var card = await BuildHeidelpay().CreatePaymentTypeAsync(TestCard);
            var charge = await card.ChargeAsync(decimal.One, "EUR", ShopReturnUri);
            Assert.NotNull(charge?.Id);
            Assert.Equal(decimal.One, charge.Amount);
            Assert.Equal("EUR", charge.Currency);
            Assert.Equal(card.Id, charge.TypeId);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var createdCard = await BuildHeidelpay().CreatePaymentTypeAsync(TestCard);

            Assert.NotNull(createdCard?.Id);
            Assert.NotNull(createdCard.CVC);
            Assert.Equal("03/2020", createdCard.ExpiryDate);
            Assert.Equal("444433******1111", createdCard.Number);

            var fetchedCard = await BuildHeidelpay().FetchPaymentTypeAsync<Card>(createdCard.Id);

            Assert.NotNull(fetchedCard?.Id);
            Assert.NotNull(fetchedCard.CVC);
            Assert.Equal("03/2020", fetchedCard.ExpiryDate);
            Assert.Equal("444433******1111", fetchedCard.Number);
        }

        private static Card TestCard { get; } = new Card { Number = "4444333322221111", ExpiryDate = "03/20", CVC = "123" };
    }
}
