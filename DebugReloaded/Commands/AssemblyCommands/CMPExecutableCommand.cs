using System;
using DebugReloaded.Support;

namespace DebugReloaded.Commands.AssemblyCommands {
    public class CMPExecutableCommand : AssemblyExecutableCommand {
        public CMPExecutableCommand(ApplicationContext ctx, string instruct) : base(ctx, instruct) {
        }

        public override void Execute() {

            var memorizables = base.GetParamsMemorizables();

            int[] param = new int[2];

            for (var i = 0; i < memorizables.Length; i++)
                param[i] = Convert.ToInt32(memorizables[i].GetValues(0, 2).ToHexString(), 16);

            param[0] -= param[1];

            base.SetResultToFlags(result: param[0]);
        }
    }
}