using System;
using System.Collections.Generic;
using DebugReloaded.Commands;
using DebugReloadedCore.Containers;
using DebugReloadedCore.Support;

namespace DebugReloaded.Interface {
    public class CommandInterpreter {
        private readonly ApplicationContext context;

        public CommandInterpreter(ApplicationContext context) {
            this.context = context;
        }

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
                        this.RCommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        this.RCommand(debugCommand.Parameters);
                    break;

                case "d":
                    if (inputs.Length != 0)
                        this.DCommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        this.DCommand(debugCommand.Parameters);
                    break;
                case "e":
                    if (inputs.Length != 0)
                        this.ECommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        this.ECommand(debugCommand.Parameters);
                    break;
                case "a":
                    if (inputs.Length != 0)
                        this.ACommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        this.ACommand(debugCommand.Parameters);
                    break;
                case "g":
                    if (inputs.Length != 0)
                        this.GCommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        this.GCommand(debugCommand.Parameters);
                    break;
                case "u":
                    if (inputs.Length != 0)
                        this.UCommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        this.UCommand(debugCommand.Parameters);
                    break;
            }
        }

        private void GCommand(List<string> debugCommandParameters, params string[] input) {
        }

        private void UCommand(List<string> debugCommandParameters, params string[] input) {
            if (debugCommandParameters.Count != 1)
                return;

            int index = int.Parse(debugCommandParameters[0]);

            MemoryRangePointer pointer = context.MainMemory.ExtractMemoryPointer(index, 50);

            List<AssemblableCommand> cmds = Disassembler.MultiCommandDisassembler(context, pointer);

            foreach (AssemblableCommand cmd in cmds) Console.WriteLine(cmd.ToString());
        }

        private void ACommand(List<string> parameters, params string[] inputs) {
            if (parameters.Count != 1)
                return;

            int index = int.Parse(parameters[0]);

            while (true) {
                string buffer = MySupport.CWR($"{index} => ");

                if (buffer == string.Empty)
                    break;

                byte[] bytes;

                try {
                    bytes = context.CommandAssembler.Assemble(buffer);
                    context.MainMemory.SetValues(index, bytes);
                } catch (Exception e) {
                    Console.WriteLine("Errore: " + e.Message);
                    continue;
                }

                index += bytes.Length;
            }
        }

        private void ECommand(List<string> parameters, params string[] inputs) {
            if (parameters.Count == 0) {
                Console.WriteLine("You need to specify the location");
                return;
            }

            if (parameters.Count != 1) return;

            int location = int.Parse(parameters[0]);

            Console.Write($"{location}h : {context.MainMemory.Dump(location, 1)} => ");

            try {
                context.MainMemory.SetValues(location,
                    MySupport.GetBytesArrayFromString(inputs.Length == 0 ? Console.ReadLine() : inputs[0]));
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

            for (var i = 0; i < 16; ++i)
                this.PrintLocationMemory(ref location);
        }

        private void PrintLocationMemory(ref int loc) {
            Console.Write($"{loc}h" + "   ");

            for (var i = 0; i < 16; i++) {
                Console.Write($"{context.MainMemory[loc]:X2}  ");
                loc++;
            }

            Console.WriteLine();
        }

        private void RCommand(List<string> parameters, params string[] inputs) {
            if (parameters.Count == 0) {
                foreach (Register register in context.Registers)
                    Console.Write($"{register.Name.ToUpper()}={register}   ");
                Console.WriteLine();
                return;
            }

            if (parameters.Count == 1) {
                Register reg = context.GetRegisterByName(parameters[0]);

                if (reg == null) {
                    Console.WriteLine("Register not found.");
                    return;
                }

                Console.WriteLine($"{reg.Name.ToUpper()} {reg}");
                try {
                    reg.SetValue(inputs.Length == 0 ? MySupport.CWR(":") : inputs[0]);
                } catch (Exception e) {
                    Console.WriteLine("Unable to set register value, " + e.Message);
                }
            }
        }
    }
}