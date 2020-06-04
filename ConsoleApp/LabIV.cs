using System;
using System.Collections.Generic;
using System.Linq;
using LAB2;

namespace ConsoleApp
{
    public class LabIV : ILab
    {
        public void Run()
        {
            var criteria = new List<Criterion>
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
            var alternativesVector = new List<List<int>>
            {
                new List<int> {2, 2, 1, 3, 2, 5, 1},
                new List<int> {3, 1, 3, 2, 4, 3, 2},
            };
            
            var alternativesVector2 = new List<List<int>>
            {
                new List<int> {1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5},
                new List<int> {2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1},
                new List<int> {3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2},
                new List<int> {4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3},
                new List<int> {5, 1, 2, 3, 4, 5, 1, 2, 3, 4, 5, 1, 2, 3, 4},
                new List<int> {6, 5, 3, 2, 2, 1, 5, 6, 6, 2, 1, 4, 5, 1, 1}
            };
            
            var answers = new List<int> {1, 2, 2, 1, 1};
            var answers2 = new List<int> {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2};
            var findBestAlt = new FindBestAlt(criteria, alternativesVector2, answers2);
            var alts = findBestAlt.FindTheBest().ToList();
            
        }
    }
}