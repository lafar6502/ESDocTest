using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoArc.Services.ES.QDSL
{
    public class RangeQuery : SimpleQuery
    {
        public string Field { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            throw new NotImplementedException();
        }
    }
}
