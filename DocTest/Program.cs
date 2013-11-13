using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocTest
{
    class Program
    {
        public static DefaultDocMappings Mappings = new DefaultDocMappings();
        public static ESSessionFactory SessionFactory;

        static void Main(string[] args)
        {
            SessionFactory = new ESSessionFactory
            {
                ESHost = "localhost",
                ESPort = 9200,
                DocMappings = Mappings
            };
            Mappings.RegisterDocumentsFromAssembly(typeof(Program).Assembly);
            using (var ses = SessionFactory.OpenSession())
            {
                var ui = new Docs.UserInfo
                {
                    Id = "1",
                    Name = "ziutek testowy",
                    Login = "ziutek",
                    MemberOf = new List<string>(new string[] { "Administrators", "Users" }),
                    Email = "ziutek@test.org"
                };
                ses.InsertNew(ui);
                ses.Search<Docs.UserInfo>(u => u.Name == "ziutek");
                ses.SaveChanges();
            }
            
        }
    }
}
