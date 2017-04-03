using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReloaded.Support {
    public static class MySupport {
        public static string CWR(object write, bool line = false) {
            if (line)
                Console.WriteLine(write.ToString());
            else
                Console.Write(write.ToString());
            return Console.ReadLine();
        }

        public static void NormalizeValueString(ref string str) {

            if (str.Length % 4 == 0)
                return;

            while (str.Length % 4 != 0)
                str = "0" + str;

        }

        public static byte[] GetBytesArrayFromString(string sbytes) {
            return
                Enumerable.Range(0, sbytes.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(sbytes.Substring(x, 2), 16))
                    .ToArray();
        }
    }
}