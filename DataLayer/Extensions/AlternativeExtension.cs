using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace LAB2.Extensions
{
    public static class AlternativeExtension
    {

        public static (Alternative best, Alternative worse) GetTheBestAndTheWorseAlternative(this List<Alternative> alternatives)
        {
            return (alternatives[0], alternatives[^1]);
        }

        public static List<Alternative> GetBetterAlternatives(this List<Alternative> alternatives, Alternative findAlternative)
        {
            var better = new List<Alternative>();
            var index = alternatives.IndexOf(findAlternative);
            if (index == -1)
            {
                // throw new Exception("Alternative does not find!");
                index = alternatives.Count;
            }

            for (var i = 0; i < index; ++i)
            {
                if (IsBetterAlternative(alternatives[i], findAlternative))
                {
                    better.Add(alternatives[i]);
                }
            }

            return better;
        }

        public static List<Alternative> GetWorseAlternatives(this List<Alternative> alternatives, Alternative findAlternative)
        {
            var worse = new List<Alternative>(); 
            var index = alternatives.IndexOf(findAlternative);
            if (index == -1) {
                // throw new Exception("Alternative does not found!");
                index = -1;
            }
            for (var i = index + 1; i < alternatives.Count; ++i) {
                if (IsWorseAlternative(alternatives[i], findAlternative))
                {
                    worse.Add(alternatives[i]);
                }
            }

            return worse;
        }

        public static List<Alternative> GetNotComparableAlternatives(this List<Alternative> alternatives, Alternative findAlternative)
        {
            var notCompatibleAlts = alternatives.Except(alternatives.GetBetterAlternatives(findAlternative)).Except(alternatives.GetWorseAlternatives(findAlternative)).ToList();
            notCompatibleAlts.Remove(findAlternative);
            return notCompatibleAlts;
        }
        
        private static bool IsWorseAlternative(Alternative alternative, Alternative alternative2)
        {
            for (var i = 0; i < alternative.AlternativeValues.Count; ++i)
            {
                var position = alternative.AlternativeValues[i].Value.Index;
                var position2 = alternative2.AlternativeValues[i].Value.Index;
                if (position < position2)
                {
                    return false;
                }
            }

            return true;
        }
        
        private static bool IsBetterAlternative(Alternative alternative, Alternative alternative2)
        {
            for (var i = 0; i < alternative.AlternativeValues.Count; ++i)
            {
                var position = alternative.AlternativeValues[i].Value.Index;
                var position2 = alternative2.AlternativeValues[i].Value.Index;
                if (position > position2)
                {
                    return false;
                }
            }

            return true;
        }

        public static double GetD(this Alternative alternative, List<double> center)
        {
            double result = 0;
            for (var i = 0; i < alternative.AlternativeValues.Count; i++)
            {
                result += Math.Abs(alternative.AlternativeValues[i].Value.Index - center[i]);
            }
            return result;
        }
        
    }
}