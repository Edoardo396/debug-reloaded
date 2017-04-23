using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Commands.AssemblyCommands;
using DebugReloaded.Support;
using static DebugReloaded.Commands.AssemblyCommands.AssemblyExecutableCommand;

namespace DebugReloaded.Commands {
    public static class Executer {

        public static void Execute(string str, ApplicationContext ctx) {

   //         AssemblableCommand cmd = new AssemblableCommand(ctx, str);

            AssemblyExecutableCommand cmd = GetCommandFromName(GetCommandNameFromString(str), ctx);





        }


    }
}
