using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Genesis.Ambience.Controls
{
    static class TimeFormatter
    {
        public static string Format(this TimeSpan ts, int ticksPerSec, string fmt)
        {
            List<int> values = new List<int>();
            values.Add(ts.Milliseconds); // z 0
            values.Add((int)ts.TotalMilliseconds); // Z 1
            values.Add(ts.Seconds); // s 2
            values.Add((int)ts.TotalSeconds); // S 3
            values.Add(ts.Minutes); // m 4
            values.Add((int)ts.TotalMinutes); // M 5
            values.Add(ts.Hours); // h 6
            values.Add((int)ts.TotalHours); // H 7
            values.Add((int)(ticksPerSec * ts.TotalSeconds)); // t 8
            List<char> trans = new List<char>("zZsSmMhHt".ToCharArray());
            
            for (int c = 0; c < fmt.Length; c++)
            {
                if (fmt[c] == '\\' && c != fmt.Length - 1)
                {
                    string before = fmt.Substring(0, c);
                    string after = fmt.Substring(c + 2);
                    string insert = fmt.Substring(c + 1, 1);
                    fmt = before + insert + after;
                }
                else
                {
                    if (trans.Contains(fmt[c]))
                    {
                        int n = 1;
                        while (c + n < fmt.Length && fmt[c + n] == fmt[c])
                            n++;

                        string before = fmt.Substring(0, c);
                        string after = fmt.Substring(c + n);
                        string insert = string.Format("{0}{1}{2}{3}", "{",
                            trans.IndexOf(fmt[c]), (n > 1) ? (":".PadRight(n + 1, '0')) : "", "}");
                        fmt = before + insert + after;
                        c += insert.Length - 1;
                    }
                }
            }

            return string.Format(fmt, Array.ConvertAll<int, object>(values.ToArray(), (n) => { Int32 nn = n; return nn; }));
        }
    }
}
