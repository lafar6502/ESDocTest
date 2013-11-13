using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoArc.Services.ES.QDSL
{
    public class MatchAllQuery : SimpleQuery
    {
        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            DefaultJsonDSL("match_all", jsw);
        }
    }

    public class MatchAllFilter : Filter
    {
        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            DefaultJsonDSL("match_all", jsw);
        }
    }
}
