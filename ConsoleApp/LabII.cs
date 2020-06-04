using System;
using System.Collections.Generic;
using LAB2;

namespace ConsoleApp
{
    public class LabII : ILab
    {
        public void Run()
        {
            var criteria = new List<Criterion>
            {
                new Criterion("K1", "K1_1", "K1_2", "K1_3"),
                new Criterion("K2", "K2_1", "K2_2", "K2_3"),
            };
            var criteria2 = new List<Criterion>
            {
                new Criterion("K1", "банан", "ананас", "яблоко"),
                new Criterion("K2", "синий", "белый", "чорный"),
            };
            var answers = new List<int> {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2};
            var answers2 = new List<int> {2, 1, 2};
            var table = new ClassificationTable2(criteria);
            table.PrintConsole();
            {
                int i = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                while (!table.isClassified())
                {
                    if (i >= answers2.Count) throw new Exception("Table can't be classified. No more answers");
                    table.NextStep(answers2[i]).PrintConsole();
                    ++i;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}