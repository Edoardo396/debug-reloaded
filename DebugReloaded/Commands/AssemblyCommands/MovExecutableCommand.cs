using System;
using DebugReloaded.Containers;
using DebugReloaded.Interface;
using DebugReloaded.Support;

namespace DebugReloaded.Commands.AssemblyCommands {
    public class MovExecutableCommand : AssemblyExecutableCommand {
        public MovExecutableCommand(ApplicationContext ctx) : base(ctx) {
        }

        public override string Name => "mov";
        public override void Execute(IMemorizable[] par) { 
            
            par[0].SetValues(0, par[1].GetValues(0, par[0].Length));
        }

    }
}