﻿using System;
using System.Collections.Generic;
using System.Linq;
using LAB2.Extensions;

namespace LAB2
{
    public class ClassificationTable2
    {
        public List<Row> Rows { get; set; }
        public int Iteration = 0;
        public List<double> CenterClass1 { get; set; }
        public List<double> CenterClass2 { get; set; }

        public class Row
        {
            public Alternative Alternative { get; set; }
            public int G { get; set; }
            public double D1 { get; set; }
            public double D2 { get; set; }
            public double P1 { get; set; }
            public double P2 { get; set; }
            public double G1 { get; set; }
            public double G2 { get; set; }
            public double F1 { get; set; }
            public double F2 { get; set; }
            public double F { get; set; }
            public bool Highlight { get; set; }
            public bool Changed { get; set; }

            public Row(Alternative alternative)
            {
                Alternative = alternative;
            }

            public Row()
            {
            }

            public override string ToString()
            {
                var result = Alternative.AlternativeValues.Select(v => v.Value.Index.ToString())
                    .Aggregate((p, n) => p + "\t" + n);
                result += "\t" + G + "\t" + Math.Round(D1, 2) + "\t" + Math.Round(D2, 2) + "\t" + Math.Round(P1, 2) +
                          "\t" + Math.Round(P2, 2) + "\t" + G1 + "\t" + G2 + "\t" + Math.Round(F1, 2) +
                          "\t" + Math.Round(F2, 2) + "\t" + Math.Round(F, 2);
                return result;
            }
        }

        public ClassificationTable2(List<Criterion> criteria)
        {
            var alternatives = criteria.GetAllAlternatives();
            Rows = new List<Row>();
            alternatives.ForEach(alt => Rows.Add(new Row(alt)));
            Rows[0].G = 1;
            Rows[^1].G = 2;
            CenterClass1 = new List<double>();
            CenterClass2 = new List<double>();
            criteria.ForEach(_ =>
            {
                CenterClass1.Add(0);
                CenterClass2.Add(0);
            });
            RecountCenter(1, CenterClass1);
            RecountCenter(2, CenterClass2);
        }

        public ClassificationTable2()
        {
        }

        public ClassificationTable2 NextStep(int _class)
        {
            RecountCenter(1, CenterClass1);
            RecountCenter(2, CenterClass2);

            Rows.ForEach(r =>
            {
                r.D1 = r.Alternative.GetD(CenterClass1);
                r.D2 = r.Alternative.GetD(CenterClass2);
            });

            var maxD = GetMaxD();

            Rows.ForEach(r =>
            {
                if (r.G != 0)
                    r.P1 = r.G == 1 ? 1 : 0;
                else
                    r.P1 = (maxD - r.D1) / (2 * maxD - r.D1 - r.D2);
                r.P2 = 1 - r.P1;
            });

            var undefinedAlternatives = GetUndefinedAlternatives();
            Rows.ForEach(r =>
            {
                if (r.G == 0)
                {
                    r.G1 = undefinedAlternatives.GetBetterAlternatives(r.Alternative).Count;
                    r.G2 = undefinedAlternatives.GetWorseAlternatives(r.Alternative).Count;
                }
                else
                {
                    r.G1 = 0;
                    r.G2 = 0;
                }
            });

            Rows.ForEach(r =>
            {
                r.F1 = r.P1 * r.G1;
                r.F2 = r.P2 * r.G2;
                r.F = r.F1 + r.F2;
            });

            Rows.ForEach(r =>
            {
                if ((r.P1 == 1 || r.P1 == 0) && r.G == 0)
                {
                    r.G = r.P1 == 1 ? 1 : 2;
                    r.Changed = true;
                }
            });

            var undefinedRows = Rows.Where(r => r.G == 0).ToList();
            if (undefinedRows.Count > 0)
            {
                var maxF = undefinedRows.Max(r => r.F);
                var rowMaxF = undefinedRows.Find(r => r.F == maxF);
                rowMaxF.Highlight = true;
                rowMaxF.G = _class;
                rowMaxF.Changed = true;
                undefinedAlternatives = GetUndefinedAlternatives();
                var alternatives = _class == 1
                    ? undefinedAlternatives.GetBetterAlternatives(rowMaxF.Alternative)
                    : undefinedAlternatives.GetWorseAlternatives(rowMaxF.Alternative);

                alternatives.ForEach(alt =>
                {
                    var foundAlt = Rows.Find(r => r.Alternative == alt);
                    foundAlt.G = _class;
                    foundAlt.Changed = true;
                });
            }

            ++Iteration;

            return this;
        }

        public bool isClassified()
        {
            return Rows.All(r => r.G != 0);
        }

        private List<Alternative> GetUndefinedAlternatives()
        {
            return Rows.Where(r => r.G == 0).Select(r => r.Alternative).ToList();
        }

        private double GetMaxD()
        {
            var maxD1 = Rows.Max(r => r.D1);
            var maxD2 = Rows.Max(r => r.D2);
            return maxD1 >= maxD2 ? maxD1 : maxD2;
        }

        private void RecountCenter(int _class, List<double> centerClass)
        {
            var alternatives = Rows.Where(r => r.G == _class).Select(r => r.Alternative).ToList();
            for (var i = 0; i < centerClass.Count; i++)
            {
                centerClass[i] = 0;
            }

            alternatives.ForEach(alt =>
            {
                for (var i = 0; i < alt.AlternativeValues.Count; i++)
                {
                    centerClass[i] += alt.AlternativeValues[i].Value.Index;
                }
            });
            for (var i = 0; i < centerClass.Count; i++)
            {
                centerClass[i] /= alternatives.Count;
            }
        }

        public void PrintConsole()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Iteration {Iteration}");
            Console.WriteLine("Amount alternatives: " + Rows.Count);
            Console.WriteLine("Amount alternatives class 1: " + Rows.Where(r => r.G == 1).ToList().Count);
            Console.WriteLine("Amount alternatives class 2: " + Rows.Where(r => r.G == 2).ToList().Count);
            Console.WriteLine($"Center1: " + CenterClass1.Select(c => c.ToString()).Aggregate((p, n) => p + ":" + n));
            Console.WriteLine($"Center2: " + CenterClass2.Select(c => c.ToString()).Aggregate((p, n) => p + ":" + n));
            Console.ForegroundColor = ConsoleColor.White;


            var result = Rows[0].Alternative.AlternativeValues.Select(v => v.Key.Name)
                .Aggregate((p, n) => p + "\t" + n);
            result += "\tG\td1\td2\tp1\tp2\tg1\tg2\tF1\tF2\tF\n";
            Console.WriteLine(result);
            Rows.ForEach(r =>
            {
                Console.ForegroundColor = r.G switch
                {
                    1 => ConsoleColor.Green,
                    2 => ConsoleColor.Red,
                    _ => ConsoleColor.Gray
                };

                if (r.Highlight)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    r.Highlight = false;
                }

                if (r.Changed)
                {
                    Console.WriteLine(r + "\t << CHANGED");
                    r.Changed = false;
                }
                else
                {
                    Console.WriteLine(r);
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
            });
            Console.WriteLine("\n----------------------------------------------------\n");
        }

        public override string ToString()
        {
            var result = Rows[0].Alternative.AlternativeValues.Select(v => v.Key.Name)
                .Aggregate((p, n) => p + "\t" + n);
            result += "\tG\td1\td2\tp1\tp2\tg1\tg2\tF1\tF2\tF\n";
            result += Rows.Select(r => r.ToString()).Aggregate((p, n) => p + "\n" + n);
            return result;
        }
    }
}