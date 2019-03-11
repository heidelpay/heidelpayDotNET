using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Heidelpay.Payment
{
    public class SDKOptions
    {
        public Uri ApiEndpoint { get; set; }

        public static string SDKVersion { get; }

        static SDKOptions()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var attr = Attribute
                .GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute))
                    as AssemblyInformationalVersionAttribute;

            SDKVersion = attr?.InformationalVersion;
        }
    }
}
