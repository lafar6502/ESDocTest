using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace InfoArc.Services.ES.QDSL
{
    public class RangeFilter : SimpleFilter
    {
        public string Field { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
            
        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            base.ToJson(jsw);
        }
    }
}
