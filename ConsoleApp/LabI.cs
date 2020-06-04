using System;
using System.Collections.Generic;
using System.Linq;
using LAB2;
using LAB2.Extensions;

namespace ConsoleApp
{
    public class LabI : ILab
    {
        public void Run()
        {
            List<Criterion> criteria = new List<Criterion>
            {
                /* 1 */ new Criterion("Programming language", "C#", "Java", "C++", "Golang", "PHP"),
                /* 2 */ new Criterion("Database", "SQLServer", "Oracle", "SQLLite", "MySql", "MongoDB"),
                /* 3 */ new Criterion("UI Framework", "Angular", "React", "Bootstrap", "AngularJs", "Vue.js"),
                /* 4 */ new Criterion("Architect", "Layers", "Client–Server", "MVC", "WEB API", "Monolith"),
                /* 5 */ new Criterion("Deploy", "Jenkins", "Docker", "Kubernetes", "Ansible", "TeamCity"),
                /* 6 */ new Criterion("Agile Methodology", "Scrum", "XP", "Crystal", "DSDM", "FDD"),
                /* 7 */ new Criterion("Testing Tool", "Cucumber", "JUnit", "Selenium", "TestNG", "Appium"),
                /* 8 */ new Criterion("Deploy platform", "Azure", "Amazon", "Google", "Heroku", "Netlify"),
                /* 9 */ new Criterion("ML Lang", "GO Lang", "Python", "C++", "Javascript", "R"),
                /* 10 */ new Criterion("Platform", "Cross", "Web", "Windows", "Mac OS", "Linux"),
                /* 11 */ new Criterion("Manage App", "Bitrix 24", "Wrike", "Asana", "Basecamp", "Trello"),
                /* 12 */ new Criterion("Repository", "GitHub", "Bitbucket", "GitLab", "Assembla", "Backlog"),
                /* 13 */ new Criterion("IDE", "IDEA", "Visual Studio", "Xcode", "NetBeans", "Eclipse"),
                /* 14 */ new Criterion("Realtime connection", "Persistence conn", "Hub conn", "Graph QL", "WebSockets", "Pear"),
                /* 15 */ new Criterion("UI style", "Flat", "Usual", "Cude", "Lights", "GNU"),
            };

            var alternatives = criteria.GetAllAlternatives();
            Console.WriteLine($"All alternatives: {alternatives.Count}");
            Console.WriteLine("First five alternatives:");
            alternatives.Take(5).ToList().ForEach(Console.WriteLine);
            // alternatives.Print();
            Console.WriteLine();
            var (best, worse) = alternatives.GetTheBestAndTheWorseAlternative();
            Console.WriteLine("The best alternative: " + best.ToString());
            Console.WriteLine("The worst alternative: " + worse.ToString());
            Console.WriteLine();
            var index = new Random().Next(0, alternatives.Count);
            var better = alternatives.GetBetterAlternatives(alternatives[index]);
            Console.WriteLine("Better alternatives then " + alternatives[index] + $"\n Count: {better.Count}");
            // better.Print();
            Console.WriteLine();
            var worseList = alternatives.GetWorseAlternatives(alternatives[index]);
            Console.WriteLine("Worse alternatives then " + alternatives[index] + $"\n Count: {worseList.Count}");
            // worseList.Print();
            Console.WriteLine();
            var notCompatible = alternatives.GetNotComparableAlternatives(alternatives[index]);
            Console.WriteLine("Not compatible alternatives then " + alternatives[index] + $"\n Count: {notCompatible.Count}");
            // notCompatible.Print();
        }
    }
}