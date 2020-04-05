using System.Collections.Generic;
using System.Linq;
using LAB2;
using LAB2.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.Data.Request;
using WebApplication.Data.Response;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/alternative")]
    public class AlternativeController : ControllerBase
    {
        private readonly ILogger<AlternativeController> _logger;
        
        public AlternativeController(ILogger<AlternativeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public List<Alternative> Post([FromBody] List<List<string>> criteriaParam)
        {
            var criteria = ToCriterion(criteriaParam);
            return criteria.GetAllAlternatives();
        }

        private List<Criterion> ToCriterion(List<List<string>> criteriaParam)
        {
            var criteria = new List<Criterion>();
            criteriaParam.ForEach(param =>
            {
                var crit = new Criterion(param[0], param.Skip(1).ToArray());
                criteria.Add(crit);
            });
            return criteria;
        }

        [HttpPost("GetBestAndWorst")]
        public ActionResult GetBestAndWorst([FromBody] List<Alternative> alternatives)
        {
            var obj = alternatives.GetTheBestAndTheWorseAlternative();
            List<Alternative> res = new List<Alternative>();
            res.Add(obj.best);
            res.Add(obj.worse);
            return Ok(res);
        }
        
        [HttpPost("GetBetter")]
        public ActionResult GetBetter([FromBody] AlternativeRequest request)
        {
            var findAlt = request.Alternatives.Find(alt => alt.Id == request.FindAlternative.Id);
            return Ok(request.Alternatives.GetBetterAlternatives(findAlt));
        }
        
        [HttpPost("GetWorse")]
        public ActionResult GetWorse([FromBody] AlternativeRequest request)
        {
            var findAlt = request.Alternatives.Find(alt => alt.Id == request.FindAlternative.Id);
            return Ok(request.Alternatives.GetWorseAlternatives(findAlt));
        }

        [HttpPost("NotComparable")]
        public ActionResult GetNotComparable([FromBody] AlternativeRequest request)
        {
            var findAlt = request.Alternatives.Find(alt => alt.Id == request.FindAlternative.Id);
            return Ok(request.Alternatives.GetNotComparableAlternatives(findAlt));
        }
    }
}