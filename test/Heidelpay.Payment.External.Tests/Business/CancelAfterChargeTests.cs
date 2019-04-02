using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class CancelAfterChargeTests : PaymentTypeTestsBase
    {
        [Fact(Skip = "It's not ready yet")]
        public async Task EmptyTest()
        {
            await Task.CompletedTask;
        }
    }
}
