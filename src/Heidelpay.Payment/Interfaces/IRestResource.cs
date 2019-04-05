// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 04-01-2019
// ***********************************************************************
// <copyright file="IRestResource.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Interface IRestResource
    /// </summary>
    public interface IRestResource 
    {
        /// <summary>
        /// Gets the type URL.
        /// </summary>
        /// <value>The type URL.</value>
        string TypeUrl { get; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        string Id { get; set; }
    }
}
