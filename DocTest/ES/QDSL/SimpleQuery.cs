using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.Services.ES.QDSL
{
    public abstract class SimpleQuery : QueryBase
    {
        [JsonProperty(PropertyName = "boost", NullValueHandling=NullValueHandling.Ignore)]
        public float? Boost { get; set; }
    }
}
