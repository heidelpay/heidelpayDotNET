// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-25-2019
//
// Last Modified By : berghtho
// Last Modified On : 03-25-2019
// ***********************************************************************
// <copyright file="SDKInfo.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Reflection;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class SDKInfo.
    /// </summary>
    public static class SDKInfo
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public static string Version { get; } = ThisAssembly.AssemblyInformationalVersion;
    }
}
