using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest
{
    public interface IDocMappings
    {
        IEnumerable<Type> GetDocTypes();
        /// <summary>
        /// Returns null if specified type has no mapping
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        string TypeId(Type t);
        Type GetDocType(string typeId);
        string GetDefaultIndexName(Type docType);
        /// <summary>
        /// Get document ID, without type prefix
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        string GetKeyValue(object document);
        /// <summary>
        /// Get full document id for ES
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        string GetFullId(object document);
    }
}
