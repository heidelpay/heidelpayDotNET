using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class MetadataTests : PaymentTypeTestsBase
    {
        [Fact]
        public async Task Create_Fetch_Metadata()
        {
            var metadata = await Heidelpay.CreateMetadataAsync(TestMetaData);
            var fetched = await Heidelpay.FetchMetaDataAsync(metadata.Id);

            Assert.NotNull(metadata?.Id);
            Assert.NotNull(fetched?.Id);

            TestMetaData.MetadataMap.Keys.ToList().ForEach(x => Assert.Equal(TestMetaData[x], metadata[x]));
            metadata.MetadataMap.Keys.ToList().ForEach(x => Assert.Equal(metadata[x], fetched[x]));
        }

        [Fact]
        public async Task Sorted_Metadata()
        {
            var metadata = await Heidelpay.CreateMetadataAsync(TestMetaDataSorted);
            Assert.Equal("delivery-date", metadata.MetadataMap.First().Key);

            var fetched = await Heidelpay.FetchMetaDataAsync(metadata.Id);

            Assert.NotNull(metadata?.Id);
            Assert.NotNull(fetched?.Id);

            Assert.Equal("delivery-date", fetched.MetadataMap.First().Key);

            TestMetaData.MetadataMap.Keys.ToList().ForEach(x => Assert.Equal(TestMetaDataSorted[x], metadata[x]));
            metadata.MetadataMap.Keys.ToList().ForEach(x => Assert.Equal(metadata[x], fetched[x]));
        }

        [Fact]
        public async Task Authorization_With_Metadata()
        {
            var card = await Heidelpay.CreatePaymentTypeAsync(PaymentTypeCardNo3DS);
            var metadata = await Heidelpay.CreateMetadataAsync(TestMetaDataSorted);
            var auth = await Heidelpay.AuthorizeAsync(new Authorization(card)
            {
                Amount = 1m,
                Currency = Currencies.EUR,
                ReturnUrl = TestReturnUri,
                MetadataId = metadata.Id
            });
            var payment = await Heidelpay.FetchPaymentAsync(auth.PaymentId);

            Assert.NotNull(payment?.Id);
            Assert.NotNull(payment?.Authorization?.Id);

            Assert.Equal(metadata.Id, payment.MetadataId);
            Assert.Equal(metadata.Id, payment.Authorization.MetadataId);
        }
    }
}
