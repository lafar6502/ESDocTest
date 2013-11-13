using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest
{
    public class DocUtil
    {
        public static string[] SplitFullId(string fullId)
        {
            var r = fullId.Split('~');
            if (r.Length != 2) throw new Exception("Invalid document id");
            return r;
        }

        public static string DocTypeId(string fullId)
        {
            return SplitFullId(fullId)[0];
        }

        public static string DocKey(string fullId)
        {
            return SplitFullId(fullId)[1];
        }

        public static string FullId(string typeId, string key)
        {
            return string.Format("{0}~{1}", typeId, key);
        }
    }
}
