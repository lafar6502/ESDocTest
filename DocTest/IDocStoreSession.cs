using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest
{
    public interface IDocStoreSession : IDisposable
    {
        object Get(string fullId);
        T Get<T>(string partialId);
        void InsertNew(object document);
        void SaveChanges();

        IEnumerable<T> Search<T>(Func<dynamic, QueryNode> query, Func<dynamic, QueryNode> filter = null);
    }
}
