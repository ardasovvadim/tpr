using System.Collections.Generic;
using System.Linq;
using LAB2;

namespace WebApplication.Data.Request
{
    public class CriterionReq
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> CriterionValues { get; set; }
    }

    public static class CriterionExtension
    {
        public static List<Criterion> ToListCriterion(this List<CriterionReq> req)
        {
            return req.Select(r => r.ToCriterion()).ToList();
        }

        public static Criterion ToCriterion(this CriterionReq req)
        {
            return new Criterion(req.Name, req.CriterionValues.ToArray());
        }

        public static List<CriterionReq> ToListReq(this List<Criterion> criteria)
        {
            return criteria.Select(c => c.ToReq()).ToList();
        }

        public static CriterionReq ToReq(this Criterion c)
        {
            return new CriterionReq
            {
                Id = c.Id,
                Name = c.Name,
                CriterionValues = c.CriterionValues.Select(val => val.Name).ToList()
            };
        }
    }
}