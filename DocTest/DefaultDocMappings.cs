using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace DocTest
{
    /// <summary>
    /// Default document type mapping implementation
    /// </summary>
    public class DefaultDocMappings : IDocMappings
    {
        internal class DocTypeInfoEntry
        {
            public Type DocType { get; set; }
            public string TypeId { get; set; }
            public Func<object, string> GetId { get; set; }
        }

        private ConcurrentDictionary<string, DocTypeInfoEntry> _id2Types = new ConcurrentDictionary<string, DocTypeInfoEntry>();
        private ConcurrentDictionary<Type, DocTypeInfoEntry> _type2Types = new ConcurrentDictionary<Type, DocTypeInfoEntry>();


        public IEnumerable<Type> GetDocTypes()
        {
            return _type2Types.Keys;
        }

        public string TypeId(Type t)
        {
            DocTypeInfoEntry e;
            return _type2Types.TryGetValue(t, out e) ? e.TypeId : null;
        }

        public Type GetDocType(string typeId)
        {
            DocTypeInfoEntry e;
            return _id2Types.TryGetValue(typeId.ToLower(), out e) ? e.DocType : null;
        }

        public string GetDefaultIndexName(Type docType)
        {
            ESDocumentTypeAttribute att = (ESDocumentTypeAttribute)Attribute.GetCustomAttribute(docType, typeof(ESDocumentTypeAttribute));
            if (att == null) throw new Exception();
            if (!string.IsNullOrEmpty(att.IndexName)) return att.IndexName;
            throw new Exception();
        }

        public void RegisterDocumentsFromAssembly(System.Reflection.Assembly asm)
        {
            foreach (var tp in asm.GetTypes())
            {
                if (tp.IsInterface) continue;
                ESDocumentTypeAttribute att = (ESDocumentTypeAttribute) Attribute.GetCustomAttribute(tp, typeof(ESDocumentTypeAttribute));
                if (att != null)
                {
                    RegisterDocumentType(tp);
                }
            }
        }

        public void RegisterDocumentType(Type t)
        {
            var pi = t.GetProperty("Id");
            if (pi == null)
            {
                pi = t.GetProperty("_id");
                if (pi == null) throw new Exception("Id or _id property required for document key");
            }
            //var dlg = Delegate.CreateDelegate(typeof(Func<object, string>), null, pi.GetGetMethod());
            var dlg = BuildIdAccessor(pi.GetGetMethod());
            RegisterDocumentType(t, dlg);
        }


        public void RegisterDocumentType<T>(Func<T, string> getId)
        {
            RegisterDocumentType(typeof(T), x => getId((T)x));
        }

        public void RegisterDocumentType(Type t, Func<object, string> getId)
        {
            ESDocumentTypeAttribute att = (ESDocumentTypeAttribute)Attribute.GetCustomAttribute(t, typeof(ESDocumentTypeAttribute));
            if (att == null) throw new Exception("ESDocumentTypeAttribute");
            if (_type2Types.ContainsKey(t)) return;
            string tid = GetTypeId(t);
            if (_id2Types.ContainsKey(tid)) throw new Exception("TypeId conflict for " + tid);
            DocTypeInfoEntry e = new DocTypeInfoEntry
            {
                DocType = t,
                TypeId = tid.ToLower(),
                GetId = getId
            };
            _id2Types.TryAdd(e.TypeId, e);
            _type2Types.TryAdd(e.DocType, e);
        }

        protected static Func<object, string> BuildIdAccessor(MethodInfo method)
        {
            var obj = Expression.Parameter(typeof(object), "o");

            Expression<Func<object, string>> expr =
                Expression.Lambda<Func<object, string>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(obj, method.DeclaringType),
                            method),typeof(string)),
                    obj);

            return expr.Compile();
        }

        protected virtual string GetTypeId(Type tp)
        {
            ESDocumentTypeAttribute att = (ESDocumentTypeAttribute)Attribute.GetCustomAttribute(tp, typeof(ESDocumentTypeAttribute));
            if (!string.IsNullOrEmpty(att.TypeID)) return att.TypeID.ToLower();
            return tp.Name.ToLower();
        }


        public string GetKeyValue(object document)
        {
            DocTypeInfoEntry e;
            if (!_type2Types.TryGetValue(document.GetType(), out e)) throw new Exception("Document type not registered " + document.GetType().FullName);
            return e.GetId(document);
        }


        public string GetFullId(object document)
        {
            string tid = TypeId(document.GetType());
            string key = GetKeyValue(document);
            return DocUtil.FullId(tid, key);
        }
    }
}
