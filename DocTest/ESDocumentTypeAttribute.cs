using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest
{
    public class ESDocumentTypeAttribute : System.Attribute
    {
        public string IndexName { get; set; }
        public string TypeID { get; set; }
    }
}
