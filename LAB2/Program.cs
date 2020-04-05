using System;
using System.Collections.Generic;

namespace LAB2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var criteria = new List<Criterion>
            {
                new Criterion("k1", "k1_1", "k1_2"),
                new Criterion("k2", "k2_1", "k2_2"),
                new Criterion("k3", "k3_1", "k3_2"),
                new Criterion("k4", "k4_1", "k4_2"),
                new Criterion("k5", "k5_1", "k5_2", "k5_3")
            };
            var answers = new List<int> { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            var table = new ClassificationTable2(criteria);
            {
                int i = 0;
                while (!table.isClassified())
                {
                    if (i >= answers.Count) throw new Exception("Table can't be classified. No more answers");
                    table.NextStep(answers[i]).PrintConsole();
                    Console.WriteLine("\n----------------------------------------------------\n");
                    ++i;
                }
            }
        }
        
        public static void Print(this List<Alternative> alternatives)
        {
            alternatives.ForEach(alt => Console.WriteLine(alt.ToString()));
        }
    }
}