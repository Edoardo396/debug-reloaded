using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using DebugReloadedCore.Containers;
using DebugReloadedCore.Support;
using Microsoft.SqlServer.Server;

namespace DebugReloaded.Commands.AssemblyCommands {

    public abstract class AssemblyExecutableCommand : AssemblableCommand, Executable {

        public string AssemblyCommand { get; protected set; }

        protected AssemblyExecutableCommand(ApplicationContext ctx, string instruct) : base(ctx, instruct) {
            AssemblyCommand = instruct;
        }

        public abstract void Execute();

        public static AssemblyExecutableCommand GetCommandFromName(string instruct, ApplicationContext myctx) {
            


            var type = Type.GetType($"DebugReloaded.Commands.AssemblyCommands.{GetCommandNameFromString(instruct).ToUpper()}ExecutableCommand");
            return (AssemblyExecutableCommand)Activator.CreateInstance(type, myctx, instruct);
        }


        protected IMemorizable[] GetParamsMemorizables() {
            return GetIMemorizablesFromCommand(AssemblyCommand, base.context);
        }

        private static IMemorizable[] GetIMemorizablesFromCommand(string cmd, ApplicationContext context) {

            IMemorizable GetMemorizableFromAssembly(string ass) {

                Register reg;

                if ((reg = context.Registers.Find(r => r.Name == ass)) != null)
                    return reg;

                MemoryRangePointer mem;

                if (ass.IndexOf('[') < ass.IndexOf(']') && ass.IndexOf('[') != -1) {


                    // TODO Byte / Word
                    // TODO Indirect Ref
                    MemoryRangePointer pointer =
                        context.MainMemory.ExtractMemoryPointer(ass.BetweenSubstring("[", "]").ToInt(), 2);

                    return pointer;
                }

                return new ImmediateNumber(MySupport.GetBytesArrayFromString(ass), true);
            }


            var memorizables = new IMemorizable[2];

            var splits = cmd.Split(',');

            for (int i = 0; i < 2; i++) {
                splits[i] = splits[i].Substring(splits[i].IndexOf(' ') != -1 ? splits[i].IndexOf(' ') : 0).TrimStart().TrimEnd();
                memorizables[i] = GetMemorizableFromAssembly(splits[i]);
            }

            return memorizables;           
        }

        public static string GetCommandNameFromString(string cmd) {
            return cmd.Substring(0, (cmd.IndexOf((char) 32))).TrimEnd();
        }


    }
}