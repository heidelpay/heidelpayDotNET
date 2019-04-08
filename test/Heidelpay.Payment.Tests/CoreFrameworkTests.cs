using System;
using Xunit;

namespace Heidelpay.Payment.Internal.Tests
{
    public class CoreFrameworkTests
    {
        [Fact]
        public void Test_Payment_Checks_IsNotEmpty()
        {
            // These tests exist in the reference Java code, aren't really required in .NET since it is base functionality
            // Initially everything is being ported over from Java, so this might get removed at some point
            Assert.False(CoreExtensions.IsNotEmpty(""));
            Assert.False(CoreExtensions.IsNotEmpty("   "));
            Assert.False(CoreExtensions.IsNotEmpty(null));
            Assert.True(CoreExtensions.IsNotEmpty("a"));
        }

        [Fact]
        public void Test_DateTime_Serializer()
        {
            Assert.True(CoreExtensions.TryParseDateTime("2018-09-13 22:47:35", out DateTime _));
            Assert.True(CoreExtensions.TryParseDateTime("2018-09-13", out DateTime _));
            Assert.False(CoreExtensions.TryParseDateTime("22:47:35 2018-09-13", out DateTime _));
            Assert.False(CoreExtensions.TryParseDateTime("2018.09.13 22:47:35", out DateTime _));
        }

        [Fact]
        public void Checks_Test()
        {
            Assert.Throws<ArgumentNullException>(() => Check.ThrowIfNull(null, "must throw"));
            Assert.Throws<ArgumentNullException>(() => Check.ThrowIfNullOrEmpty(null, "must throw"));
            Assert.Throws<ArgumentNullException>(() => Check.ThrowIfNullOrEmpty("", "must throw"));
            Assert.Throws<ArgumentNullException>(() => Check.ThrowIfNullOrEmpty(" ", "must throw"));
            Assert.Throws<PaymentException>(() => Check.ThrowIfTrue(true, "must throw"));

            Check.ThrowIfNull(new object(), "must not throw");
            Check.ThrowIfNullOrEmpty("a", "must not throw");
            Check.ThrowIfTrue(false, "must not throw");
        }

        [Fact]
        public void Encode_Decode_Test()
        {
            var encoded = CoreExtensions.EncodeToBase64("Test");
            var decoded = CoreExtensions.DecodeFromBase64(encoded);

            Assert.Equal("Test", decoded);
        }

        [Fact]
        public void Trailing_Slash_Test()
        {
            Assert.Equal("", CoreExtensions.EnsureTrailingSlash(""));
            Assert.Null(CoreExtensions.EnsureTrailingSlash(null));

            Assert.Equal("https://www.google.at/", CoreExtensions.EnsureTrailingSlash("https://www.google.at/"));
            Assert.Equal("https://www.google.at/", CoreExtensions.EnsureTrailingSlash("https://www.google.at"));
        }
    }
}
