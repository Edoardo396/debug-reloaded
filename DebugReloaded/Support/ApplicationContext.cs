using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Containers;

namespace DebugReloaded.Support {
    public class ApplicationContext {

        public static readonly int memSize = 65535;

        Register ax = new Register("ax");

        Register bx = new Register("bx");

        Register cx = new Register("cx");

        Register dx = new Register("dx");

        Register ds = new Register("ds");

        Register ip = new Register("ip");

        Memory mainMemory = new Memory(memSize);

    }
}