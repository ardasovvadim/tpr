using System;
using System.Collections.Generic;
using LAB2;
using LAB2.Extensions;

namespace ConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            GetFirst();
        }

        public static void GetFirst()
        {
            List<Criterion> criteria = new List<Criterion>
            {
                new Criterion("Смак", "солодкий", "гіркий", "кислий", "солоний"),
                new Criterion("колір", "червоний", "жовтий", "зелений"),
            };

            var alternatives = criteria.GetAllAlternatives();
            Console.WriteLine("All alternatives:");
            alternatives.Print();
            Console.WriteLine();
            var (best, worse) = alternatives.GetTheBestAndTheWorseAlternative();
            Console.WriteLine("The best alternative: " + best.ToString());
            Console.WriteLine("The worst alternative: " + worse.ToString());
            Console.WriteLine();
            var index = 5;
            Console.WriteLine("Better alternatives then " + alternatives[index].ToString());
            var better = alternatives.GetBetterAlternatives(alternatives[index]);
            better.Print();
            Console.WriteLine();
            Console.WriteLine("Worse alternatives then " + alternatives[index].ToString());
            var worseList = alternatives.GetWorseAlternatives(alternatives[index]);
            worseList.Print();
            Console.WriteLine();
            Console.WriteLine("Not compatible alternatives then " + alternatives[index].ToString());
            var notCompatible = alternatives.GetNotComparableAlternatives(alternatives[index]);
            notCompatible.Print();
        }

        public static void Print(this List<Alternative> alternatives)
        {
            int i = 0;
            alternatives.ForEach(alt =>
            {
                Console.WriteLine($"{i + 1}: " + alt.ToString());
                ++i;
            });
        }
    }
}