using Heidelpay.Payment.PaymentTypes;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business.PaymentTypes
{
    public class ApplepayTests : PaymentTypeTestsBase
    {
        private Action<Applepay> ConfigurePaymentType { get; } = new Action<Applepay>(x =>
        {
            x.Version = "EC_v1";
            x.Data = "eQnEAqrhdti41auDwH9GZD7xMWOG9fNAenXbRp2HptwViAgLDWcVlvj1i1y9JmYOoU4oFpBQ/ACceYrIsDvz+lC55eQugnkxwAG6JTKotBgI0b1ERdqqYSn1CGfvzpZWQW4C0yVBZZXfzVq+iANfT8Kl7sma5orHoBsNeoqmd+VHnAlK0ZAfiWoci3h7xMK/o+iOftglQly9x91HqrvCB63KVDausffJLoqbmJYh6HJEJvrOBju9FWMJzGVI97xSEK/AoOYMRrvUSJpLvW2H3WqxZJ2wUcNNRjlHuzhEONuz20eD/ac4lDmNlaxt9HpcAR1r/+iBwkDlbTk+IivjZ1krh9Aaf+yZps8KYef14eaW97CVmzp9NIcmG4J9dDGbxhbOR4h4gTdmmq4NgiFfNratUtEcEXP8cuIAvraDFQ==";
            x.Signature = "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";
            x.Header = new ApplepayHeader
            {
                EphemeralPublicKey = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEC0enUaVUenpYt2r3HuMJ98NjBTBuZBCBMB+H3mFtfuRh7NEdxe+dNtPEIZJ2fsofgLoBUOCksj087saeUZk2bw==",
                PublicKeyHash = "M2yzlpBsH3GwH5jTV9GgKC7bAUdeIOIfjwQhoKjg5+s=",
                TransactionId = "d8016e60711ce879314c449072ad6bd2cc575027394ffb353584e8eb55cf1e32",
            };
        });


        [Fact]
        public async Task Create_PaymentType_Via_Config()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Create_PaymentType_Via_Instance()
        {
            var instance = new Applepay(Heidelpay);
            ConfigurePaymentType(instance);
            var result = await Heidelpay.CreatePaymentTypeAsync(instance);
            Assert.NotNull(result?.Id);
        }

        [Fact]
        public async Task Fetch_PaymentType()
        {
            var createdTypeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);

            Assert.NotNull(createdTypeInstance?.Id);

            var fetched = await Heidelpay.FetchPaymentTypeAsync<Applepay>(createdTypeInstance.Id);

            Assert.NotNull(fetched?.Id);
        }

        [Fact]
        public async Task Authorize_PaymentType_object()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            var auth = await Heidelpay.AuthorizeAsync(decimal.One, Currencies.EUR, result, ShopReturnUri);
            Assert.NotNull(result?.Id);

            AssertAuthorizationSimple(auth, decimal.One);
        }

        [Fact]
        public async Task Authorize_PaymentType_typeId()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            var auth = await Heidelpay.AuthorizeAsync(decimal.One, Currencies.EUR, result.Id, ShopReturnUri);
            Assert.NotNull(result?.Id);

            AssertAuthorizationSimple(auth, decimal.One);
        }

        [Fact]
        public async Task Charge_PaymentType()
        {
            var typeInstance = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
            var charge = await typeInstance.ChargeAsync(decimal.One, Currencies.EUR, ShopReturnUri);

            Assert.Equal(typeInstance.Id, charge.TypeId);

            AssertCharge(charge, decimal.One);
        }
    }
}
