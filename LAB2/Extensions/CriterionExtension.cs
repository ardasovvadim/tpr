using System.Collections.Generic;
using System.Linq;

namespace LAB2.Extensions
{
    public static class CriterionExtension
    {
        public static List<Alternative> GetAllAlternatives(this List<Criterion> criteria)
        {
            var amountAlternatives = criteria.Select(c => c.CriterionValues.Count).Aggregate((p, n) => p * n);
            var alternatives = new List<Alternative>(amountAlternatives);
            var amountValueArray = new List<int>(criteria.Count);
            for (var i = 0; i < criteria.Count; ++i)
            {
                amountAlternatives /= criteria[i].CriterionValues.Count;
                amountValueArray.Add(amountAlternatives);
            }

            var iValueArray = new List<int>(criteria.Count);
            for (int i = 0; i < criteria.Count; i++)
            {
                iValueArray.Add(-1);
            }

            for (var i = 0; i < alternatives.Capacity; ++i)
            {
                var alternative = new Alternative();
                for (var k = 0; k < criteria.Count; k++)
                {
                    if (i % amountValueArray[k] == 0)
                    {
                        ++iValueArray[k];
                        if (iValueArray[k] >= criteria[k].CriterionValues.Count)
                        {
                            iValueArray[k] = 0;
                        }
                    }

                    alternative.AlternativeValues.Add(new Pair<Criterion, CriterionValue>(criteria[k],
                        criteria[k].CriterionValues[iValueArray[k]]));
                }

                alternatives.Add(alternative);
            }

            return alternatives;
        }
    }
}