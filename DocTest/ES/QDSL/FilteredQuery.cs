using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest.Services.ES.QDSL
{
    public class FilteredQuery : CompositeQuery
    {
        public QueryBase Query { get; set; }
        public Filter Filter { get; set; }

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            
            if (Query == null && Filter == null) return;
            jsw.WriteStartObject();
            jsw.WritePropertyName("filtered");
            jsw.WriteStartObject();
            if (Query != null)
            {
                jsw.WritePropertyName("query");
                Query.ToJson(jsw);
            }
            if (Filter != null)
            {
                jsw.WritePropertyName("filter");
                Filter.ToJson(jsw);
            }
            jsw.WriteEndObject();
            jsw.WriteEndObject();
        }

        public override void AddChildQuery(QueryBase qb)
        {
            Query = qb;
        }

        public override IEnumerable<QueryBase> ChildQueries
        {
            get { return new QueryBase[] { Query }; }
        }
    }
}
