using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest
{
    public class LuceneQueryBuilder
    {
        public static QueryNode DynQuery(Func<dynamic, QueryNode> predicate)
        {
            var dq = new DynamicField();
            return predicate(dq);
        }

        public static string EscapeQueryText(string txt)
        {
            StringBuilder sb = new StringBuilder(txt.Length + 2);
            string escs = "+-&&||!(){}[]^\"~*?:\\";
            foreach (char c in txt)
            {
                if (escs.IndexOf(c) >= 0) sb.Append('\\');
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string FormatQueryDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddThh:mm:ssZ") + "/DAY";
        }

        public static QueryNode Raw(string query)
        {
            return new RawQuery(query);
        }

        public static string BuildParametrizedQuery(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public static string BuildParametrizedQuery(string query, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        static void Test()
        {

            var nn = Field.Named("Login") == "ala" && Field.Named("Active") == false;

            nn = nn && Field.Named("Role").In(new object[] { "a", "b", "c" });
            nn = nn && Field.Named("CreatedDate").Between((DateTime?)null, DateTime.Now.AddDays(10));
            var nn2 = QueryNode.Raw("UserId:[* TO *] AND Active:true") && Field.Named("Login").IsNull();

            DynQuery(x => x.Active == true && x.Login == "Das ist mein szajze");

        }
    }
}
