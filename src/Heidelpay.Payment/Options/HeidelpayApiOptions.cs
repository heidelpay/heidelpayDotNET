﻿using System;

namespace Heidelpay.Payment.Options
{
    public class HeidelpayApiOptions
    {
        public Uri ApiEndpoint { get; set; }

        public string ApiKey { get; set; }

        public string HttpClientName { get; set; }
    }
}
