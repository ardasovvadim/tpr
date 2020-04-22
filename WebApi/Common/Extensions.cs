using System.Collections.Generic;
using System.Linq;
using LAB2;

namespace WebApplication.Common
{
    public static class Extensions
    {
        public static List<Criterion> ToCriterion(List<List<string>> criteriaParam)
        {
            var criteria = new List<Criterion>();
            criteriaParam.ForEach(param =>
            {
                var crit = new Criterion(param[0], param.Skip(1).ToArray());
                criteria.Add(crit);
            });
            return criteria;
        }
    }
}