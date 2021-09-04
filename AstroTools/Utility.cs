using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AstroTools
{
    public static class Utility
    {
        public static string GetTagValue(string fullString, string tag)
        {
            List<string> result = new List<string>();
            string retVal = string.Empty;

            string normalized = fullString.Replace(":", "").Replace(";", "").Trim();
            var tokens = normalized.Split(' ').ToList<string>();
            tokens.RemoveAll(t => t.Length == 0);

            int startIdx = tokens.FindIndex(t => t.Trim() == tag.Trim());
            

            if (startIdx >= 0)
            {
                int idx = startIdx + 1;
                bool done = false;

                while(idx < tokens.Count && !done)
                {
                    string t = tokens[idx].Trim();

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
            int tempInt;
            bool result = false;

            if (int.TryParse(s, out tempInt))
            {
                result = true;
            }

            return result;
        }
    }
}
