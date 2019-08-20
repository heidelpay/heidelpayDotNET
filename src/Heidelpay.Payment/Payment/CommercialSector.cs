// ***********************************************************************
// Assembly         : Heidelpay.Payment
// ***********************************************************************
// <copyright file="Payment.cs" company="Heidelpay">
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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommercialSector
    {
        /// <summary />
        OTHER,
        /// <summary />
        WHOLESALE_TRADE_EXCEPT_VEHICLE_TRADE,
        /// <summary />
        RETAIL_TRADE_EXCEPT_VEHICLE_TRADE,
        /// <summary />
        WATER_TRANSPORT,
        /// <summary />
        AIR_TRANSPORT,
        /// <summary />
        WAREHOUSING_AND_SUPPORT_ACTIVITES_FOR_TRANSPORTATION,
        /// <summary />
        POSTAL_AND_COURIER_ACTIVITIES,
        /// <summary />
        ACCOMMODATION,
        /// <summary />
        FOOD_AND_BEVERAGE_SERVICE_ACTIVITIES,
        /// <summary />
        MOTION_PICTURE_PRODUCTION_AND_SIMILAR_ACTIVITIES,
        /// <summary />
        TELECOMMUNICATIONS,
        /// <summary />
        COMPUTER_PROGRAMMING_CONSULTANCY_AND_RELATED_ACTIVITIES,
        /// <summary />
        INFORMATION_SERVICE_ACTIVITIES,
        /// <summary />
        RENTAL_AND_LEASING_ACTIVITIES,
        /// <summary />
        TRAVEL_AGENCY_AND_RELATED_ACTIVITIES,
        /// <summary />
        SERVICES_TO_BUILDINGS_AND_LANDSCAPE_ACTIVITIES,
        /// <summary />
        LIBRARIES_AND_SIMILAR_CULTURAL_ACTIVITIES,
        /// <summary />
        SPORTS_ACTIVITIES_AND_AMUSEMENT_AND_RECREATION_ACTIVITIES,
        /// <summary />
        OTHER_PERSONAL_SERVICE_ACTIVITIES,
        /// <summary />
        NON_RESIDENTIAL_REAL_ESTATE_ACTIVITIES,
        /// <summary />
        MANAGEMENT_CONSULTANCY_ACTIVITIES,
        /// <summary />
        ELECTRICITY_GAS_AND_STEAM_SUPPLY,
        /// <summary />
        WATER_COLLECTION_TREATMENT_AND_SUPPLY,
        /// <summary />
        SEWERAGE,
        /// <summary />
        MANUFACTURE_OF_FOOD_PRODUCTS,
        /// <summary />
        MANUFACTURE_OF_BEVERAGES,
        /// <summary />
        MANUFACTURE_OF_TEXTILES,
        /// <summary />
        OTHERS_COMMERCIAL_SECTORS,
        /// <summary />
        MANUFACTURE_OF_WEARING_APPAREL,
        /// <summary />
        MANUFACTURE_OF_LEATHER_AND_RELATED_PRODUCTS,
        /// <summary />
        MANUFACTURE_OF_PHARMACEUTICAL_PRODUCTS,
        /// <summary />
        REPAIR_AND_INSTALLATION_OF_MACHINERY_AND_EQUIPMENT,
        /// <summary />
        TRADE_AND_REPAIR_OF_MOTOR_VEHICLES,
        /// <summary />
        PUBLISHING_ACTIVITIES,
        /// <summary />
        REPAIR_OF_COMPUTERS_AND_GOODS,
        /// <summary />
        PRINTING_AND_REPRODUCTION_OF_RECORDED_MEDIA,
        /// <summary />
        MANUFACTURE_OF_FURNITURE,
        /// <summary />
        OTHER_MANUFACTURING,
        /// <summary />
        ADVERTISING_AND_MARKET_RESEARCH,
        /// <summary />
        OTHER_PROFESSIONAL_SCIENTIFIC_AND_TECHNICAL_ACTIVITIES,
        /// <summary />
        ARTS_ENTERTAINMENT_AND_RECREATION
    }
}
