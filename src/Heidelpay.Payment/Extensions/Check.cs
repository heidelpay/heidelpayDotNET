﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Heidelpay.Payment.Extensions
{
    public static class Check
    {
        public static void NotNull(object obj, string parameterName)
        {
            if (obj == null) throw new ArgumentNullException(parameterName);
        }
    }
}