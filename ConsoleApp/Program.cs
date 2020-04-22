using System;
using System.Collections.Generic;
using LAB2;
using LAB2.Extensions;

namespace ConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            // new LabI().Run();
            new LabII().Run();
        }
        
    }
    public interface ILab
    {
        void Run();
    }
}