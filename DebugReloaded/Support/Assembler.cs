using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Commands;
using DebugReloaded.Containers;

namespace DebugReloaded.Support {
    public sealed class Assembler {

        public List<Command> Commands { get; set; }

        private MemoryRangePointer CodeSegment;

        public Assembler() {
            Commands = new List<Command>();
        }

        public Assembler(List<Command> commands, MemoryRangePointer CodeSegment) : this() {
            Commands = commands;
        }

        public void Assemble() {

        }

    }
}
