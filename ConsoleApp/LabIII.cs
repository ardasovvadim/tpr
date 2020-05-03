using System;
using System.Collections.Generic;
using System.Linq;
using LAB2;
using LAB2.Extensions;

namespace ConsoleApp
{
    public class LabIII : ILab
    {
        public void Run()
        {
            var criteria = new List<Criterion>
            {
                new Criterion("Distance", "Small", "Medium", "Large"),
                new Criterion("Salary", "Large", "Medium", "Small"),
                new Criterion("Vacation", "Large", "Medium", "Small")
            };
            var criteria2 = new List<Criterion>
            {
                new Criterion("K1", "K1_1", "K1_2", "K1_3", "K1_4"),
                new Criterion("K2", "K2_1", "K2_2", "K2_3", "K2_4"),
                new Criterion("K3", "K3_1", "K3_2", "K3_3", "K3_4"),
                new Criterion("K4", "K4_1", "K4_2", "K4_3", "K4_4"),
            };
            var criteria3 = new List<Criterion>
            {
                new Criterion("Programming language", "C#", "Java", "C++", "Golang", "PHP"),
                new Criterion("Database", "SQLServer", "Oracle", "SQLLite", "MySql", "MongoDB"),
                new Criterion("UI Framework", "Angular", "React", "Bootstrap", "AngularJs", "Vue.js"),
                new Criterion("Architect", "Layers", "Client–Server", "MVC", "WEB API", "Monolith"),
                new Criterion("Deploy", "Jenkins", "Docker", "Kubernetes", "Ansible", "TeamCity"),
            };
            var alternativesVector3 = new List<List<int>>
            {
                new List<int> {1, 2, 3, 4, 5},
                new List<int> {2, 3, 4, 5, 1},
                new List<int> {3, 4, 5, 1, 2},
                new List<int> {4, 5, 1, 2, 3},
                new List<int> {5, 1, 2, 3, 4},
            };
            var alternativesVector = new List<List<int>>
            {
                new List<int> {1, 2, 3},
                new List<int> {2, 3, 1},
                new List<int> {3, 1, 2}
            };
            var alternativesVector2 = new List<List<int>>
            {
                new List<int> {1, 2, 3, 4},
                new List<int> {2, 4, 1, 3},
                new List<int> {3, 1, 4, 2}
            };
            var allAlternatives = criteria.GetAllAlternatives();
            var alternatives = alternativesVector.Select(vector => allAlternatives.GetAlternativeByVector(vector)).ToList();

            var matrix = new CompareMatrix(criteria3);
            Console.WriteLine();
            Console.WriteLine(matrix.VectorToString());
            Console.WriteLine();
            Console.WriteLine(matrix);
            matrix.Compare();
            Console.WriteLine();
            matrix.BuildWayVectors();
            matrix.BuildUnionWayVectors();
            matrix.SortAlternatives(alternativesVector3);
        }
    }
}