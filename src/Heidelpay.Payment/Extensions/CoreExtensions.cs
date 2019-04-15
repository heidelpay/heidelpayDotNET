// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="CoreExtensions.cs" company="Heidelpay">
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

using System.Globalization;
using System.Text;

namespace System
{
    /// <summary>
    /// Class CoreExtensions.
    /// </summary>
    internal static class CoreExtensions
    {
        /// <summary>
        /// Encodes to base64.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>System.String.</returns>
        public static string EncodeToBase64(this string plainText)
        {
            Check.ThrowIfNull(plainText, nameof(plainText));

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decodes from base64.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns>System.String.</returns>
        public static string DecodeFromBase64(this string base64EncodedData)
        {
            Check.ThrowIfNull(base64EncodedData, nameof(base64EncodedData));

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// The allowed date time formats
        /// </summary>
        static readonly string[] AllowedDateTimeFormats = new[] 
        {
            DateTimeFormat,
            DateOnlyFormat,
        };

        /// <summary>
        /// The date only format
        /// </summary>
        public const string DateOnlyFormat = "yyyy-MM-dd";
        /// <summary>
        /// The date time format
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Tries to parse date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryParseDateTime(this string value, out DateTime result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default; 
                return false;
            }

            return DateTime.TryParseExact(value, AllowedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        /// <summary>
        /// Determines whether [the specified value] [is not empty].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is not empty] [the specified value]; otherwise, <c>false</c>.</returns>
        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Ensures the trailing slash.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string EnsureTrailingSlash(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.EndsWith("/"))
                return value;

            return value + "/";
        }
    }
}
