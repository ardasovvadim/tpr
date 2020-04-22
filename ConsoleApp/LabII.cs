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
                new Criterion("аромат", "банан", "ананас", "яблука"),
                new Criterion("колір", "синій", "білий", "чорний"),
            };
            var answers = new List<int> { 1, 2};
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
    }
}