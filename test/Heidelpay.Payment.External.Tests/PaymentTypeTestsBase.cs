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
        public const decimal StandardChargedBasketResult = 490.05m;
        public const string DateOnlyFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

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
            return new ThreadSafeRandom()
                .Next(10000, 99999)
                .ToString();
        }

        protected Customer CreateFactoringOKCustomer(string customerId)
        {
            var customer = new Customer("Maximilian", "Mustermann");

            customer.Salutation = Salutation.Mr;
            customer.BirthDate = new DateTime(1980, 11, 22);
            customer.BillingAddress = CreateAddress("Maximilian Mustermann", "Hugo-Junkers-Str. 3", "Frankfurt am Main", "Frankfurt am Main", "60386", "DE");
            customer.ShippingAddress = CreateAddress("Maximilian Mustermann", "Hugo-Junkers-Str. 3", "Frankfurt am Main", "Frankfurt am Main", "60386", "DE");

            return customer;
        }

        protected Address CreateAddress(string name, string street, string city, string state, string zip, string country)
        {
            return new Address
            {
                Name = name,
                Street = street,
                City = city,
                State = state,
                Zip = zip,
                Country = country,
            };
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
            return new Customer("Max", "Musterperson") { BillingAddress = TestAddress };
        }

        protected Customer GetMinimumBusinessCustomerRegistered()
        {
            var cust = new Customer("Heidelpay GmbH");
            cust.BillingAddress = TestAddress;
            cust.CompanyInfo = CompanyInfo.BuildRegistered();
            cust.CompanyInfo.CommercialRegisterNumber = "HRB337681 MANNHEIM";
            return cust;
        }

        protected Customer GetMaximumBusinessCustomerRegistered()
        {
            var cust = GetMaximumCustomer(GetRandomId(), "Heidelpay GmbH");
            cust.BillingAddress = TestAddress;
            cust.CompanyInfo = CompanyInfo.BuildRegistered();
            cust.CompanyInfo.CommercialRegisterNumber = "HRB337681 MANNHEIM";
            cust.CompanyInfo.CommercialSector = CommercialSector.AIR_TRANSPORT;
            return cust;
        }


        protected Customer GetMaximumCustomerSameAddress(string customerId)
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

        protected Customer GetMaximumCustomer(string customerId, string companyName = null)
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
                CompanyName = companyName,
                ShippingAddress = new Address { Name = "Schubert", Street = "Vangerowstraße 18", City = "Heidelberg", Country = "BW", Zip = "69115", State = "DE" },
            };
        }

        protected Basket GetMaximumBasket(decimal? amount = 500.05m, decimal? discount = 10m)
        {
            var basket = new Basket
            {
                AmountTotalGross = amount.Value,
                AmountTotalDiscount = discount.Value,
                CurrencyCode = Currencies.EUR,
                Note = "Mystery Shopping",
                OrderId = GetRandomInvoiceId(),
            };
            basket.AddBasketItem(new BasketItem
            {
                BasketItemReferenceId = "Artikelnummer4711",
                AmountDiscount = decimal.One,
                AmountGross = amount.Value,
                AmountNet = amount.Value,
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
                CurrencyCode = Currencies.EUR,
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

        protected void AssertCancel(Cancel cancel, decimal? cancelAmount = 866.49m)
        {
            Assert.NotNull(cancel?.Id);
            Assert.NotNull(cancel.Processing.UniqueId);
            Assert.NotNull(cancel.Processing.ShortId);
            Assert.Equal(cancelAmount, cancel.Amount);
            Assert.Equal(Status.Success, cancel.Status);
        }

        protected void AssertCharge(Charge charge, decimal? chargeAmount = 866.49m, Status status = Status.Success, string currency = Currencies.EUR)
        {
            Assert.NotNull(charge?.Id);
            Assert.NotNull(charge.Processing.UniqueId);
            Assert.NotNull(charge.Processing.ShortId);
            Assert.Equal(currency, charge.Currency);
            Assert.Equal(chargeAmount, charge.Amount);
            Assert.Equal(status, charge.Status);
        }

        protected void AssertAuthorizationSimple(Authorization authorization, decimal? authAmount = 866.49m, Status status = Status.Success)
        {
            Assert.NotNull(authorization?.Id);
            Assert.Equal(authAmount, authorization.Amount);
            Assert.Equal(status, authorization.Status);
            Assert.NotNull(authorization.TypeId);
        }

        protected void AssertAuthorizationFull(Authorization authorization, decimal? authAmount = 866.49m)
        {
            AssertAuthorizationSimple(authorization, authAmount);

            Assert.NotNull(authorization.Processing.PdfLink);
            Assert.NotNull(authorization.Processing.ExternalOrderId);

            Assert.NotNull(authorization.PaymentId);
            Assert.NotNull(authorization.CustomerId);
            Assert.NotNull(authorization.BasketId);
        }

        protected void AssertShipment(Shipment shipment)
        {
            Assert.NotNull(shipment?.Id);
        }
    }

    public class ThreadSafeRandom
    {
        private static readonly Random _global = new Random();
        [ThreadStatic] private static Random _local;

        public int Next(int min, int max)
        {
            if (_local == null)
            {
                lock (_global)
                {
                    if (_local == null)
                    {
                        int seed = _global.Next(min, max);
                        _local = new Random(seed);
                    }
                }
            }

            return _local.Next(min, max);
        }
    }
}
