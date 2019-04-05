using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public class ShipmentTests : PaymentTypeTestsBase
    {
        [Fact(Skip = "Merchant is having problem with insurance provider")]
        public async Task Authorize_With_Shipment()
        {
            await Task.CompletedTask;
        }
    }
}
