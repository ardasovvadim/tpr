using System;
using System.Collections.Generic;
using System.Linq;
using LAB2.Extensions;

namespace LAB2
{
    public class FindBestAlt
    {
        private readonly List<Criterion> _criteria;
        private readonly List<Alternative> _allAlternatives;
        private readonly List<int> _answers;
        private readonly List<Alternative> _alternativesForCompare;
        private readonly List<Alternative> _compareResults;

        public FindBestAlt(List<Criterion> criteria, List<List<int>> alternatives, List<int> answers)
        {
            _criteria = criteria;
            _allAlternatives = criteria.GetAllAlternatives();
            _answers = answers;
            _alternativesForCompare =
                alternatives.Select(vector => _allAlternatives.GetAlternativeByVector(vector)).ToList();
            _compareResults = new List<Alternative>();
        }

        public Alternative FindTheBest()
        {
            var rankAlts = new Dictionary<Alternative, int>();

            for (var i = 0; i < _alternativesForCompare.Count - 1; i++)
            {
                var currentAlternative1 = _alternativesForCompare[i];
                var amountValues = currentAlternative1.AlternativeValues.Count;
                var limitationsRanges1 = GetZeroListBySize(amountValues);
                for (var j = i + 1; j < _alternativesForCompare.Count; j++)
                {
                    var currentAlternative2 = _alternativesForCompare[j];
                    var limitationsRanges2 = GetZeroListBySize(amountValues);

                    SetLimitationsRanges(currentAlternative1, limitationsRanges1, currentAlternative2,
                        limitationsRanges2);

                    var baseAlternative = GetBaseAlternative(currentAlternative1, limitationsRanges1,
                        currentAlternative2);

                    Console.WriteLine("\nBase alternative: " + baseAlternative.ToString() + "\n");

                    var currentRank1 = 1;
                    var currentRank2 = 1;
                    var results = new List<int>();
                    var copyBaseAlt1 = new Alternative(baseAlternative);
                    var copyBaseAlt2 = new Alternative(baseAlternative);
                    var markIndexes1 = new List<int>();
                    var markIndexes2 = new List<int>();
                    var @continue = true;
                    var lastRound = 0;
                    while (@continue)
                    {
                        if (lastRound == 0 || lastRound == 1)
                        {
                            var indexNextRank1 = limitationsRanges1.FindIndex(value => value == currentRank1);
                            copyBaseAlt1.AlternativeValues[indexNextRank1] =
                                currentAlternative1.AlternativeValues[indexNextRank1];
                            markIndexes1.Add(indexNextRank1);
                        }

                        if (lastRound == 0 || lastRound == 2)
                        {
                            var indexNextRank2 = limitationsRanges2.FindIndex(value => value == currentRank2);
                            copyBaseAlt2.AlternativeValues[indexNextRank2] =
                                currentAlternative2.AlternativeValues[indexNextRank2];
                            markIndexes2.Add(indexNextRank2);
                        }

                        var chosenAlt = PrintCompareAlternatives(copyBaseAlt1, copyBaseAlt2, markIndexes1,
                            markIndexes2);

                        if (chosenAlt == copyBaseAlt1)
                        {
                            var added = false;
                            if (lastRound == 2)
                            {
                                added = true;
                                results.Add(2);
                                lastRound = 0;
                                copyBaseAlt1 = new Alternative(baseAlternative);
                                copyBaseAlt2 = new Alternative(baseAlternative);
                                markIndexes1.Clear();
                                markIndexes2.Clear();
                                currentRank1 = currentRank2;
                            }
                            else
                            {
                                ++currentRank1;
                                lastRound = 1;
                            }


                            if (!limitationsRanges1.Contains(currentRank1))
                            {
                                if (!added)
                                {
                                    results.Add(1);
                                }

                                @continue = false;
                            }
                        }
                        else
                        {
                            var added = false;
                            if (lastRound == 1)
                            {
                                added = true;
                                results.Add(1);
                                lastRound = 0;
                                copyBaseAlt1 = new Alternative(baseAlternative);
                                copyBaseAlt2 = new Alternative(baseAlternative);
                                markIndexes1.Clear();
                                markIndexes2.Clear();
                                currentRank2 = currentRank1;
                            }
                            else
                            {
                                ++currentRank2;
                                lastRound = 2;
                            }


                            if (!limitationsRanges2.Contains(currentRank2))
                            {
                                if (!added)
                                {
                                    results.Add(1);
                                }

                                @continue = false;
                            }
                        }
                    }

                    var res1 = results.Count(r => r == 1);
                    var res2 = results.Count(r => r == 2);

                    if (res1 > res2)
                    {
                        ++rankAlts[currentAlternative1];
                    }
                    else
                    {
                        ++rankAlts[currentAlternative2];
                    }
                }
            }

            return rankAlts.FirstOrDefault(r => r.Value == rankAlts.Max(alt => alt.Value)).Key;
        }

        private Alternative PrintCompareAlternatives(Alternative alt1, Alternative alt2, List<int> markIndexes1,
            List<int> markIndexes2)
        {
            Console.Write("Alternative 1: { ");
            for (var z = 0; z < alt1.AlternativeValues.Count; z++)
            {
                if (markIndexes1.Contains(z))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(alt1.AlternativeValues[z].Value.Name + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(alt1.AlternativeValues[z].Value.Name + " ");
                }
            }

            Console.WriteLine("}");

            Console.Write("Alternative 2: { ");
            for (var z = 0; z < alt2.AlternativeValues.Count; z++)
            {
                if (markIndexes2.Contains(z))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(alt2.AlternativeValues[z].Value.Name + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(alt2.AlternativeValues[z].Value.Name + " ");
                }
            }

            Console.WriteLine("}");

            Console.Write("What better 1 or 2: ");
            var result = Console.ReadLine();
            Console.WriteLine();
            if (int.TryParse(result, out var intResult))
            {
                return intResult == 1 ? alt1 : alt2;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private List<int> GetZeroListBySize(in int amountValues)
        {
            var result = new List<int>();
            for (int i = 0; i < amountValues; i++)
            {
                result.Add(0);
            }

            return result;
        }

        private void SetLimitationsRanges(Alternative currentAlternative1, List<int> limitationsRanges1,
            Alternative currentAlternative2, List<int> limitationsRanges2)
        {
            //left side
            var highlight = new List<Pair<Criterion, CriterionValue>>();
            for (var i = 0; i < currentAlternative1.AlternativeValues.Count; i++)
            {
                if (currentAlternative1.AlternativeValues[i].Value.Index >
                    currentAlternative2.AlternativeValues[i].Value.Index)
                {
                    highlight.Add(currentAlternative1.AlternativeValues[i]);
                }
            }

            MarkRange(currentAlternative1, highlight, limitationsRanges1);

            highlight.Clear();
            for (var i = 0; i < currentAlternative2.AlternativeValues.Count; i++)
            {
                if (currentAlternative2.AlternativeValues[i].Value.Index >
                    currentAlternative1.AlternativeValues[i].Value.Index)
                {
                    highlight.Add(currentAlternative2.AlternativeValues[i]);
                }
            }

            MarkRange(currentAlternative2, highlight, limitationsRanges2);
        }

        private void MarkRange(Alternative currentAlternative1, List<Pair<Criterion, CriterionValue>> highlight,
            List<int> limitationsRanges1)
        {
            Console.WriteLine("Rank highlight's limitations:");
            Console.Write("{ ");
            currentAlternative1.AlternativeValues.ForEach(value =>
            {
                if (highlight.Contains(value))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(value.Value.Name);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(value.Value.Name);
                }

                Console.Write(", ");
            });
            Console.Write("}\n");
            Console.Write("Your range: ");
            var answer = Console.ReadLine();
            var ranks = answer.Split(' ');
            for (var i = 0; i < ranks.Length; i++)
            {
                var rank = ranks[i];
                if (int.TryParse(rank, out var r))
                {
                    var index = currentAlternative1.AlternativeValues.IndexOf(highlight[i]);
                    limitationsRanges1[index] = r;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        private Alternative GetBaseAlternative(Alternative currentAlternative1, List<int> limitationsRanges1,
            Alternative currentAlternative2)
        {
            var baseAlternative = new Alternative();
            for (var i = 0; i < limitationsRanges1.Count; i++)
            {
                if (limitationsRanges1[i] == 0)
                {
                    baseAlternative.AlternativeValues.Add(currentAlternative1.AlternativeValues[i]);
                }
                else
                {
                    baseAlternative.AlternativeValues.Add(currentAlternative2.AlternativeValues[i]);
                }
            }

            return baseAlternative;
        }
    }
}