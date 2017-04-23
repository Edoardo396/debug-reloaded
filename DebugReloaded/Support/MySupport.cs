using System;
using System.Linq;

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

        public static string ByteArrayToString(byte[] ba) {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static byte[] GetBytesArrayFromString(string sbytes) {
            return
                Enumerable.Range(0, sbytes.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(sbytes.Substring(x, 2), 16))
                    .ToArray();
        }

        public static string BetweenSubstring(this string s, string del1, string del2) {

            int ind1 = s.IndexOf(del1) + 1, ind2 = s.IndexOf(del2) - 1;

            if (ind1 == -1 || ind2 == -1)
                return string.Empty;

            return s.Substring(ind1, ind2 - ind1);
        }

        public static int ToInt(this string s) {
            return int.Parse(s);
        }

    }
}