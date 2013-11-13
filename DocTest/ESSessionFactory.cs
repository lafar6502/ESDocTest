using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlainElastic.Net;

namespace DocTest
{
    public class ESSessionFactory
    {
        public string ESHost { get; set; }
        public int ESPort { get; set; }
        public IDocMappings DocMappings { get; set; }
        
        public ESSessionFactory()
        {
            ESHost = "localhost";
            ESPort = 9200;
        }

        private ElasticConnection GetConnection()
        {
            ElasticConnection ec = new ElasticConnection(ESHost, ESPort);
            return ec;
        }
        
        public IDocStoreSession OpenSession()
        {
            return new ESStoreSession(GetConnection(), DocMappings);
        }
    }
}
