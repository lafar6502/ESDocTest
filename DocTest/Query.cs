using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DocTest
{
    /// <summary>
    /// Lucene query AST node
    /// </summary>
    public abstract class QueryNode
    {
        public AndNode And(QueryNode rhs)
        {
            return new AndNode(this, rhs);
        }

        public OrNode Or(QueryNode rhs)
        {
            return new OrNode(this, rhs);
        }

        public QueryNode Add(QueryNode rhs)
        {
            return new PlusNode(this, rhs);
        }

        public NotNode Not()
        {
            return new NotNode(this);
        }

        public static bool operator true(QueryNode expr)
	    {
	        return false; // never true to disable short-circuit evaluation of a || b
	    }
	
	    public static bool operator false(QueryNode expr)
	    {
	        return false; // never false to disable short-circuit evaluation of a && b
	    }

        public static QueryNode operator &(QueryNode lhs, QueryNode rhs)
        {
            return lhs.And(rhs);
        }

        public static QueryNode operator |(QueryNode lhs, QueryNode rhs)
        {
            return lhs.Or(rhs);
        }

        public static QueryNode operator +(QueryNode lhs, QueryNode rhs)
        {
            return lhs.Add(rhs);
        }


        public static QueryNode operator !(QueryNode n)
        {
            return n.Not();
        }

        public static QueryNode Raw(string query)
        {
            return new RawQuery(query);
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public abstract object Accept(IQueryNodeVisitor visitor);
    }

    public class RawQuery : QueryNode
    {
        private string _q;
        public RawQuery(string q)
        {
            _q = q;
        }

        public override string ToString()
        {
            return _q;
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    

    public class AndNode : QueryNode
    {
        public QueryNode Left { get; set; }
        public QueryNode Right { get; set; }
        public AndNode(QueryNode left, QueryNode right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("({0}) AND ({1})", Left.ToString(), Right.ToString());
        }


        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
        
    }

    public class PlusNode : QueryNode
    {
        public QueryNode Left { get; set; }
        public QueryNode Right { get; set; }
        public PlusNode(QueryNode left, QueryNode right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Left.ToString(), Right.ToString());
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class NotNode : QueryNode
    {
        public QueryNode Inner { get; set; }
        
        public NotNode(QueryNode q)
        {
            Inner = q;
        }

        public override string ToString()
        {
            return string.Format("NOT ({0})", Inner.ToString());
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class OrNode : QueryNode
    {
        public QueryNode Left { get; set; }
        public QueryNode Right { get; set; }
        
        public OrNode(QueryNode left, QueryNode right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return string.Format("({0}) OR ({1})", Left.ToString(), Right.ToString());
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    /// <summary>
    /// Field boolean condition
    /// </summary>
    public class FieldPredicate : QueryNode
    {
        public Field Fld { get; set; }

        /// <summary>
        /// boolean operator types
        /// </summary>
        public enum OperatorType
        {
            EQ,
            LT,
            GT,
            NE,
            IN,
            RANGE,
            LK,
            NULL,
			NOTNULL
        }
        public OperatorType Operator { get;set;}
        public object Value { get;set;}
		public object Value2 { get; set;}

        private static string FormatFieldValue(object v)
        {
            if (v == null) return null;
            var nt = Nullable.GetUnderlyingType(v.GetType());
            if (nt != null)
            {
                PropertyInfo pi = v.GetType().GetProperty("HasValue");
                bool b = (bool) pi.GetValue(v, null);
                if (!b) return null;
                pi = v.GetType().GetProperty("Value");
                v = pi.GetValue(v, null);
            }
            if (v is DateTime)
            {
                return LuceneQueryBuilder.FormatQueryDate((DateTime)v);
            }
            else if (v is string)
            {
                return LuceneQueryBuilder.EscapeQueryText((string)v);
            }
            else
            {
                return Convert.ToString(v);
            }
        }

        public override string ToString()
        {
			if (Fld.Name == null)
			{
				return Convert.ToString(Value); //default field query or a raw query...
			}
			switch(Operator)
			{
				case OperatorType.EQ:
					return string.Format("{0}:{1}", Fld.Name, FormatFieldValue(Value));				
				case OperatorType.LK:
                    var str = Convert.ToString(Value);
                    if (str.EndsWith("*")) str = str.Substring(0, str.Length - 1);
                    return string.Format("{0}:{1}*", Fld.Name, LuceneQueryBuilder.EscapeQueryText(str) + "*");
				case OperatorType.NE:
					return string.Format("-{0}:{1}", Fld.Name, FormatFieldValue(Value));
				case OperatorType.NULL:
					return string.Format("-{0}", Fld.Name);
				case OperatorType.NOTNULL:
					return string.Format("{0}:[* TO *]", Fld.Name);
				case OperatorType.RANGE:
                    string s1 = FormatFieldValue(Value);
                    string s2 = FormatFieldValue(Value2);
					return string.Format("{0}:[{1} TO {2}]", Fld.Name, s1 == null ? "*" : s1, s2 == null ? "*" : s2);
                case OperatorType.LT:
                    return string.Format("{0}:[* TO {1}]", Fld.Name, FormatFieldValue(Value));
                case OperatorType.GT:
                    return string.Format("{0}:[{1} TO *]", Fld.Name, FormatFieldValue(Value));
				case OperatorType.IN:
                    StringBuilder sb = new StringBuilder();
                    List<object> l = Value as List<object>;
                    if (l == null) throw new Exception("Invalid argument in field " + Fld.Name);
                    foreach (object v in l)
                    {
                        if (sb.Length > 0) sb.Append(" OR ");
                        sb.Append(FormatFieldValue(v));
                    }
                    return string.Format("{0}:({1})", Fld.Name, sb);
				default:
					throw new NotImplementedException(string.Format("Field: {0}, Operator: {1}, Value: {2}", Fld.Name, Operator, Value));
			}
            
        }

        public override object Accept(IQueryNodeVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    /// <summary>
    /// object field expression
    /// </summary>
    public class Field
    {
        public string Name { get;set;}

        public FieldPredicate EQ(object v)
        {
            if (v == null) return this.IsNull();
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.EQ, Value = v };
        }

        public static FieldPredicate operator ==(Field lhs, object rhs)
        {
            return lhs.EQ(rhs);
        }

        public FieldPredicate NE(object v)
        {
            if (v == null) return this.IsNotNull();
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.NE, Value = v };
        }

        public static FieldPredicate operator !=(Field lhs, object rhs)
        {
            return lhs.NE(rhs);
        }

        public FieldPredicate LT(object v)
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.LT, Value = v };
        }

        public static FieldPredicate operator <(Field lhs, object rhs)
        {
            return lhs.LT(rhs);
        }

        public FieldPredicate GT(object v)
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.GT, Value = v };
        }

        public static FieldPredicate operator >(Field lhs, object rhs)
        {
            return lhs.GT(rhs);
        }

        public FieldPredicate In<T>(IEnumerable<T> values)
        {
            List<object> l = new List<object>();
            foreach (var v in values)
            {
                l.Add(v);
            }
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.IN, Value = l };
        }

        public FieldPredicate In<T>(params T[] values)
        {
            return In<T>((IEnumerable<T>)values);
        }

        public FieldPredicate Between<T>(T min, T max)
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.RANGE, Value = min, Value2 = max };
        }

        public FieldPredicate Like(string t)
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.LK, Value = t };
        }

        public FieldPredicate IsNull()
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.NULL, Value = null };
        }

        public FieldPredicate IsNotNull()
        {
            return new FieldPredicate { Fld = this, Operator = FieldPredicate.OperatorType.NOTNULL, Value = null };
        }


        public static Field Named(string name)
        {
            return new Field(name);
        }

        public static Field Default
        {
            get
            {
                return Field.Named("_default");
            }
        }

        public Field(string name)
        {
            Name = name;
        }
    }




    internal class DynamicField : System.Dynamic.DynamicObject
    {
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            result = Field.Named(binder.Name);
            return true;
        }
    }

    public interface IQueryNodeVisitor
    {
        object Visit(AndNode n);
        object Visit(OrNode n);
        object Visit(FieldPredicate n);
        object Visit(NotNode n);
        object Visit(PlusNode n);
        object Visit(RawQuery n);
    }

    

    
}
