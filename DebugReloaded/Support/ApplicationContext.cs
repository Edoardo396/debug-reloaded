using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Commands;
using DebugReloaded.Containers;
using DebugReloaded.Interface;

namespace DebugReloaded.Support {
    public class ApplicationContext {
        public static readonly int memSize = 65535;

        public Memory mainMemory = new Memory(memSize);

        public CommandInterpreter Interpreter;

        public List<Register> Registers { get; } = new List<Register>() {
            new Register("ax"),
            new Register("bx"),
            new Register("cx"),
            new Register("dx"),
            new Register("si"),
            new Register("di"),
            new Register("cs"),
            new Register("ds")
        };

        public List<Command> Program = new List<Command>();

        public Register GetRegisterByName(string name) {
            return Registers.Find(r => r.Name == name);
        }

        public ApplicationContext() {
            Interpreter = new CommandInterpreter(this);
        }       
    }
}