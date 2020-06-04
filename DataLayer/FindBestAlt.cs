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
            // _allAlternatives = criteria.GetAllAlternatives();
            _answers = answers;
            _alternativesForCompare = GetAltsByVector(criteria, alternatives);
            _compareResults = new List<Alternative>();
        }

        private List<Alternative> GetAltsByVector(List<Criterion> criteria, List<List<int>> alternatives)
        {
            var result = new List<Alternative>();
            alternatives.ForEach(alt =>
            {
                var newAlt = new Alternative();
                var values = newAlt.AlternativeValues;
                for (var i = 0; i < criteria.Count; i++)
                {
                    var crit = criteria[i];
                    values.Add(new Pair<Criterion, CriterionValue>(crit, crit.CriterionValues[alt[i] - 1]));
                }

                result.Add(newAlt);
            });
            return result;
        }

        public IEnumerable<Alternative> FindTheBest()
        {
            _alternativesForCompare.ForEach(Console.WriteLine);
            Console.WriteLine("\n\n");
            
            var rankAlts = new Dictionary<Alternative, int>();
            _alternativesForCompare.ForEach(alt => rankAlts[alt] = 0);

            for (var i = 0; i < _alternativesForCompare.Count - 1; i++)
            {
                var currentAlternative1 = _alternativesForCompare[i];
                var amountValues = currentAlternative1.AlternativeValues.Count;
                for (var j = i + 1; j < _alternativesForCompare.Count; j++)
                {
                    var currentAlternative2 = _alternativesForCompare[j];

                    Console.WriteLine($"---------------------------------------------------------------");
                    Console.WriteLine($"Compare {i} alt: {currentAlternative1}");
                    Console.WriteLine($"Compare {j} alt: {currentAlternative2}\n");

                    var limitationsRanges1 = GetZeroListBySize(amountValues);
                    var limitationsRanges2 = GetZeroListBySize(amountValues);

                    SetLimitationsRanges(currentAlternative1, limitationsRanges1, currentAlternative2,
                        limitationsRanges2);

                    var baseAlternative = GetBaseAlternative(currentAlternative1, limitationsRanges1,
                        currentAlternative2);

                    Console.WriteLine("\nBase alternative: " + baseAlternative + "\n");

                    var currentRank1 = 1;
                    var currentRank2 = 1;
                    var copyBaseAlt1 = new Alternative(baseAlternative);
                    var copyBaseAlt2 = new Alternative(baseAlternative);
                    var markIndexes1 = new List<int>();
                    var markIndexes2 = new List<int>();
                    var @continue = true;
                    // 2
                    var lastRound = 0;
                    // 1
                    var results = new List<int>();
                    var favorite = GetFavorite(limitationsRanges1, limitationsRanges2);
                    Console.WriteLine($"Favorite: {favorite}\n");
                    while (@continue)
                    {
                        // get next limitation for left side or initial value
                        // getting worse alternative
                        if (lastRound == 0 || lastRound == 1)
                        {
                            var indexNextRank1 = limitationsRanges1.FindIndex(value => value == currentRank1);
                            copyBaseAlt1.AlternativeValues[indexNextRank1] =
                                currentAlternative1.AlternativeValues[indexNextRank1];
                            markIndexes1.Add(indexNextRank1);
                        }

                        // get next limitation for right side or initial value
                        // getting worse alternative
                        if (lastRound == 0 || lastRound == 2)
                        {
                            var indexNextRank2 = limitationsRanges2.FindIndex(value => value == currentRank2);
                            copyBaseAlt2.AlternativeValues[indexNextRank2] =
                                currentAlternative2.AlternativeValues[indexNextRank2];
                            markIndexes2.Add(indexNextRank2);
                        }

                        // Compare alternatives
                        var chosenAlt = PrintCompareAlternatives(copyBaseAlt1, copyBaseAlt2, markIndexes1,
                            markIndexes2);

                        // if left side better
                        if (chosenAlt == copyBaseAlt1)
                        {
                            var added = false;
                            // last round get right side
                            if (lastRound == 2)
                            {
                                switch (favorite)
                                {
                                    case 1:
                                        results.Add(1);
                                        ++currentRank1;
                                        ++currentRank2;
                                        break;
                                    case 2:
                                        results.Add(2);
                                        currentRank1 = currentRank2;
                                        break;
                                }

                                added = true;
                                lastRound = 0;
                                copyBaseAlt1 = new Alternative(baseAlternative);
                                copyBaseAlt2 = new Alternative(baseAlternative);
                                markIndexes1.Clear();
                                markIndexes2.Clear();
                            }
                            else
                            {
                                ++currentRank1;
                                lastRound = 1;
                            }


                            if (!limitationsRanges1.Contains(currentRank1) ||
                                !limitationsRanges2.Contains(currentRank2))
                            {
                                if (!added)
                                {
                                    // results.Add(1);
                                    switch (favorite)
                                    {
                                        case 1:
                                            results.Add(1);
                                            break;
                                        case 2:
                                            results.Clear();
                                            break;
                                    }
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
                                lastRound = 0;
                                copyBaseAlt1 = new Alternative(baseAlternative);
                                copyBaseAlt2 = new Alternative(baseAlternative);
                                markIndexes1.Clear();
                                markIndexes2.Clear();

                                switch (favorite)
                                {
                                    case 1:
                                        results.Add(1);
                                        currentRank2 = currentRank1;
                                        break;
                                    case 2:
                                        results.Add(2);
                                        ++currentRank1;
                                        ++currentRank2;
                                        break;
                                }
                            }
                            else
                            {
                                ++currentRank2;
                                lastRound = 2;
                            }


                            if (!limitationsRanges2.Contains(currentRank2) ||
                                !limitationsRanges2.Contains(currentRank2))
                            {
                                if (!added)
                                {
                                    // results.Add(2);
                                    switch (favorite)
                                    {
                                        case 1:
                                            results.Clear();
                                            break;
                                        case 2:
                                            results.Add(2);
                                            break;
                                    }
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
                        Console.WriteLine("First win.");
                        Console.WriteLine($"Alt {i}: {currentAlternative1}");
                        Console.WriteLine($"Amount points: {rankAlts[currentAlternative1]}\n");
                    }
                    else if (res1 < res2)
                    {
                        ++rankAlts[currentAlternative2];
                        Console.WriteLine("Second win.");
                        Console.WriteLine($"Alt {j}: {currentAlternative2}");
                        Console.WriteLine($"Amount points: {rankAlts[currentAlternative2]}\n");
                    }
                    else
                    {
                        Console.WriteLine("Can not compare alternatives.\n");
                    }
                }
            }

            var alts = rankAlts.Where(r => r.Value == rankAlts.Max(alt => alt.Value)).ToList();
            if (alts.Count > 1)
            {
                Console.WriteLine("There are several alts with max wins. We need compare their by extra method");
                alts.ForEach(a =>
                    Console.WriteLine(
                        $"Alt {_alternativesForCompare.FindIndex(al => al == a.Key)} points {a.Value}: {a.Key}"));
            }
            else
            {
                var alt = alts.FirstOrDefault();
                Console.WriteLine(
                    $"The best alternative {_alternativesForCompare.FindIndex(al => al == alt.Key)} points {alt.Value}: {alt.Key}");
            }

            return alts.Select(alt => alt.Key);
        }

        private int GetFavorite(List<int> limitationsRanges1, List<int> limitationsRanges2)
        {
            var count1 = limitationsRanges1.Count(value => value != 0);
            var count2 = limitationsRanges2.Count(value => value != 0);
            return count1 > count2 ? 2 : count1 == count2 ? new Random().Next(1, 3) : 1;
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

            if (_answers != null && _answers.Any())
            {
                var answer = _answers.FirstOrDefault();
                if (answer != 0)
                {
                    Console.WriteLine($"What better 1 or 2: {answer}\n");
                    _answers.RemoveAt(0);
                    return answer == 1 ? alt1 : alt2;
                }

                throw new Exception("No answers");
            }
            else
            {
                var value = new Random().Next(1, 3);
                Console.WriteLine($"What better 1 or 2: {value}\n");
                return value == 1 ? alt1 : alt2;

                // Console.Write("What better 1 or 2: ");
                // var result = Console.ReadLine();
                // Console.WriteLine();
                // if (int.TryParse(result, out var intResult))
                // {
                //     return intResult == 1 ? alt1 : alt2;
                // }
                // else
                // {
                //     throw new ArgumentException();
                // }
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

        private void MarkRange(Alternative currentAlternative, List<Pair<Criterion, CriterionValue>> highlight,
            List<int> limitationsRanges)
        {
            Console.WriteLine("Rank highlight's limitations:");
            Console.Write("{ ");
            currentAlternative.AlternativeValues.ForEach(value =>
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
            // var answer = Console.ReadLine();
            // var ranks = answer.Split(' ');
            // for (var i = 0; i < ranks.Length; i++)
            // {
            //     var rank = ranks[i];
            //     if (int.TryParse(rank, out var r))
            //     {
            //         var index = currentAlternative1.AlternativeValues.IndexOf(highlight[i]);
            //         limitationsRanges1[index] = r;
            //     }
            //     else
            //     {
            //         throw new ArgumentException();
            //     }
            // }
            for (var i = 0; i < highlight.Count; i++)
            {
                var index = currentAlternative.AlternativeValues.IndexOf(highlight[i]);
                int value;
                while (true)
                {
                    value = new Random().Next(1, highlight.Count + 1);
                    if (!limitationsRanges.Contains(value)) break;
                }

                limitationsRanges[index] = value;
            }

            Console.WriteLine(limitationsRanges.Where(val => val != 0).Select(val => val.ToString())
                .Aggregate((p, n) => p + " " + n) + "\n");
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