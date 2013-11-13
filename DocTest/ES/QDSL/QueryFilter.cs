using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoArc.Services.ES.QDSL
{
    public class QueryFilter : Filter
    {
        public QueryBase Query { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            if (Query == null) return;
            jsw.WriteStartObject();
            jsw.WritePropertyName("query");
            Query.ToJson(jsw);
            jsw.WriteEndObject();
        }
    }
}
