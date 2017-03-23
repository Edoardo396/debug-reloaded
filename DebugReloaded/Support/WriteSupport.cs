using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReloaded.Support {
    public static class WriteSupport {

        public static string CWR(object write, bool line = false) {
            if (line)
                Console.WriteLine(write.ToString());
            else
                Console.Write(write.ToString());
            return Console.ReadLine();
        }
    }
}