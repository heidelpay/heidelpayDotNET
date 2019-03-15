using System;
using System.Reflection;

namespace Heidelpay.Payment
{
    public static class SDKInfo
    {
        public static string Version { get; }

        static SDKInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var attr = Attribute
                .GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute))
                    as AssemblyInformationalVersionAttribute;

            Version = attr?.InformationalVersion;
        }
    }
}
