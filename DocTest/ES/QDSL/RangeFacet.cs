using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace InfoArc.Services.ES.QDSL
{
    public class RangeFacet : FacetBase
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("value_field")]
        public string ValueField { get; set; }
        
        public class Range
        {
            [JsonProperty("from")]
            public string From { get; set; }
            [JsonProperty("to")]
            public string To { get; set; }
        }
        [JsonProperty("ranges")]
        public List<Range> Ranges { get; set; }

        public RangeFacet()
        {
            Ranges = new List<Range>();
        }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            DefaultJsonDSL("range", jsw);
        }
    }
}
