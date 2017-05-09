using DebugReloaded.Support;

namespace DebugReloaded.Commands.AssemblyCommands {
    public class JNZExecutableCommand : AssemblyExecutableCommand {
        public JNZExecutableCommand(ApplicationContext ctx, string instruct) : base(ctx, instruct) {
        }

        public override void Execute() {

            var memorizables = base.GetParamsMemorizables();

            if(Context.GetFlagByName("zf").Value)
                this.Context.GetRegisterByName("ip").SetValue(memorizables[0].GetValues(0, 2)); // Perform jumping if zero flag is set.

        }
    }
}