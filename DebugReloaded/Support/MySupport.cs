using System;
using System.Collections.Generic;
using System.Linq;

namespace DebugReloaded.Support {
    public static class MySupport {

        /// <summary>
        /// Scrive e legge dalla console
        /// </summary>
        /// <param name="write">Testo da scrivere</param>
        /// <param name="line">inserire \n alla fine?</param>
        /// <returns>Lettra da console</returns>
        public static string CWR(object write, bool line = false) {
            if (line)
                Console.WriteLine(write.ToString());
            else
                Console.Write(write.ToString());
            return Console.ReadLine();
        }

        /// <summary>
        /// Aggiunge degli zeri all'inizio della stringa finchè non diventa lunghezza mult. di 4
        /// </summary>
        /// <param name="str">Riferimento alla stringa</param>
        public static void NormalizeValueString(ref string str) {
            while (str.Length % 4 != 0)
                str = "0" + str;
        }

        /// <summary>
        /// Converte array di byte in string
        /// </summary>
        /// <param name="ba"></param>
        /// <returns>La stringa</returns>
        public static string ByteArrayToString(byte[] ba) {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        /// <summary>
        /// Converte array di byte in stringa
        /// </summary>
        /// <param name="arr">(this)</param>
        /// <returns>Valore convertito</returns>
        public static string ToHexString(this byte[] arr) {
            return ByteArrayToString(arr);
        }

        /// <summary>
        /// Converte la stringa in un array di byte (HEX)
        /// </summary>
        /// <param name="sbytes">Stirnga</param>
        /// <returns>Array di bytes</returns>
        public static byte[] ToByteArray(this string sbytes) {
            return GetBytesArrayFromString(sbytes);
        }

        /// <summary>
        /// Converte la stringa in un array di byte (HEX)
        /// </summary>
        /// <param name="sbytes">Stirnga</param>
        /// <returns>Array di bytes</returns>
        public static byte[] GetBytesArrayFromString(string sbytes) {
            return
                Enumerable.Range(0, sbytes.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(sbytes.Substring(x, 2), 16))
                    .ToArray();
        }

        /// <summary>
        /// Aggiunge 0 all'array fino a quando la sua lunghezza non è multipla di 4
        /// </summary>
        public static byte[] NormlizeForHex(byte[] _bytes) {
            var bytes = new List<byte>(_bytes);

            while(bytes.Count % 4 != 0)
                bytes.Insert(0, 0x0);

            return bytes.ToArray();
        }


        /// <summary>
        /// Restutuisce il testo tra 
        /// </summary> delimitatori specificati
        /// <param name="s">Stringa da cui estrarre</param>
        /// <param name="del1">Delimitatore 1</param>
        /// <param name="del2">Delimitatore 2</param>
        /// <returns></returns>
        public static string BetweenSubstring(this string s, string del1, string del2) {
            int ind1 = s.IndexOf(del1) + 1, ind2 = s.IndexOf(del2) - 1;

            if (ind1 == -1 || ind2 == -1)
                return string.Empty;

            return s.Substring(ind1, ind2 - ind1 + 1);
        }

        /// <summary>
        /// Converte stringa in intero
        /// </summary>
        public static int ToInt(this string s) {
            return int.Parse(s);
        }

        /// <summary>
        /// Restituisce un array di byte togliendo gli zeri iniziali
        /// </summary>
        public static byte[] Normalize(byte[] bytes) {

            int firstNot0Pos = -1;

            for (int i = 0; i < bytes.Length; i++) {
                if (bytes[i] == 0) continue;
                firstNot0Pos = i;
                break;
            }

            return bytes.Skip(firstNot0Pos).ToArray();
        }

    }
}