using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace InfoArc.Services.ES.QDSL
{
    public class QueryString : SimpleQuery
    {
        [JsonProperty("query")]   
        public string Query { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            this.DefaultJsonDSL("query_string", jsw);
        }

    }
}
