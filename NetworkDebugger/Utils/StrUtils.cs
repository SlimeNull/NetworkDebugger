using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDebugger.Utils
{
    internal class StrUtils
    {
        public static string EscapeString(string source)
        {
            StringBuilder sb =
                new StringBuilder(source.Length);

            bool escape = false;
            foreach (char i in source)
            {
                if (escape)
                    sb.Append(i switch
                    {
                        't' => '\t',
                        'r' => '\r',
                        'n' => '\n',
                        _ => i
                    });
                else if (i == '\\')
                    escape = true;
                else
                    sb.Append(i);
            }

            return sb.ToString();
        }
    }
}
