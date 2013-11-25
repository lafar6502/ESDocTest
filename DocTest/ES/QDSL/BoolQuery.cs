using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.Services.ES.QDSL
{
    public class BoolQuery : CompositeQuery
    {
        public List<QueryBase> Must { get; set; }
        public List<QueryBase> Should { get; set; }
        public List<QueryBase> MustNot { get; set; }
        public int? MinimumShouldMatch { get; set; }

        

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            jsw.WriteStartObject();
            jsw.WritePropertyName("bool");
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
            jsw.WriteEndObject();
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
