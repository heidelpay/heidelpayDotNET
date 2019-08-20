﻿// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="IProvide3DS.cs" company="heidelpay GmbH, tieto Austria GmbH">
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

namespace Heidelpay.Payment.Interfaces
{
    /// <summary>
    /// Interface IProvide3DS
    /// </summary>
    internal interface IProvide3DS
    {
        /// <summary>
        /// Gets a value indicating whether [3ds] is set.
        /// </summary>
        /// <value><c>null</c> if [3ds] contains no value, <c>true</c> if [3ds]; otherwise, <c>false</c>.</value>
        bool? ThreeDs { get; }
    }
}
