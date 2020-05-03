using System;
using System.Collections.Generic;
using LAB2;

namespace ConsoleApp
{
    public class LabIV : ILab
    {
        public void Run()
        {
            var criteria = new List<Criterion>
            {
                new Criterion("Programming language", "C#", "Java", "C++", "Golang", "PHP"),
                new Criterion("Database", "SQLServer", "Oracle", "SQLLite", "MySql", "MongoDB"),
                new Criterion("UI Framework", "Angular", "React", "Bootstrap", "AngularJs", "Vue.js"),
                new Criterion("Architect", "Layers", "Client–Server", "MVC", "WEB API", "Monolith"),
                new Criterion("Deploy", "Jenkins", "Docker", "Kubernetes", "Ansible", "TeamCity"),
                new Criterion("Agile Methodology", "Scrum", "XP", "Crystal", "DSDM", "FDD"),
                new Criterion("Testing Tool", "Cucumber", "JUnit", "Selenium", "TestNG", "Appium"),
            };
            var alternativesVector = new List<List<int>>
            {
                new List<int> {2, 2, 1, 3, 2, 5, 1},
                new List<int> {3, 1, 3, 2, 4, 3, 2},
            };
            var findBestAlt = new FindBestAlt(criteria, alternativesVector, null);
            var alt = findBestAlt.FindTheBest();

            Console.WriteLine($"\nThe best alternative: {alt}");
        }
    }
}