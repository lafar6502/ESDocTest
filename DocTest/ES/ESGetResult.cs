using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.ES
{
    public class ESGetResult : ESBaseResult
    {
        public string _id;
        public string _index;
        [JsonProperty(TypeNameHandling=TypeNameHandling.Auto)]
        public object _source;
        public string _type;
        public long _version;
        public bool exists;
    }
}
