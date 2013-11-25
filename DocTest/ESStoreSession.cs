using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;
using DocTest.ES;
using System.Collections.Concurrent;
using QL = DocTest.Services.ES.QDSL;

namespace DocTest
{
    public class ESStoreSession : IDocStoreSession
    {
        private ElasticConnection _conn;
        private IDocMappings _mappings;
        private DocSerializer _ser;

        protected enum DocState
        {
            Unmodified,
            Added,
            Modified
        };

        protected class DocHolder
        {
            public ESGetResult Original { get; set; }
            public object Current { get; set;}
            public DocState State { get; set; }
        }

        private ConcurrentDictionary<string, DocHolder> _cache = new ConcurrentDictionary<string, DocHolder>();

        public ESStoreSession(ElasticConnection conn, IDocMappings mappings)
        {
            this._conn = conn;
            _mappings = mappings;
            _ser = new DocSerializer(_mappings);
        }

        public object Get(string fullId)
        {
            DocHolder dh = _cache.GetOrAdd(fullId.ToLower(), id =>
            {
                Type docType = _mappings.GetDocType(DocUtil.DocTypeId(fullId));
                string index = _mappings.GetDefaultIndexName(docType);
                var cmd = Commands.Get(index, DocUtil.DocTypeId(fullId), DocUtil.DocKey(fullId));
                var res = _conn.Get(cmd.BuildCommand());
                var r = _ser.Deserialize<ESGetResult>(res);
                return new DocHolder { Original = r, Current = r._source, State = DocState.Unmodified };
            });
            return dh.Current;
        }

        public T Get<T>(string key)
        {
            return (T)Get(DocUtil.FullId(_mappings.TypeId(typeof(T)), key));
        }

        public void Dispose()
        {

        }


        public void InsertNew(object document)
        {
            string id = _mappings.GetFullId(document);
            if (_cache.ContainsKey(id)) throw new Exception("Document already added");
            _cache.TryAdd(id, new DocHolder
            {
                Current = document,
                State = DocState.Added
            });
        }



        public void SaveChanges()
        {
            foreach (var dh in _cache.Values.Where(x => x.State == DocState.Added || x.State == DocState.Modified))
            {
                if (dh.State == DocState.Added)
                {
                    InsertESDocument(dh);
                    dh.State = DocState.Unmodified;
                }
                else if (dh.State == DocState.Modified)
                {
                    UpdateESDocument(dh);
                    dh.State = DocState.Unmodified;
                }
            }
            _cache = new ConcurrentDictionary<string, DocHolder>();
        }

        protected void UpdateESDocument(DocHolder dh)
        {
            if (dh.Current == null) throw new Exception();
            string tid = _mappings.TypeId(dh.Current.GetType());
            string index = _mappings.GetDefaultIndexName(dh.Current.GetType());
            string json = _ser.SerializeDocument(dh.Current);
            var cmd = Commands.Index(index, tid, _mappings.GetKeyValue(dh.Current));
            if (dh.Original != null)
                cmd = cmd.Version(dh.Original._version);
            var res = _conn.Post(cmd.BuildCommand(), json);
        }

        protected void InsertESDocument(DocHolder dh)
        {
            if (dh.Current == null) throw new Exception();
            string tid = _mappings.TypeId(dh.Current.GetType());
            string index = _mappings.GetDefaultIndexName(dh.Current.GetType());
            string json = _ser.SerializeDocument(dh.Current);
            var cmd = Commands.Index(index, tid, _mappings.GetKeyValue(dh.Current));
            var res = _conn.Put(cmd.BuildCommand(), json);
        }


        public IEnumerable<T> Search<T>(Func<dynamic, QueryNode> query, Func<dynamic, QueryNode> filter = null)
        {
            string index = _mappings.GetDefaultIndexName(typeof(T));
            string dtype = _mappings.TypeId(typeof(T));
            string lq = LuceneQueryBuilder.DynQuery(query).ToString();
            string fq = filter == null ? null : LuceneQueryBuilder.DynQuery(filter).ToString();

            var sr = new QL.SearchRequest {
                Query = new QL.QueryString { Query = lq },
                Filter = fq == null ? null : new QL.QueryFilter { Query = new QL.QueryString { Query = fq } }

            };

            var res = _conn.Post(Commands.Search(index, dtype).BuildCommand(), sr.ToJson());

            return new List<T>();
        }
    }
}
