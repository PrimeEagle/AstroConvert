using System;
using System.Collections.Generic;
using System.Linq;

namespace AstroTools
{
    public static class Utility
    {
        public static string GetTagValue(string fullString, string tag)
        {
            var result = new List<string>();
            var retVal = string.Empty;

            var normalized = fullString.Replace(":", "").Replace(";", "").Trim();
            var tokens = normalized.Split(' ').ToList<string>();
            tokens.RemoveAll(t => t.Length == 0);

            var startIdx = tokens.FindIndex(t => t.Trim() == tag.Trim());


            if (startIdx < 0) return retVal;
            {
                var idx = startIdx + 1;
                var done = false;

                while(idx < tokens.Count && !done)
                {
                    var t = tokens[idx].Trim();

                    if (Utility.IsNumber(t) ||
                        (tag == "Gliese" && t == "NN") ||
                        (t.Length == 1 && new string[] { "A", "B", "C", "D", "E" }.Contains(t)))
                    {
                        result.Add(t);
                    }
                    else
                    {
                        done = true;
                    }

                    idx++;
                }

                retVal = string.Join(' ', result.ToArray());
            }

            return retVal;
        }

        public static bool IsNumber(string s)
        {
            var result = int.TryParse(s, out _);

            return result;
        }

        public static string RandomDigits(int length)
        {
            var random = new Random();
            var s = string.Empty;
            for (var i = 0; i < length; i++)
                s = string.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
