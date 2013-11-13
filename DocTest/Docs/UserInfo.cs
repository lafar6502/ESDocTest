using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DocTest.Docs
{
    [ESDocumentType(IndexName="doctest")]
    public class UserInfo
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public List<string> MemberOf { get; set; }
    }
}
