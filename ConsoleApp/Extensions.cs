using System;
using System.Collections.Generic;
using LAB2;

namespace ConsoleApp
{
    public static class Extensions
    {
        public static void Print(this List<Alternative> alternatives)
        {
            int i = 0;
            alternatives.ForEach(alt =>
            {
                Console.WriteLine($"{i + 1}: " + alt.ToString());
                ++i;
            });
        }
    }
}