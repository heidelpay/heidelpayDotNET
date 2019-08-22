// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="SDKInfo.cs" company="Heidelpay">
//     Copyright (c) 2019 Heidelpay GmbH. All rights reserved.
// </copyright>
// ***********************************************************************
// Licensed under the Apache License, Version 2.0 (the “License”);
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an “AS IS” BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ***********************************************************************

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
