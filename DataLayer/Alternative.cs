using System.Collections.Generic;
using System.Linq;

namespace LAB2
{
    public class Alternative
    {
        private static int counter = 0;
        public int Id { get; set; }
        public List<Pair<Criterion, CriterionValue>> AlternativeValues { get; set; }
        
        public Alternative()
        {
            Id = counter;
            ++counter;
            AlternativeValues = new List<Pair<Criterion, CriterionValue>>();
        }

        public override string ToString()
        {
            return "Alternative { " + AlternativeValues.Select(v => v.Value.Name + " ").Aggregate((p, n) => p + n) +
                   "}";
        }
    }

    public class Pair<T1, T2>
    {
        public T1 Key { get; set; }
        public T2 Value { get; set; }

        public Pair()
        {
        }
        public Pair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    
}