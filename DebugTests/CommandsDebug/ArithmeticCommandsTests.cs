using System;
using DebugReloaded.Commands.AssemblyCommands;
using DebugReloaded.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTests.DebugCommandsTests;

namespace DebugTests.CommandsDebug {
    [TestClass]
    public class ArithmeticCommandsTests {
        [TestMethod]
        public void AddTests() {

            string[] commands = {"add ax,00ff", "add bx,ax", "add [200],bx"};

            foreach (var command in commands) {

                var cmd = AssemblyExecutableCommand.GetCommandFromName(command, context);

                cmd.Execute();

                Console.WriteLine($"AX: {context.GetRegisterByName("ax").Value.ToHexString()} BX: {context.GetRegisterByName("bx").Value.ToHexString()} [200]: {context.MainMemory.Dump(200,2)}");          
            }
        }
    }
}
