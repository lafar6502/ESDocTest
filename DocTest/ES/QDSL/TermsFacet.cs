using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.Services.ES.QDSL
{
    public class TermsFacet : FacetBase
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("size")]
        public int? Size { get; set; }
        [JsonProperty("order")]
        public string Order { get; set; }
        [JsonProperty("exclude")]
        public List<string> Exclude { get; set; }
        [JsonProperty("script")]
        public string Script { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            
            DefaultJsonDSL("terms", jsw);
            
        }
    }
}
