using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest
{
    /// <summary>
    /// Document mapping interface
    /// </summary>
    public interface IDocMappings
    {
        /// <summary>
        /// registered document types
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetDocTypes();
        /// <summary>
        /// Returns null if specified type has no mapping
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        string TypeId(Type t);
        /// <summary>
        /// return type for typeid
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        Type GetDocType(string typeId);
        /// <summary>
        /// default index for storing documents of specified type
        /// </summary>
        /// <param name="docType"></param>
        /// <returns></returns>
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
