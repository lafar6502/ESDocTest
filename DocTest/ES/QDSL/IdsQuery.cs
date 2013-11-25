using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.Services.ES.QDSL
{
    class IdsQuery : QueryBase
    {
        [JsonProperty("type")]
        public string DocType { get; set; }
        [JsonProperty("values")]
        public List<string> Ids { get; set; }

        public override void ToJson(JsonWriter jsw)
        {
            DefaultJsonDSL("ids", jsw);
        }
    }
}
