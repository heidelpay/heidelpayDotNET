// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 04-04-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-04-2019
// ***********************************************************************
// <copyright file="MetaDataConverter.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Communication.Converter
{
    /// <summary>
    /// Class MetaDataConverter. This class cannot be inherited.
    /// Implements the <see cref="Newtonsoft.Json.JsonConverter{Heidelpay.Payment.MetaData}" />
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter{Heidelpay.Payment.MetaData}" />
    internal sealed class MetaDataConverter : JsonConverter<MetaData>
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.</value>
        public override bool CanRead => true;
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
        /// </summary>
        /// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.</value>
        public override bool CanWrite => true;

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
        /// <param name="hasExistingValue">The existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override MetaData ReadJson(JsonReader reader, Type objectType, MetaData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new MetaData();
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, MetaData value, JsonSerializer serializer)
        {
            var serialized = JsonConvert.SerializeObject(value.MetadataMap);
            writer.WriteValue(JToken.Parse(serialized).ToString());
        }
    }
}
