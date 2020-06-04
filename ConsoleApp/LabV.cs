using System;
using System.Collections.Generic;
using System.Linq;
using LAB2;

namespace ConsoleApp
{
    public class LabV : ILab
    {
        public void Run()
        {
            var criteria = new List<Criterion>
            {
                new Criterion("K1", "K1_1", "K1_2", "K1_3"),
                new Criterion("K2", "K2_1", "K2_2", "K2_3"),
                new Criterion("K3", "K3_1", "K3_2", "K3_3"),
                new Criterion("K4", "K3_1", "K3_2", "K3_3"),
                new Criterion("K5", "K3_1", "K3_2", "K3_3"),
            };

            var criteria2 = new List<Criterion>
            {
                /* 1 */ new Criterion("Programming language", "C#", "Java", "C++", "Golang", "PHP"),
                /* 2 */ new Criterion("Database", "SQLServer", "Oracle", "SQLLite", "MySql", "MongoDB"),
                /* 3 */ new Criterion("UI Framework", "Angular", "React", "Bootstrap", "AngularJs", "Vue.js"),
                /* 4 */ new Criterion("Architect", "Layers", "Client–Server", "MVC", "WEB API", "Monolith"),
                /* 5 */ new Criterion("Deploy", "Jenkins", "Docker", "Kubernetes", "Ansible", "TeamCity")
            };
            
            var alternative = new List<List<string>>
            {
                new List<string> { "C#",  "Oracle", "Bootstrap", "Client–Server", "Docker"},
                new List<string> { "Java",  "MySql", "Angular", "MVC", "Ansible"},
                new List<string> { "Golang",  "SQLServer", "AngularJs", "WEB API", "TeamCity"},
                new List<string> { "PHP",  "SQLServer", "Vue.js", "Layers", "Jenkins"},
                new List<string> { "Java",  "Oracle", "React", "Client–Server", "Docker"},
            };
            
            var alternatives = new List<List<int>>
            {
                new List<int> {1, 2, 3},
                new List<int> {2, 3, 1},
                new List<int> {3, 2, 1},
                new List<int> {3, 2, 1},
                new List<int> {3, 2, 1}
            };

            var iCrit = new Iteration(criteria.Count, criteria.Select(c => c.Name).ToList());
            iCrit.Print();

            var iAlts = new List<Iteration>();
            for (var i = 0; i < alternatives.Count; i++)
            {
                iAlts.Add(new Iteration(alternatives.Count, new List<string> {"A1", "A2", "A3", "A4", "A5"}));
                iAlts[i].Print();
            }

            var iC = new List<double>();
            for (var i = 0; i < criteria.Count; i++)
            {
                var result = 0.0;
                iCrit.WeightCrit.ForEach(val => { iAlts.ForEach(alt => result += val * alt.WeightCrit[i]); });
                iC.Add(result);
            }

            for (var i = 0; i < iC.Count; i++)
            {
                Console.WriteLine($"C{i + 1}: {iC[i]}");
            }

            Console.WriteLine("\n");

            var iBest = iC.FindIndex(val => val == iC.Max());

            Console.WriteLine($"Best alternative: {iBest + 1}");
        }

        private class Iteration
        {
            public List<List<double>> Matrix { get; set; }
            public List<double> OwnVector { get; set; }
            public List<double> WeightCrit { get; set; }
            public List<string> Labels { get; set; }

            public Iteration(int size, List<string> labels = null)
            {
                Matrix = new List<List<double>>();
                OwnVector = new List<double>();
                WeightCrit = new List<double>();
                Labels = labels;

                for (var i = 0; i < size; i++)
                {
                    var result = new List<double>();
                    for (var j = 0; j < size; j++)
                    {
                        result.Add(i == j ? 1 : 0);
                    }

                    Matrix.Add(result);
                    OwnVector.Add(0);
                    WeightCrit.Add(0);
                }

                var variants = new double[] {3, 5, 7, 9};
                for (var i = 0; i < Matrix.Count; i++)
                {
                    for (var j = i + 1; j < Matrix[i].Count; j++)
                    {
                        var index = new Random().Next(0, variants.Length);
                        Matrix[i][j] = variants[index];
                        Matrix[j][i] = 1.0 / variants[index];
                    }
                }

                for (var i = 0; i < Matrix.Count; i++)
                {
                    var result = Matrix[i][0];
                    for (var j = 1; j < Matrix[i].Count; j++)
                    {
                        result *= Matrix[i][j];
                    }

                    OwnVector[i] = Math.Pow(result, 1.0 / Matrix[i].Count);
                }

                var sum = OwnVector.Sum();
                for (var i = 0; i < WeightCrit.Count; i++)
                {
                    WeightCrit[i] = OwnVector[i] / sum;
                }
            }


            public void Print()
            {
                if (Labels != null)
                    Console.WriteLine("\t" + Labels.Aggregate((p, n) => p + "\t" + n) +
                                      "\t|\tOwn\t\t\t|\tWeight\t\t\t");

                for (var i = 0; i < Matrix.Count; i++)
                {
                    if (Labels != null) Console.Write(Labels[i] + "\t");

                    for (var j = 0; j < Matrix[i].Count; j++)
                    {
                        Console.Write(Math.Round(Matrix[i][j], 2) + "\t");
                    }

                    Console.Write("|\t" + OwnVector[i] + "\t|\t" + WeightCrit[i] + "\t\n");
                }

                Console.WriteLine("\n");
            }
        }
    }
}