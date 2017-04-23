using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using DebugReloaded.Containers;
using DebugReloaded.Support;
using Microsoft.SqlServer.Server;

namespace DebugReloaded.Commands.AssemblyCommands {
    public abstract class AssemblyExecutableCommand : AssemblableCommand, Executable {

        protected ApplicationContext ctx;

        public abstract string Name { get; }

        protected AssemblyExecutableCommand(ApplicationContext ctx) {
            this.ctx = ctx;
        }

        public abstract void Execute(IMemorizable[] par);

        public static AssemblyExecutableCommand GetCommandFromName(string cmdName, ApplicationContext myctx) {
            

            var type = Type.GetType($"DebugReloaded.Commands.AssemblyCommands.{cmdName.ToUpper()}Command");
            return (AssemblyExecutableCommand)Activator.CreateInstance(type, myctx);
        }



        public static IMemorizable[] GetIMemorizablesFromCommand(string cmd, ApplicationContext context) {

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
                splits[i] = splits[i].TrimStart().TrimEnd();
                memorizables[i] = GetMemorizableFromAssembly(splits[i]);
            }

            return memorizables;           
        }

        public static string GetCommandNameFromString(string cmd) {
            return cmd.Substring(0, (cmd.IndexOf((char) 32) - 1));
        }


    }
}