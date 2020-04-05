using System.Collections.Generic;

namespace LAB2
{
    public class Criterion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CriterionValue> CriterionValues { get; set; }
        private int _indexer = 1;

        public Criterion()
        {
        }

        public Criterion(string name, params string[] values)
        {
            Name = name;
            CriterionValues = new List<CriterionValue>();
            foreach (var value in values)
            {
                CriterionValues.Add(new CriterionValue(value, _indexer));
                ++_indexer;
            }
        }
    }

    public class CriterionValue
    {
        public string Name { get; }
        public int Index { get; set; }

        public CriterionValue(string name, int index)
        {
            Name = name;
            Index = index;
        }

        public CriterionValue()
        {
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }
}