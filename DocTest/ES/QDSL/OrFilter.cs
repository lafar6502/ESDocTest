using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoArc.Services.ES.QDSL
{
    public class OrFilter : Filter
    {
        public List<Filter> Args { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            if (Args == null || Args.Count == 0) return;
            if (Args.Count == 1)
            {
                Args[0].ToJson(jsw);
                return;
            }
            jsw.WriteStartObject();
            jsw.WritePropertyName("or");
            jsw.WriteStartArray();
            foreach (var f in Args)
                f.ToJson(jsw);
            jsw.WriteEndArray();
            jsw.WriteEndObject();
        }
    }
}
