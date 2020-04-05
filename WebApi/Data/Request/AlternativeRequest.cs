using System.Collections.Generic;
using LAB2;

namespace WebApplication.Data.Request
{
    public class AlternativeRequest
    {
        public List<Alternative> Alternatives { get; set; }
        public Alternative FindAlternative { get; set; }
    }
}