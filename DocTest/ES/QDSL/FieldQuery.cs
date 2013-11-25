using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest.Services.ES.QDSL
{
    public class FieldQuery : SimpleQuery
    {
        public override void ToJson(Newtonsoft.Json.JsonWriter jsw)
        {
            throw new NotImplementedException();
        }
    }
}
