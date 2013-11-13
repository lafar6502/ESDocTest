using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoArc.Services.ES.QDSL
{
    public class NestedQuery : CompositeQuery
    {
        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            throw new NotImplementedException();
        }

        public override void AddChildQuery(QueryBase qb)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<QueryBase> ChildQueries
        {
            get { throw new NotImplementedException(); }
        }
    }
}
