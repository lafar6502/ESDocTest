using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.Services.ES.QDSL
{
    public class TermFilter : SimpleFilter
    {
        public string Field { get; set; }
        public string Term { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            if (string.IsNullOrEmpty(Term)) return;
            jsw.WriteStartObject();
            jsw.WritePropertyName("term");
            jsw.WriteStartObject();
            jsw.WritePropertyName(Field);
            jsw.WriteValue(Term);
            jsw.WriteEndObject();
            jsw.WriteEndObject();
        }
    }
}
