using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded.Interface {
    public class CommandInterpreter {
        private ApplicationContext context;

        public void WaitForCommands() {
            while (true) {
                Console.Write("-");
                string command = Console.ReadLine();
                this.ExecuteCommand(new DebugCommand(command));
            }
        }

        public void ExecuteCommand(DebugCommand debugCommand, params string[] inputs) {
            switch (debugCommand.CommandString) {
                case "r":
                    if (inputs.Length != 0)
                        RCommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        RCommand(debugCommand.Parameters);
                    break;
                /*
            case "d":
                DCommand(debugCommand.Parameters);
                break;
            case "e":
                ECommand(debugCommand.Parameters);
                break;
            case "a":
                ACommand(debugCommand.Parameters);
                break;
                 */
            }
        }

        private void RCommand(List<string> parameters, params string[] inputs) {
            if (parameters.Count == 0) {
                foreach (Register register in context.Registers)
                    Console.Write($"{register.Name.ToUpper()}={register.ToString()}   ");
                Console.WriteLine();
                return;
            }

            if (parameters.Count == 1) {
                Register reg = context.GetRegisterByName(parameters[0]);

                if (reg == null) {
                    Console.WriteLine("Register not found.");
                    return;
                }

                Console.WriteLine($"{reg.Name.ToUpper()} {reg.ToString()}");
                try {
                    reg.SetValue(inputs.Length == 0 ? WriteSupport.CWR(":") : inputs[0]);
                } catch (Exception e) {
                    Console.WriteLine("Unable to set register value, " + e.Message);
                }
            }
        }


        public CommandInterpreter(ApplicationContext context) {
            this.context = context;
        }
    }
}