using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace DocTest.Services.ES.QDSL
{
    /// <summary>
    /// ES query specification
    /// </summary>
    public class SearchRequest 
    {
        public string QueryId { get; set; }
        public QueryBase Query { get; set; }
        public Filter Filter { get; set; }
        public Filter FacetFilter { get; set; }
        public List<FacetBase> Facets { get; set; }
        public List<string> SearchIndexes { get; set; }
        public List<string> SearchDocTypes { get; set; }
        public List<string> Fields { get; set; }

        public string ResultsTemplate { get; set; }
        public int? ResultLimit { get; set; }

        public SearchRequest()
        {
            Facets = new List<FacetBase>();
        }

        public void ToJson(JsonWriter jsw)
        {
            jsw.WriteStartObject();
            if (Fields != null)
            {
                jsw.WritePropertyName("fields");
                jsw.WriteStartArray();
                Fields.ForEach(s => jsw.WriteValue(s));
                jsw.WriteEndArray();
            }
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
            if (Facets != null && Facets.Count > 0)
            {
                jsw.WritePropertyName("facets");
                jsw.WriteStartObject();
                foreach (var f in Facets)
                {
                    jsw.WritePropertyName(f.Name);
                    f.ToJson(jsw);
                }
                jsw.WriteEndObject();
            }
            jsw.WriteEndObject();
        }

        public string ToJson()
        {
            var sw = new StringWriter();
            JsonTextWriter jtw = new JsonTextWriter(sw);
            ToJson(jtw);
            jtw.Flush();
            return sw.ToString();
        }

        public void AddFacet(FacetBase fb)
        {
            if (Facets == null) Facets = new List<FacetBase>();
            if (Facets.Any(x => x.Name == fb.Name)) throw new Exception("Facet already defined: " + fb.Name);
            Facets.Add(fb);
        }

        public SearchRequest Where(QueryNode qn)
        {
            return this;
        }

        public SearchRequest FilterQuery(QueryNode qn)
        {
            return this;
        }

        public SearchRequest FilterQuery(Func<dynamic, QueryNode> fun)
        {
            return this;
        }

        public SearchRequest Where(Func<dynamic, QueryNode> fun)
        {
            return this;
        }

        public SearchRequest OrderBy(Func<dynamic, Field> fun, bool asc)
        {
            return this;
        }

        public SearchRequest TermFacet(string term)
        {
            throw new NotImplementedException();
        }
    }
}
