using System.Collections.Generic;
using System.Linq;
using LAB2;

namespace WebApplication.Data.Response
{
    public class AlternativeRes
    {
        public int Id { get; set; }
        public List<AltPair> values { get; set; }
        public class AltPair
        {
            public string NameCriterion { get; set; }
            public string Value { get; set; }
        }
    }
    public static class AlternativeExtension 
    {
        public static List<AlternativeRes> ToListRes(this List<Alternative> alternatives)
        {
            return alternatives.Select(alt => alt.ToRes()).ToList();
        }

        public static AlternativeRes ToRes(this Alternative alternative)
        {
            return new AlternativeRes
            {
                Id = alternative.Id,
                values = alternative.AlternativeValues.Select(val => new AlternativeRes.AltPair
                    {NameCriterion = val.Key.Name, Value = val.Value.Name}).ToList()
            };
        }

        // public static List<Alternative> ToListAlternative(this List<AlternativeRes> alternatives, List<Criterion> criteria)
        // {
        //     return alternatives.Select(alt => alt.ToAlternative(criteria)).ToList();
        // }

        // public static Alternative ToAlternative(this AlternativeRes alternative, List<Criterion> criteria)
        // {
        //     return new Alternative
        //     {
        //         Id =  alternative.Id,
        //         AlternativeValues = alternative.values.Select(v =>
        //         {
        //             var key = criteria.Find(c => c.Name == v.NameCriterion);
        //             var value = key.CriterionValues.FirstOrDefault(val => val.Name == v.NameCriterion);
        //             return new KeyValuePair<Criterion, CriterionValue>(key, value);
        //         }).ToList()
        //     };
        // }
    }
    
}