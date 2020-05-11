using System;
using System.Collections.Generic;
using System.Linq;
using LAB2.Extensions;

namespace LAB2
{
    public class CompareMatrix
    {
        private readonly List<Criterion> Criteria;
        private readonly List<Alternative> Alternatives;
        public List<List<string>> Matrix { get; set; }
        public List<string> Vector { get; set; }
        private int NumberCompares { get; }
        private List<Coord> Changed { get; set; } = new List<Coord>();
        private List<List<string>> WaysVectors;
        private List<string> UnionWay;

        private class Coord
        {
            public int I { get; set; }
            public int J { get; set; }

            public Coord(int i, int j)
            {
                I = i;
                J = j;
            }
        }

        public CompareMatrix(List<Criterion> criteria)
        {
            Criteria = criteria;
            Alternatives = criteria.GetAllAlternatives();

            var amount = criteria[0].CriterionValues.Count;
            var startNumber = "";
            for (var i = 0; i < amount; i++)
            {
                startNumber += "1";
            }

            var vectorSize = criteria.Count * (amount - 1);

            Matrix = new List<List<string>>();
            for (int i = 0; i < vectorSize; i++)
            {
                var vector = new List<string>();
                for (int j = 0; j < vectorSize; j++)
                {
                    vector.Add("");
                }

                Matrix.Add(vector);
            }

            for (int i = 0; i < Matrix.Count; i++)
            {
                Matrix[i][i] = "2";
            }

            Vector = new List<string>();
            for (var i = 0; i < criteria.Count; i++)
            {
                for (int j = 2; j <= amount; j++)
                {
                    var number = startNumber.Substring(0, i) + j + startNumber.Substring(i + 1);
                    Vector.Add(number);

                    for (int k = j + 1; k <= amount; k++)
                    {
                        var block = i * (amount - 1);
                        var iterationBlock = j - 2;
                        Matrix[block + iterationBlock][k - 2 + block] = "1";
                    }
                }
            }

            NumberCompares = criteria.Count * (criteria.Count - 1) / 2;
        }

        public string VectorToString()
        {
            return "{ " + Vector.Aggregate((p, n) => p + " " + n) + " }";
        }

        public override string ToString()
        {
            var result = "\t" + Vector.Aggregate((p, n) => p + "\t" + n) + "\n";
            for (var i = 0; i < Matrix.Count; i++)
            {
                result += Vector[i] + "\t";
                result += Matrix[i].Aggregate((p, n) => p + "\t" + n);
                result += "\n";
            }

            return result;
        }

        public void WriteMatrix()
        {
            Console.WriteLine("\t" + Vector.Aggregate((p, n) => p + "\t" + n));
            for (var i = 0; i < Matrix.Count; i++)
            {
                Console.Write(Vector[i] + "\t");
                for (var j = 0; j < Matrix[i].Count; j++)
                {
                    var val = Changed.Find(coord => coord.I == i && coord.J == j);
                    if (val != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    Console.Write(Matrix[i][j] + "\t");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine();
            }

            Changed.Clear();
        }

        public void Compare()
        {
            for (int iAlt = 0; iAlt < Criteria.Count - 1; iAlt++)
            {
                for (int i = iAlt + 1; i < Criteria.Count; i++)
                {
                    var leftCrit = iAlt;
                    var rightCrit = i;
                    var numberCompare = Criteria[i].CriterionValues.Count - 1;
                    for (int j = 0; j < numberCompare; j++)
                    {
                        var leftAltVec = GetVector(leftCrit, j + 1);
                        var rightAltVec = GetVector(rightCrit, j + 1);
                        MarkMatrixPoint(leftAltVec, rightAltVec);
                    }
                }
            }

            for (int i = 0; i < Matrix.Count - 1; i++)
            {
                for (int j = 1 + i; j < Matrix[i].Count; j++)
                {
                    if (Matrix[i][j] == "")
                    {
                        var leftAltVec = Vector[i];
                        var rightAltVec = Vector[j];
                        MarkMatrixPoint(leftAltVec, rightAltVec);
                    }
                }
            }
        }

        private void MarkMatrixPoint(string leftAltVec, string rightAltVec)
        {
            var leftAlt = Alternatives.GetAlternativeByVector(leftAltVec);
            var rightAlt = Alternatives.GetAlternativeByVector(rightAltVec);
            var chooseAlt = leftAlt;
            Console.WriteLine("What better?");
            // WriteChooseAlt(leftAltVec, rightAltVec, 1);
            WriteChooseAlt(leftAlt, rightAlt, 1);
            {
                var cont = true;
                while (cont)
                {
                    var key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                        {
                            chooseAlt = leftAlt;
                            // WriteChooseAlt(leftAltVec, rightAltVec, 1);
                            WriteChooseAlt(leftAlt, rightAlt, 1);
                            break;
                        }
                        case ConsoleKey.RightArrow:
                        {
                            chooseAlt = rightAlt;
                            // WriteChooseAlt(leftAltVec, rightAltVec, 2);
                            WriteChooseAlt(leftAlt, rightAlt, 2);
                            break;
                        }
                        case ConsoleKey.Enter:
                        {
                            cont = false;
                            break;
                        }
                    }
                }
            }
            MarkVector(leftAlt, rightAlt, leftAlt == chooseAlt ? "1" : "3");
            WriteMatrix();
        }

        private void MarkVector(Alternative leftAlt, Alternative rightAlt, string value)
        {
            var iVector = leftAlt.ToVector();
            var jVector = rightAlt.ToVector();
            var i = Vector.IndexOf(iVector);
            var j = Vector.IndexOf(jVector);
            switch (value)
            {
                case "1":
                {
                    var current = i;
                    while (IsExistsLeft(current))
                    {
                        --current;
                        if (Matrix[current][j] != "")
                        {
                            continue;
                        }

                        Matrix[current][j] = "1";
                        Changed.Add(new Coord(current, j));
                    }

                    current = j;
                    while (IsExistsRight(current))
                    {
                        ++current;
                        if (Matrix[i][current] != "")
                        {
                            continue;
                        }

                        Matrix[i][current] = "1";
                        Changed.Add(new Coord(i, current));
                    }

                    break;
                }
                case "3":
                {
                    var current = j;
                    while (IsExistsLeft(current))
                    {
                        --current;
                        if (Matrix[i][current] != "")
                        {
                            continue;
                        }

                        Matrix[i][current] = "3";
                        Changed.Add(new Coord(i, current));
                    }

                    current = i;
                    while (IsExistsRight(current))
                    {
                        ++current;
                        if (Matrix[current][j] != "")
                        {
                            continue;
                        }

                        Matrix[current][j] = "3";
                        Changed.Add(new Coord(current, j));
                    }

                    break;
                }
            }

            if (Matrix[i][j] != "")
            {
                return;
            }

            Changed.Add(new Coord(i, j));
            Matrix[i][j] = value;
        }

        private bool IsExistsRight(int i)
        {
            if (i + 1 >= Vector.Count)
            {
                return false;
            }

            var current = Vector[i];
            var right = Vector[i + 1];
            for (int j = 0; j < current.Length; j++)
            {
                if (current[j] != '1')
                {
                    if (current[j] == Criteria[0].CriterionValues.Count)
                    {
                        return false;
                    }

                    if (right[j] != '1')
                    {
                        var number1 = int.Parse(current[j].ToString());
                        var number2 = int.Parse(right[j].ToString());
                        return number1 + 1 == number2;
                    }
                }
            }

            return false;
        }

        private bool IsExistsLeft(int i)
        {
            if (i - 1 < 0)
            {
                return false;
            }

            var current = Vector[i];
            var left = Vector[i - 1];
            for (int j = 0; j < current.Length; j++)
            {
                if (current[j] != '1')
                {
                    if (current[j] == '2')
                    {
                        return false;
                    }

                    if (left[j] != '1')
                    {
                        var number1 = int.Parse(current[j].ToString());
                        var number2 = int.Parse(left[j].ToString());
                        return number1 - 1 == number2;
                    }
                }
            }

            return false;
        }

        private void WriteChooseAlt<T>(T leftAlt, T rightAlt, int i)
        {
            switch (i)
            {
                case 0:
                {
                    Console.Write(leftAlt + "\t" + rightAlt);
                    break;
                }
                case 1:
                {
                    ClearCurrentConsoleLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(leftAlt);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\t" + rightAlt);
                    break;
                }
                case 2:
                {
                    ClearCurrentConsoleLine();
                    Console.Write(leftAlt);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\t" + rightAlt);
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
            }
        }

        private string GetVector(int crit, int val)
        {
            string start = "";
            Criteria.ForEach(cr => start += "1");
            return start.Substring(0, crit) + (val + 1) + start.Substring(crit + 1);
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public void BuildWayVectors()
        {
            var wayVectors = new List<List<string>>();
            for (int i1 = 0; i1 < Criteria.Count - 1; i1++)
            {
                for (int i2 = i1 + 1; i2 < Criteria.Count; i2++)
                {
                    var iLeftCriteria = i1;
                    var iRightCriteria = i2;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Criteria {i1} and {i2}:");
                    Console.ForegroundColor = ConsoleColor.White;

                    var amountElements = Criteria[0].CriterionValues.Count - 1;
                    var vector1 = Vector.GetRange(iLeftCriteria * amountElements, amountElements);
                    var vector2 = Vector.GetRange(iRightCriteria * amountElements, amountElements);
                    var vector = vector1;
                    vector.AddRange(vector2);
                    var startPoint = Vector[iLeftCriteria * (Criteria[0].CriterionValues.Count - 1)];
                    // !!!
                    // var endPoint = Vector[iRightCriteria * (Criteria[0].CriterionValues.Count - 2)];
                    var accessMatrix = GenerateAccessMatrix(iLeftCriteria, iRightCriteria);
                    Console.Write("\t");
                    vector.ForEach(el => Console.Write(el + "\t"));
                    Console.WriteLine();
                    for (int i = 0; i < accessMatrix.Count; i++)
                    {
                        Console.Write(vector[i] + "\t");
                        for (var j = 0; j < accessMatrix[i].Count; j++)
                        {
                            Console.Write(accessMatrix[i][j] + "\t");
                        }

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                    var wayVector = FindWay(accessMatrix, vector);
                    if (wayVector == null)
                    {
                        throw new Exception();
                    }

                    wayVector.Insert(0, Alternatives.GetTheBestAndTheWorseAlternative().best.ToVector());

                    Console.Write("Way vector: ");
                    Console.Write(wayVector.Aggregate((p, n) => p + " -> " + n));
                    Console.WriteLine("\n");

                    wayVectors.Add(wayVector);
                }
            }

            WaysVectors = wayVectors;
        }

        private List<string> FindWay(List<List<int>> accessMatrix, List<string> vector, List<int> startPoints = null)
        {
            startPoints ??= new List<int> {0, Criteria[0].CriterionValues.Count - 1};
            for (int iStartPoint = 0; iStartPoint < startPoints.Count; iStartPoint++)
            {
                var iFrom = startPoints[iStartPoint];
                var ways = new List<List<int>>();
                ways.Add(new List<int> {iFrom});
                while (true)
                {
                    var addWays = new List<List<int>>();
                    var deleteWays = new List<List<int>>();
                    for (int i = 0; i < ways.Count; i++)
                    {
                        var currentEl = ways[i][^1];
                        var copyWay = ways[i].ToList();
                        for (int j = 0, counter = 0; j < accessMatrix[currentEl].Count; j++)
                        {
                            if (accessMatrix[currentEl][j] == 1)
                            {
                                if (ways[i].Contains(j))
                                {
                                    continue;
                                }

                                if (counter == 0)
                                {
                                    ways[i].Add(j);
                                }
                                else if (counter > 0)
                                {
                                    var newCopyWay = copyWay.ToList();
                                    newCopyWay.Add(j);
                                    addWays.Add(newCopyWay);
                                }

                                ++counter;
                            }
                            else if (j == accessMatrix[currentEl].Count - 1 && accessMatrix[currentEl][j] != 1 &&
                                     counter == 0)
                            {
                                if (ways[i].Count == vector.Count)
                                {
                                    return ways[i].Select(el => vector[el]).ToList();
                                }

                                deleteWays.Add(ways[i]);
                            }
                        }
                    }

                    ways.AddRange(addWays);
                    addWays.Clear();
                    deleteWays.ForEach(w => ways.Remove(w));
                    deleteWays.Clear();
                    if (ways.Count == 0)
                    {
                        break;
                    }
                }
            }

            return null;
        }

        private List<List<int>> GenerateAccessMatrix(in int iLeftCriteria, in int iRightCriteria)
        {
            var amountElements = Criteria[0].CriterionValues.Count - 1;
            var vector = Vector.GetRange(iLeftCriteria * amountElements, amountElements);
            vector.AddRange(Vector.GetRange(iRightCriteria * amountElements, amountElements));

            var matrix = new List<List<int>>();
            for (int i = 0; i < vector.Count; i++)
            {
                var m = new List<int>();
                for (var j = 0; j < vector.Count; j++)
                {
                    m.Add(0);
                }

                matrix.Add(m);
            }

            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0; j < vector.Count; j++)
                {
                    if (i == j) continue;
                    var alt1 = vector[i];
                    var alt2 = vector[j];
                    var index1 = Vector.IndexOf(alt1);
                    var index2 = Vector.IndexOf(alt2);
                    var value = Matrix[index1][index2];
                    if (value != "")
                    {
                        if (IsFromOneVector(alt1, alt2))
                        {
                            if (!IsLeft(alt1, alt2) && !IsRight(alt1, alt2))
                            {
                                continue;
                            }
                        }

                        if (value == "1")
                        {
                            matrix[i][j] = 1;
                        }
                        else if (value == "3")
                        {
                            matrix[j][i] = 1;
                        }
                    }
                }
            }

            return matrix;
        }

        private bool IsRight(string alt1, string alt2)
        {
            var maxNumber = (Criteria[0].CriterionValues.Count).ToString();

            for (var i = 0; i < alt1.Length; i++)
            {
                if (alt1[i] != '1')
                {
                    if (alt1[i].ToString() == maxNumber || alt2[i] == '1')
                    {
                        return false;
                    }

                    var number1 = int.Parse(alt1[i].ToString());
                    var number2 = int.Parse(alt2[i].ToString());
                    return number1 + 1 == number2;
                }
            }

            return false;
        }

        private bool IsLeft(string alt1, string alt2)
        {
            for (var i = 0; i < alt1.Length; i++)
            {
                if (alt1[i] != '1')
                {
                    if (alt1[i] == '2' || alt2[i] == '1')
                    {
                        return false;
                    }

                    var number1 = int.Parse(alt1[i].ToString());
                    var number2 = int.Parse(alt2[i].ToString());
                    return number1 - 1 == number2;
                }
            }

            return false;
        }

        private bool IsFromOneVector(string alt1, string alt2)
        {
            for (var i = 0; i < alt1.Length; i++)
            {
                if (alt1[i] != '1')
                {
                    if (alt2[i] != '1')
                    {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        public void BuildUnionWayVectors()
        {
            var unionVector = WaysVectors.Aggregate((p, n) =>
            {
                p.AddRange(n);
                return p;
            }).Distinct().ToList();

            var matrix = new List<List<int>>();
            for (var i = 0; i < unionVector.Count; i++)
            {
                var v = new List<int>();
                for (var j = 0; j < unionVector.Count; j++)
                {
                    v.Add(0);
                }

                matrix.Add(v);
            }

            WaysVectors.ForEach(way =>
            {
                for (var i = 0; i < way.Count - 1; i++)
                {
                    var left = way[i];
                    var right = way[i + 1];
                    var iElement = unionVector.FindIndex(el => el == left);
                    var jElement = unionVector.FindIndex(el => el == right);
                    matrix[iElement][jElement] = 1;
                }
            });

            unionVector.ForEach(v => Console.Write($"\t{v}"));
            Console.WriteLine();
            for (var i = 0; i < matrix.Count; i++)
            {
                Console.Write($"{unionVector[i]}");
                for (var j = 0; j < matrix[i].Count; j++)
                {
                    Console.Write("\t" + matrix[i][j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            var unionWay = FindWay(matrix, unionVector, new List<int> {0});
            if (unionWay == null)
            {
                throw new Exception();
            }

            Console.Write("Union way vector: ");
            Console.Write(unionWay.Aggregate((p, n) => p + " -> " + n));
            ;
            // for (var i = 0; i < unionWay.Count; i++)
            // {
            //     Console.Write(unionWay[i] + ", ");
            // }

            Console.WriteLine();
            Console.WriteLine();

            UnionWay = unionWay;
        }

        public void SortAlternatives(List<List<int>> alternativeVector)
        {
            var indexes = new List<List<int>>();
            alternativeVector.ForEach(vector =>
            {
                var number = new List<int>();
                for (var i = 0; i < vector.Count; i++)
                {
                    var index = UnionWay.FindIndex(way => vector[i] == int.Parse(way[i].ToString())) + 1;
                    number.Add(index);
                }

                indexes.Add(number);
            });

            var sortNumbers = new List<List<int>>();
            indexes.ForEach(numbers => sortNumbers.Add(numbers.ToList()));

            sortNumbers.ForEach(number => number.Sort());

            var intNumbers = sortNumbers.Select(number =>
                int.Parse(number.Select(num => num.ToString()).Aggregate((p, n) => p + n))).ToList();
            var indexBest = intNumbers.FindIndex(n => n == intNumbers.Min());

            Console.WriteLine();
            Console.WriteLine("Final:");
            Console.WriteLine();
            for (var i = 0; i < alternativeVector.Count; i++)
            {
                var vector = alternativeVector[i].Select(v => v.ToString()).Aggregate((p, n) => p + " " + n);
                var number = indexes[i].Select(n => n.ToString()).Aggregate((p, n) => p + " " + n);
                var sortNumber = sortNumbers[i].Select(n => n.ToString()).Aggregate((p, n) => p + " " + n);

                if (i == indexBest)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{i + 1}:\t\t{vector}\t\t{number}\t\t{sortNumber}");
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }

                Console.WriteLine($"{i + 1}:\t\t{vector}\t\t{number}\t\t{sortNumber}");
            }

            Console.WriteLine();
            // vectors.ForEach( v => Console.Write($"\t{v}"));
        }
    }
}