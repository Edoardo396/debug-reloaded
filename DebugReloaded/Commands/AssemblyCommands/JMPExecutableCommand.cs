using DebugReloaded.Support;

namespace DebugReloaded.Commands.AssemblyCommands {
    public class JMPExecutableCommand : AssemblyExecutableCommand {
        public JMPExecutableCommand(ApplicationContext ctx, string instruct) : base(ctx, instruct) {
        }

        public override void Execute() {

            var parameters = this.GetParamsMemorizables();

            this.context.GetRegisterByName("ip").SetValue(parameters[0].GetValues(0, 2));
        }
    }
}