using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoArc.Services.ES.QDSL
{
    /// <summary>
    /// Filter specification
    /// </summary>
    public abstract class Filter : Statement
    {

        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            throw new NotImplementedException();
        }
    }

    
}
