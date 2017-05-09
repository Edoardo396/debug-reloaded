using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Linq;
using DebugReloaded.Commands;
using DebugReloaded.Commands.AssemblyCommands;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded.Interface {
    /// <summary>
    /// Interprete di comandi di ^debug
    /// </summary>
    public class CommandInterpreter {
        private readonly ApplicationContext context;

        public CommandInterpreter(ApplicationContext context) {
            this.context = context;
        }

        /// <summary>
        /// Continua a chiedere ed eseguire comandi
        /// </summary>
        public void WaitForCommands() {
            while (true) {
                Console.Write("-");
                string command = Console.ReadLine();
                this.ExecuteCommand(new DebugCommand(command));
            }
        }

        /// <summary>
        /// Esegue il comando specificato
        /// </summary>
        /// <param name="debugCommand">Comando da eseguire</param>
        /// <param name="inputs">Eventuali parametri (input per debug e tests)</param>
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
                case "t":
                    if (inputs.Length != 0)
                        this.TCommand(debugCommand.Parameters, inputs[0], inputs[0]);
                    else
                        this.TCommand(debugCommand.Parameters);
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

        private void TCommand(List<string> debugCommandParameters, params string[] input) {
            int startLocation;
            try {
                if (debugCommandParameters.Count == 0)                 
                    startLocation =
                        //BitConverter.ToInt32( MySupport.Normalize(Context.GetRegisterByName("ip").Value.Reverse().ToArray()), 0);
                        Convert.ToInt32(MySupport.Normalize(context.GetRegisterByName("ip").Value.ToArray()).ToHexString(), 16);
                else {
                    startLocation = Convert.ToInt32(debugCommandParameters[0], 16);
                    context.GetRegisterByName("ip")
                        .SetValue(MySupport.Normalize(BitConverter.GetBytes(startLocation).Reverse().ToArray()));
                }

                AssemblableCommand nextCommand = Disassembler.DisassembleNextCommand(context,
                    context.MainMemory.ExtractMemoryPointer(
                        Convert.ToInt32(
                            MySupport.NormlizeForHex(context.GetRegisterByName("ip").Value).ToArray().ToHexString(), 16),
                        16));

                AssemblyExecutableCommand cmd = AssemblyExecutableCommand.GetCommandFromName(nextCommand.ToString(),
                    context);

                context.Registers[context.Registers.FindIndex(e => e.Name == "ip")] += new byte[]
                    {0x0, (byte) nextCommand.Length};


                cmd.Execute();

                this.RCommand(null);
            } catch (Exception e) {
                ConsoleLogger.Write("Error: " + e.Message + " CLASS: " + e.ToString(), "ERROR", ConsoleColor.Red);
            }
        }


        private void UCommand(IReadOnlyList<string> debugCommandParameters, params string[] input) {
            if (debugCommandParameters.Count != 1)
                return;
            int index = int.Parse(debugCommandParameters[0]);

            MemoryRangePointer pointer = context.MainMemory.ExtractMemoryPointer(index, 50);

            List<AssemblableCommand> cmds = Disassembler.MultiCommandDisassembler(context, pointer);

            foreach (AssemblableCommand cmd in cmds) Console.WriteLine(cmd.ToString());
        }

        private void ACommand(IReadOnlyList<string> parameters, params string[] inputs) {
            if (parameters.Count != 1)
                return;

            int index = int.Parse(parameters[0], NumberStyles.HexNumber);

            while (true) {
                string buffer = MySupport.CWR($"{index:X4} => ");

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

        private void ECommand(IReadOnlyList<string> parameters, params string[] inputs) {
            if (parameters.Count == 0) {
                Console.WriteLine("You need to specify the location");
                return;
            }

            if (parameters.Count != 1) return;

            int location = int.Parse(parameters[0], NumberStyles.HexNumber);

            Console.Write($"0x{location:X4} : {context.MainMemory.Dump(location, 1)} => ");

            try {
                context.MainMemory.SetValues(location,
                    MySupport.GetBytesArrayFromString(inputs.Length == 0 ? Console.ReadLine() : inputs[0]));
            } catch (Exception e) {
                Console.WriteLine("Error while inserting data: " + e.Message);
            }
        }

        private void DCommand(IReadOnlyList<string> parameters, params string[] inputs) {
            if (parameters.Count == 0) {
                Console.WriteLine("You need to specify the location");
                return;
            }

            if (parameters.Count != 1) return;

            int location = int.Parse(parameters[0], NumberStyles.HexNumber);

            for (var i = 0; i < 16; ++i)
                this.PrintLocationMemory(ref location);
        }

        // BUG Problem with set
        private void RCommand(List<string> parameters, params string[] inputs) {
            if (parameters == null || parameters.Count == 0) {
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

                    var stringaInput = inputs.Length == 0 ? MySupport.CWR(":") : inputs[0];
                    MySupport.NormalizeValueString(ref stringaInput);
                    reg.SetValue(stringaInput);

                } catch (Exception e) {
                    Console.WriteLine("Unable to set register value, " + e.Message);
                }
            }
        }


        /// <summary>
        /// Scrive 16 bytes di memoria
        /// </summary>
        /// <param name="loc">Byte inizuiale</param>
        private void PrintLocationMemory(ref int loc) {
            Console.Write($"{loc}h" + "   ");

            for (var i = 0; i < 16; i++) {
                Console.Write($"{context.MainMemory[loc]:X2}  ");
                loc++;
            }

            Console.WriteLine();
        }
    }
}