﻿using Heidelpay.Payment.Interfaces;
using System.Collections.Generic;

namespace Heidelpay.Payment
{
    public class MetaData : IRestResource
    {
        public string Id { get; set; }
        public Heidelpay Heidelpay { get; set; }

        public IDictionary<string, string> MetadataMap { get; set; } = new Dictionary<string, string>();

        public MetaData() 
            : this(false)
        {
        }

        public MetaData(bool sorted)
        {
            if(sorted)
            {
                MetadataMap = new SortedDictionary<string, string>();
            }
        }

        public string TypeUrl => "metadata";
    }
}