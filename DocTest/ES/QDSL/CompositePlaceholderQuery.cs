using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoArc.Services.ES.QDSL
{
    public class CompositePlaceholderQuery : CompositeQuery
    {
        public List<QueryBase> Queries { get; set; }

        public CompositePlaceholderQuery()
        {
            Queries = new List<QueryBase>();
        }

        public override void AddChildQuery(QueryBase qb)
        {
            Queries.Add(qb);
        }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<QueryBase> ChildQueries
        {
            get { return Queries; }
        }
    }
}
