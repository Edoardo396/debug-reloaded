using System;
using System.Linq;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded.Commands.AssemblyCommands {
    public class ADDExecutableCommand : AssemblyExecutableCommand {

        public ADDExecutableCommand(ApplicationContext ctx, string instruct) : base(ctx, instruct) {
        }

        public override void Execute() {
            var parameters = this.GetParamsMemorizables();

            int[] param = new int[2];

            for (var i = 0; i < parameters.Length; i++) 
                param[i] = Convert.ToInt32(parameters[i].GetValues(0, 2).ToHexString(),16);

            param[0] += param[1];

            parameters[0].SetValues(0, MySupport.Normalize(BitConverter.GetBytes(param[0]).Reverse().ToArray()));

            base.SetResultToFlags(result: param[0]);
        }
    }
}