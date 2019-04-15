// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-15-2019
// ***********************************************************************
// <copyright file="RestClientConstants.cs" company="Heidelpay">
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

namespace Heidelpay.Payment.Communication
{
    /// <summary>
    /// Class RestClientConstants.
    /// </summary>
    public static class RestClientConstants
    {
        /// <summary>
        /// The user agent
        /// </summary>
        public const string USER_AGENT = "User-Agent";
        /// <summary>
        /// The authorization
        /// </summary>
        public const string AUTHORIZATION = "Authorization";
        /// <summary>
        /// The basic
        /// </summary>
        public const string BASIC = "Basic";
        /// <summary>
        /// The user agent prefix
        /// </summary>
        public const string USER_AGENT_PREFIX = "heidelpay-DOTNET-";
        /// <summary>
        /// The content type json
        /// </summary>
        public const string CONTENT_TYPE_JSON = "application/json; charset=UTF-8";
        /// <summary>
        /// The content type
        /// </summary>
        public const string CONTENT_TYPE = "Content-Type";
        /// <summary>
        /// The accept language
        /// </summary>
        public const string ACCEPT_LANGUAGE = "Accept-Language";
    }
}
