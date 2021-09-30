using System;
using System.Collections.Generic;
using System.Linq;
using VNet.Utility;

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

                    if (t.IsNumber() ||
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
    }
}
