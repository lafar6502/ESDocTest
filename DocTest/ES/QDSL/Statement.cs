using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace InfoArc.Services.ES.QDSL
{
    public abstract class Statement
    {
        public abstract void ToJson(JsonWriter jsw);

        [JsonIgnore]
        private JsonSerializer _ser;

        public Statement()
        {
        }

        protected JsonSerializer GetSerializer()
        {
            if (_ser != null) return _ser;
            _ser = JsonSerializer.Create(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });
            return _ser;
        }

        protected void DefaultJsonDSL(string propertyName, JsonWriter jsw)
        {
            jsw.WriteStartObject();
            jsw.WritePropertyName(propertyName);
            GetSerializer().Serialize(jsw, this);
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

        protected void WriteSubqueries(string propertyName, IEnumerable<Statement> queries, JsonWriter jsw)
        {
            if (queries == null) return;
            int n = queries.Count();
            if (n == 0) return;
            jsw.WritePropertyName(propertyName);
            if (n > 1) jsw.WriteStartArray();
            foreach (var q in queries)
            {
                q.ToJson(jsw);
            }
            if (n > 1) jsw.WriteEndArray();
        }
    }
}
