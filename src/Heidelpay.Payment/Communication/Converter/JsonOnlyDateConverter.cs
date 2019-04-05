// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 04-01-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-02-2019
// ***********************************************************************
// <copyright file="JsonOnlyDateConverter.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Newtonsoft.Json.Converters
{
    /// <summary>
    /// Class JsonOnlyDateConverter.
    /// Implements the <see cref="Newtonsoft.Json.Converters.IsoDateTimeConverter" />
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.Converters.IsoDateTimeConverter" />
    public class JsonOnlyDateConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonOnlyDateConverter"/> class.
        /// </summary>
        public JsonOnlyDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
