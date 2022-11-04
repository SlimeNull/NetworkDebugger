using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDebugger.Utils
{
    internal static class EncodingUtils
    {
        public static Encoding[] AllSupportedEncoding { get; }
        
        static EncodingUtils()
        {
            EncodingInfo[] encodingInfos = Encoding.GetEncodings();
            Encoding[] allEncodings = new Encoding[encodingInfos.Length];

            for (int i = 0; i < allEncodings.Length; i++)
                allEncodings[i] = encodingInfos[i].GetEncoding();

            for (int i = 0; i < allEncodings.Length; i++)
            {
                Encoding encoding = allEncodings[i];
                if (encoding.CodePage == 20127)
                    (allEncodings[0], allEncodings[i]) = (allEncodings[i], allEncodings[0]);
                else if (encoding.CodePage == 1200)
                    (allEncodings[1], allEncodings[i]) = (allEncodings[i], allEncodings[1]);
                else if (encoding.CodePage == 65001)
                    (allEncodings[2], allEncodings[i]) = (allEncodings[i], allEncodings[2]);
                else if (encoding.CodePage == 936)
                    (allEncodings[3], allEncodings[i]) = (allEncodings[i], allEncodings[3]);
            }

            AllSupportedEncoding = allEncodings;
        }
    }
}
