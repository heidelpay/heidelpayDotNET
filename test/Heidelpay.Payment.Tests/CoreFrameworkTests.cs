using System;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Internal.Tests
{
    public class CoreFrameworkTests
    {
        [Fact]
        public async Task Test_Payment_Checks_IsNotEmpty()
        {
            // These tests exist in the reference Java code, aren't really required in .NET since it is base functionality
            // Initially everything is being ported over from Java, so this might get removed at some point
            Assert.False(CoreExtensions.IsNotEmpty(""));
            Assert.False(CoreExtensions.IsNotEmpty("   "));
            Assert.False(CoreExtensions.IsNotEmpty(null));
            Assert.True(CoreExtensions.IsNotEmpty("a"));
        }

        [Fact]
        public async Task Test_DateTime_Serializer()
        {
            Assert.True(CoreExtensions.TryParseDateTime("2018-09-13 22:47:35", out DateTime _));
            Assert.True(CoreExtensions.TryParseDateTime("2018-09-13", out DateTime _));
            Assert.False(CoreExtensions.TryParseDateTime("22:47:35 2018-09-13", out DateTime _));
            Assert.False(CoreExtensions.TryParseDateTime("2018.09.13 22:47:35", out DateTime _));
        }
    }
}
