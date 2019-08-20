using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Heidelpay.Payment.External.Tests.Business
{
    public abstract class PaymentTypeTestsBase
    {
        protected IHeidelpay Heidelpay
        {
            get
            {
                return BuildHeidelpay();
            }
        }

        protected IHeidelpay BuildHeidelpay(string privateKey = null)
        {
            var services = new ServiceCollection();

            services.AddHttpClient();
            services.AddLogging();

            services.AddHeidelpay(opt =>
            {
                opt.ApiEndpoint = new Uri("https://api.heidelpay.com");
                opt.ApiVersion = "v1";
                opt.ApiKey = privateKey ?? "s-priv-2a102ZMq3gV4I3zJ888J7RR6u75oqK3n";
            });

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<IHeidelpay>();
        }



        protected static string GetRandomId()
        {
            return Guid.NewGuid().ToString("B")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("-", "")
                .ToUpper();
        }

        protected static string GetRandomInvoiceId()
        {
            return GetRandomId().Substring(0, 5);
        }

        protected async Task<Customer> CreateMaximumCustomer(HeidelpayClient heidelpay)
        {
            return await heidelpay.CreateCustomerAsync(GetMaximumCustomer(GetRandomId()));
        }

        protected async Task<Customer> CreateMaximumCustomerSameAddress(HeidelpayClient heidelpay)
        {
            return await heidelpay.CreateCustomerAsync(GetMaximumCustomerSameAddress(GetRandomId()));
        }

        protected Customer GetMinimumCustomer()
        {
            return new Customer( "Max", "Musterperson" );
        }

        protected Customer GetMaximumCustomerSameAddress(String customerId)
        {
            TryParseDateTime("1974-03-10", out DateTime dt);

            return new Customer("Max", "Musterperson")
            {
                CustomerId = customerId,
                Salutation = Salutation.Mr,
                Email = "info@heidelpay.com",
                Mobile = "+43676123456",
                BirthDate = dt,
                BillingAddress = TestAddress,
                ShippingAddress = TestAddress,
            };
        }
        protected Customer GetMaximumCustomer(String customerId)
        {
            TryParseDateTime("1974-03-10", out DateTime dt);

            return new Customer("Max", "Musterperson")
            {
                CustomerId = customerId,
                Salutation = Salutation.Mr,
                Email = "info@heidelpay.com",
                Mobile = "+43676123456",
                BirthDate = dt,
                BillingAddress = TestAddress,
                ShippingAddress = new Address { Name = "Schubert", Street = "Vangerowstraße 18", City = "Heidelberg", Country = "BW", Zip = "69115", State = "DE" },
            };
        }

        protected Basket GetMaximumBasket()
        {
            var basket = new Basket
            {
                AmountTotalGross = 500.05m,
                AmountTotalDiscount = 10m,
                CurrencyCode = "EUR",
                Note = "Mystery Shopping",
                OrderId = GetRandomInvoiceId(),
            };
            basket.AddBasketItem(new BasketItem
            {
                BasketItemReferenceId = "Artikelnummer4711",
                AmountDiscount = decimal.One,
                AmountGross = 500.5m,
                AmountNet = 420.1m,
                AmountPerUnit = 100.1m,
                AmountVat = 80.4m,
                Quantity = 5,
                Title = "Apple iPhone",
                SubTitle = "Yet another Phone",
                Unit = "Pc.",
                Vat = 19,
                ImageUrl = new Uri("https://www.heidelpay.com/typo3conf/ext/heidelpay_site/Resources/Public/Images/Heidelpay-Logo_weiss.svg")
            });
            return basket;
        }

        protected Basket GetMinimumBasket()
        {
            var basket = new Basket
            {
                AmountTotalGross = 500.05m,
                CurrencyCode = "EUR",
                OrderId = GetRandomInvoiceId(),
            };
            basket.AddBasketItem(new BasketItem
            {
                BasketItemReferenceId = "Artikelnummer4711",
                AmountNet = 420.1m,
                AmountPerUnit = 100.1m,
                Quantity = 5,
                Title = "Apple iPhone",
            });
            return basket;
        }

        protected MetaData TestMetaData { get; } = new MetaData { ["invoice-nr"] = "Rg-2018-11-1", ["shop-id"] = "4711", ["delivery-date"] = "24.12.2018", ["reason"] = "X-mas present" };
        protected MetaData TestMetaDataSorted { get; } = new MetaData(true) { ["invoice-nr"] = "Rg-2018-11-1", ["shop-id"] = "4711", ["delivery-date"] = "24.12.2018", ["reason"] = "X-mas present" };
        protected Address TestAddress { get; } = new Address { Name = "Mozart", Street = "Grüngasse 16", City = "Vienna", State = "Vienna", Zip = "1010", Country = "AT" };
        protected Action<Card> PaymentTypeCard { get; } = new Action<Card>(x => 
        {
            x.Number = "4444333322221111";
            x.ExpiryDate = "03/20";
            x.CVC = "123"; 
        });

        protected Action<Card> PaymentTypeCardNo3DS { get; } = new Action<Card>(x =>
        {
            x.Number = "4444333322221111";
            x.ExpiryDate = "03/20";
            x.CVC = "123";
            x.ThreeDs = false;
        });

        protected Action<Card> PaymentTypeCard3DS { get; } = new Action<Card>(x =>
        {
            x.Number = "4444333322221111";
            x.ExpiryDate = "03/20";
            x.CVC = "123";
            x.ThreeDs = true;
        });


        static readonly string[] AllowedDateTimeFormats = new[]
        {
            DateTimeFormat,
            DateOnlyFormat,
        };

        public const string DateOnlyFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static bool TryParseDateTime(string value, out DateTime result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }

            return DateTime.TryParseExact(value, AllowedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        protected Uri TestReturnUri { get; } = new Uri("https://www.google.at");
        protected Uri ShopReturnUri { get; } = new Uri("https://www.meinShop.de");

        protected void AssertEquals(Customer expected, Customer actual)
        {
            Assert.Equal(expected.Firstname ?? "", actual.Firstname);
            Assert.Equal(expected.Lastname ?? "", actual.Lastname);
            Assert.Equal(expected.CustomerId ?? "", actual.CustomerId);
            Assert.Equal(expected.BirthDate, actual.BirthDate);
            Assert.Equal(expected.Email ?? "", actual.Email);
            Assert.Equal(expected.Mobile ?? "", actual.Mobile);
            Assert.Equal(expected.Phone ?? "", actual.Phone);
            AssertEquals(expected.BillingAddress, actual.BillingAddress);
            AssertEquals(expected.ShippingAddress, actual.ShippingAddress);
        }

        protected void AssertEquals(Address expected, Address actual)
        {
            if (expected == null)
            {
                return;
            }
            Assert.Equal(expected.City ?? "", actual.City);
            Assert.Equal(expected.Country ?? "", actual.Country);
            Assert.Equal(expected.Name ?? "", actual.Name);
            Assert.Equal(expected.State ?? "", actual.State);
            Assert.Equal(expected.Street ?? "", actual.Street);
            Assert.Equal(expected.Zip ?? "", actual.Zip);
        }

        protected void AssertEqual(Basket expected, Basket actual)
        {
            Assert.Equal(expected.AmountTotalGross, actual.AmountTotalGross);
            Assert.Equal(expected.AmountTotalDiscount, actual.AmountTotalDiscount);
            Assert.Equal(expected.CurrencyCode, actual.CurrencyCode);
            Assert.Equal(expected.Note, actual.Note);
            Assert.Equal(expected.OrderId, actual.OrderId);
            Assert.Single(actual.BasketItems);

            AssertEqual(expected.BasketItems.First(), actual.BasketItems.First());
        }

        protected void AssertEqual(BasketItem expected, BasketItem actual)
        {
            Assert.Equal(expected.AmountDiscount, actual.AmountDiscount);
            Assert.Equal(expected.AmountGross, actual.AmountGross);
            Assert.Equal(expected.AmountNet, actual.AmountNet);
            Assert.Equal(expected.AmountPerUnit, actual.AmountPerUnit);
            Assert.Equal(expected.AmountVat, actual.AmountVat);
            Assert.Equal(expected.BasketItemReferenceId, actual.BasketItemReferenceId);
            Assert.Equal(expected.Quantity, actual.Quantity);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Unit, actual.Unit);
            Assert.Equal(expected.Vat, actual.Vat);
        }

        protected void AssertEquals(Charge expected, Charge actual)
        {
            Assert.Equal(expected.Amount, actual.Amount);
            Assert.Equal(expected.Currency, actual.Currency);
            Assert.Equal(expected.CustomerId, actual.CustomerId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.PaymentId, actual.PaymentId);
            Assert.Equal(expected.ReturnUrl, actual.ReturnUrl);
            Assert.Equal(expected.RiskId, actual.RiskId);
            Assert.Equal(expected.TypeId, actual.TypeId);
            AssertEquals(expected.Processing, actual.Processing);
        }

        protected void AssertEquals(Processing expected, Processing actual)
        {
            Assert.Equal(expected.ShortId, actual.ShortId);
            Assert.Equal(expected.UniqueId, actual.UniqueId);
        }

        protected void AssertEquals(Cancel expected, Cancel actual)
        {
            Assert.Equal(expected.Amount, actual.Amount);
            Assert.Equal(expected.Currency, actual.Currency);
            Assert.Equal(expected.CustomerId, actual.CustomerId);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.PaymentId, actual.PaymentId);
            Assert.Equal(expected.ReturnUrl, actual.ReturnUrl);
            Assert.Equal(expected.RiskId, actual.RiskId);
            Assert.Equal(expected.TypeId, actual.TypeId);
            AssertEquals(expected.Processing, actual.Processing);
        }
    }
}
