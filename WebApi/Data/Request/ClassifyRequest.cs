using System.Collections.Generic;

namespace WebApplication.Data.Request
{
    public class ClassifyRequest
    {
        public List<List<string>> Criteria { get; set; }
        public List<int> Answers { get; set; }
    }
}