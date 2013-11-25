using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.Services.ES.QDSL
{
    public abstract class CompositeQuery : QueryBase
    {
        public abstract void AddChildQuery(QueryBase qb);
        public abstract IEnumerable<QueryBase> ChildQueries { get; }
    }
}
