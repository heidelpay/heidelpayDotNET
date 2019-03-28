using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.Json.Converters
{
    public class JsonOnlyDateConverter : IsoDateTimeConverter
    {
        public JsonOnlyDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
