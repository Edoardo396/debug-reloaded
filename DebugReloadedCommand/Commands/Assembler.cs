using System.Collections.Generic;
using System.Linq;
using DebugReloadedCore.Support;

namespace DebugReloaded.Commands {
    public class Assembler {
        private readonly ApplicationContext context;

        public Assembler(ApplicationContext context) {
            this.context = context;
        }

        public byte[] Assemble(string instruction) {
            var cmd = new AssemblableCommand(context, instruction);
            return cmd.Assemble();
        }

        public string Disassemble(byte[] bytes) {
            List<CommandTemplate> cmd =
                context.CommandTemplList.Where(
                    c => MySupport.GetBytesArrayFromString(c.OpCode.Substring(0, 2))[0] == bytes[0]).ToList();


            return string.Empty;
        }
    }
}