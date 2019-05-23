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
            x.Data = "CDNa1nRTdo4G7efZwAnmWFXHe8AddpxKPQtSVUl/7RBweAeJkqFD49rr4IxeeWfgNsbTEabKaUEkGxut9Rr8vJxNJ0OVDuZRQLueJFFFwTAxBIwqRCxGWqOEdP7WfPGoYibOG43r2kj0MjMDtkD7tVt+wZwLQeaLSprXJzvHVphtuZz/NH0Bl7U2TWy4wB3qvSbUSqqqPsF84sOwCKTvLYbN+yEKOT5dLcSKOiY9v3XasaqjEXLSn5FjHW49nFrg4W2M57LD7LlhHd15ihPBxoTBZBaA37N/23APUdPyv25qPy1QojUehYHGJAmEV0bKIf4kY/uBcGNMbPtmYTveq5MJVrEXcQFll1EOR3daQEi+jAH84ZYBvdpANF6KXas6E/Tf36+hXKDfA2p1";
            x.Signature = "MIAGCSqGSIb3DQEHAqCAMIACAQExDzANBglghkgBZQMEAgEFADCABgkqhkiG9w0BBwEAAKCAMIID5jCCA4ugAwIBAgIIaGD2mdnMpw8wCgYIKoZIzj0EAwIwejEuMCwGA1UEAwwlQXBwbGUgQXBwbGljYXRpb24gSW50ZWdyYXRpb24gQ0EgLSBHMzEmMCQGA1UECwwdQXBwbGUgQ2VydGlmaWNhdGlvbiBBdXRob3JpdHkxEzARBgNVBAoMCkFwcGxlIEluYy4xCzAJBgNVBAYTAlVTMB4XDTE2MDYwMzE4MTY0MFoXDTIxMDYwMjE4MTY0MFowYjEoMCYGA1UEAwwfZWNjLXNtcC1icm9rZXItc2lnbl9VQzQtU0FOREJPWDEUMBIGA1UECwwLaU9TIFN5c3RlbXMxEzARBgNVBAoMCkFwcGxlIEluYy4xCzAJBgNVBAYTAlVTMFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEgjD9q8Oc914gLFDZm0US5jfiqQHdbLPgsc1LUmeY";
            x.Header.EphemeralPublicKey = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEM/wYg/LmVp2e+TqXsUAIFlzK02Rwm9PAWiu6d3z+iQf8oLvqKUzf1OpjsJeGZfYriTIFe4H9EP6QWxMjoyIs5w==";
            x.Header.PublicKeyHash = "M2yzlpBsH3GwH5jTV9GgKC7bAUdeIOIfjwQhoKjg5+s=";
            x.Header.TransactionId = "d518ad5c087011e44149b4e74c6a7021ab24cf3d01887efde7694f6a04bda238";
        });

        [Fact]
        public async Task Create_PaymentType()
        {
            var result = await Heidelpay.CreatePaymentTypeAsync(ConfigurePaymentType);
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
    }
}
