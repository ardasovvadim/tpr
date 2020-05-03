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
                new Criterion("K1", "C#", "Java"),
                new Criterion("K2", "SQL Server", "MySql"),
                new Criterion("K3", "Angular", "React JS"),
                new Criterion("K4", "WEB API", "MVC"),
                new Criterion("K5", "Docker", "Jenkins", "Kubernetes")
            };
            var criteria2 = new List<Criterion>
            {
                new Criterion("K1", "Apple", "Samsung"),
                new Criterion("K2", "iOS 13", "Android 9"),
                new Criterion("K3", "iPhone 11", "Galaxy S10"),
                new Criterion("K4", "5.5", "6"),
                new Criterion("K5", "2436 x 1125", "2688 x 1242", "1792 x 828")
            };
            var answers = new List<int> {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2};
            var answers2 = new List<int> {1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
            var table = new ClassificationTable2(criteria);
            table.PrintConsole();
            {
                int i = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                while (!table.isClassified())
                {
                    if (i >= answers2.Count) throw new Exception("Table can't be classified. No more answers");
                    table.NextStep(answers[i]).PrintConsole();
                    ++i;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}