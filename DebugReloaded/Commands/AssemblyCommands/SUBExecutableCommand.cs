using System;
using System.Linq;
using DebugReloaded.Support;

namespace DebugReloaded.Commands.AssemblyCommands {
    public class SUBExecutableCommand : AssemblyExecutableCommand {
        public SUBExecutableCommand(ApplicationContext ctx, string instruct) : base(ctx, instruct) {
        }

        public override void Execute() {

            var memorizables = base.GetParamsMemorizables();


            int[] param = new int[2];

            for (var i = 0; i < memorizables.Length; i++)
                param[i] = Convert.ToInt32(memorizables[i].GetValues(0, 2).ToHexString(), 16);

            param[0] -= param[1];

            memorizables[0].SetValues(0, MySupport.Normalize(BitConverter.GetBytes(param[0]).Reverse().ToArray()));

            base.SetResultToFlags(result: param[0]);
        }
    }
}