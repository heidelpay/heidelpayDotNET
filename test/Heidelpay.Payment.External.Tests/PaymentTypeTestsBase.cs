using Heidelpay.Payment.Extensions;
using Heidelpay.Payment.Interfaces;
using Heidelpay.Payment.PaymentTypes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Heidelpay.Payment.External.Tests.Business
{
    public abstract class PaymentTypeTestsBase
    {
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
                AmountTotal = 500.05m,
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
                Unit = "Pc.",
                Vat = 19,
            });
            return basket;
        }

        protected Basket GetMinimumBasket()
        {
            var basket = new Basket
            {
                AmountTotal = 500.05m,
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
    }
}
