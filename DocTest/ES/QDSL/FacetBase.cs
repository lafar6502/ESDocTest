using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace InfoArc.Services.ES.QDSL
{
    public abstract class FacetBase : Statement
    {
        [JsonIgnore]
        public string Name { get; set; }
        /// <summary>
        /// Szablon faceta 
        /// </summary>
        [JsonIgnore]
        public string Template { get; set; }
    }
}
