using System;
using DebugReloaded.Commands.AssemblyCommands;
using DebugReloaded.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTests.DebugCommandsTests;

namespace DebugTests {
    [TestClass]
    public class AssemblyCommandsTests {

        // [TestMethod]
        public void MovCommands() {

            string[] commands = {"mov ax,fff1", "mov bx,5f65", "mov [200],ax", "mov cx,[200]"};

            foreach (var command in commands) {

                var cmd = AssemblyExecutableCommand.GetCommandFromName(command, context);

                cmd.Execute();

                Console.WriteLine(context.GetRegisterByName("ax").Value.ToHexString());
                Console.WriteLine(context.GetRegisterByName("bx").Value.ToHexString());
                Console.WriteLine(context.GetRegisterByName("cx").Value.ToHexString());

                
            }

            Console.WriteLine(context.MainMemory.Dump(200, 10));
        }


        [TestMethod]
        public void IndirectReferenceMov() {
            string[] commands = {"mov ax,0200", "mov [ax],ffff"};

            foreach (string command in commands) {

                var cmd = AssemblyExecutableCommand.GetCommandFromName(command, context);

                cmd.Execute();

                Console.WriteLine(context.GetRegisterByName("ax").Value.ToHexString());

                Console.WriteLine(context.MainMemory.Dump(200,2));
            }
        }

    }
}
