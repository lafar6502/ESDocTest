using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace DocTest
{
    public class DocSerializer
    {
        private JsonSerializer _ser;
        private IDocMappings _mappings;
        public DocSerializer(IDocMappings mappings)
        {
            _mappings = mappings;
            JsonSerializerSettings ss = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            _ser = JsonSerializer.Create(ss);
            _ser.Converters.Add(new Newtonsoft.Json.Converters.IsoDateTimeConverter());
            _ser.Converters.Add(new PlainElastic.Net.Serialization.FacetCreationConverter());
        }

        public string SerializeDocument(object doc)
        {
            var job = JObject.FromObject(doc, _ser);
            var tp = doc.GetType();
            JArray jar = new JArray();
            while (tp != null && tp != typeof(object))
            {
                var tid = _mappings.TypeId(tp);
                if (tid == null) break;
                jar.Add(tid);
                tp = tp.BaseType;
            }
            job["_inherits"] = jar;
            var tw = new StringWriter();
            _ser.Serialize(tw, job);
            return tw.ToString();
        }

        public object Deserialize(string json)
        {
            return _ser.Deserialize(new JsonTextReader(new StringReader(json)));
        }

        public T Deserialize<T>(string json)
        {
            return _ser.Deserialize<T>(new JsonTextReader(new StringReader(json)));
        }

        
    }
}
