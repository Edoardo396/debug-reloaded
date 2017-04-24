using System;
using DebugReloaded.Interface;
using DebugReloadedCore.Support;

namespace DebugReloaded.Commands.AssemblyCommands {
    public class MOVExecutableCommand : AssemblyExecutableCommand {
        public MOVExecutableCommand(ApplicationContext ctx, string instruct) : base(ctx, instruct) {
        }
        public override void Execute() {

            var parameters = base.GetParamsMemorizables();

            parameters[0].SetValues(0, parameters[1].GetValues(0, parameters[0].Length));
        }

    }
}