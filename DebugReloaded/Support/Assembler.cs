using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Commands;
using DebugReloaded.Containers;

namespace DebugReloaded.Support {
    public sealed class Assembler {

        public class AssemblerCommand {
            
            




        }

        public List<CommandTemplate> Commands { get; set; }

        private MemoryRangePointer CodeSegment;

        public Assembler() {
            Commands = new List<CommandTemplate>();
        }

        public Assembler(List<CommandTemplate> commands, MemoryRangePointer CodeSegment) : this() {
            Commands = commands;
        }

        public void Assemble() {

        }

    }
}
