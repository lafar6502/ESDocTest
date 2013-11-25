using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest.Services.ES.QDSL
{
    public class NotFilter : CompositeFilter
    {
        public Filter Filter { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            if (Filter == null) return;
            jsw.WritePropertyName("not");
            Filter.ToJson(jsw);
        }
    }
}
