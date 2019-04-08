// ***********************************************************************
// Assembly         : Heidelpay.Payment
// Author           : berghtho
// Created          : 03-14-2019
//
// Last Modified By : berghtho
// Last Modified On : 03-14-2019
// ***********************************************************************
// <copyright file="UnsupportedPaymentTypeException.cs" company="Heidelpay">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Heidelpay.Payment
{
    /// <summary>
    /// Class UnsupportedPaymentTypeException. This class cannot be inherited.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class UnsupportedPaymentTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedPaymentTypeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnsupportedPaymentTypeException(string message)
            : base(message)
        {

        }
    }
}
