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

                case "d":
                    if (inputs.Length != 0)
                        DCommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        DCommand(debugCommand.Parameters);
                    break;
                case "e":
                    if (inputs.Length != 0)
                        ECommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        ECommand(debugCommand.Parameters);
                    break;
            }
        }

        private void ECommand(List<string> parameters, params string[] inputs) {
            if (parameters.Count == 0) {
                Console.WriteLine("You need to specify the location");
                return;
            }

            if (parameters.Count != 1) return;

            int location = int.Parse(parameters[0]);

            Console.Write($"{location}h : {context.mainMemory.Dump(location,1)} => ");

            try {
                context.mainMemory.SetValues(location, MySupport.GetBytesArrayFromString(inputs.Length == 0 ? Console.ReadLine() : inputs[0]));
            } catch (Exception e) {
                Console.WriteLine("Error while inserting data: " + e.Message);
            }
        }

        private void DCommand(List<string> parameters, params string[] inputs) {
            if (parameters.Count == 0) {
                Console.WriteLine("You need to specify the location");
                return;
            }

            if (parameters.Count != 1) return;

            int location = int.Parse(parameters[0]);

            for (int i = 0; i < 16; ++i)
                this.PrintLocationMemory(ref location);
        }

        private void PrintLocationMemory(ref int loc) {
            Console.Write($"{loc}h" + "   ");

            for (int i = 0; i < 16; i++) {
                Console.Write($"{context.mainMemory[loc]:X2}  ");
                loc++;
            }

            Console.WriteLine();
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
                    reg.SetValue(inputs.Length == 0 ? MySupport.CWR(":") : inputs[0]);
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