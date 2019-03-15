using Heidelpay.Payment.Communication;
using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.Tests.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.Tests
{
    public class CoreFrameworkTests
    {
        [Fact]
        public async Task Test_Payment_Checks_IsNotEmpty()
        {
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