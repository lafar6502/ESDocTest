using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest.Services.ES.QDSL
{
    public class FilterFacet : FacetBase
    {
        public Filter Filter { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            if (Filter == null) throw new Exception("Missing filter in facet " + Name);
            jsw.WriteStartObject();
            jsw.WritePropertyName("filter");
            Filter.ToJson(jsw);
            jsw.WriteEndObject();
        }
    }
}
