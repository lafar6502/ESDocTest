using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace InfoArc.Services.ES.QDSL
{
    public class BoolFilter : Filter
    {
        public List<Filter> Must { get; set; }
        public List<Filter> Should { get; set; }
        public List<Filter> MustNot { get; set; }
        public int? MinimumShouldMatch { get; set; }

        

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            jsw.WriteStartObject();
            if (MinimumShouldMatch.HasValue)
            {
                jsw.WritePropertyName("minimum_should_match");
                jsw.WriteValue(MinimumShouldMatch.Value);
            }
            WriteSubqueries("must", Must, jsw);
            WriteSubqueries("should", Should, jsw);
            WriteSubqueries("must_not", MustNot, jsw);
            jsw.WriteEndObject();
        }

       
    }
}
