using System;
using DebugReloaded.Commands.AssemblyCommands;
using DebugReloaded.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTests.DebugCommandsTests;

namespace DebugTests.CommandsDebug {
    [TestClass]
    public class JumpCommandsTests {
        [TestMethod]
        public void UnConditionalJumpTest() {

            var command = AssemblyExecutableCommand.GetCommandFromName("jmp 100", context);

            command.Execute();

            Assert.IsTrue(context.GetRegisterByName("ip").GetValues().ToHexString() == "0100");
        }

        [TestMethod]
        public void JNZCondJump() {

            context.GetFlagByName("zf").Set();
            context.GetRegisterByName("ip").SetValue(new byte[] {0x01, 0x00});

            var command = AssemblyExecutableCommand.GetCommandFromName("jnz 400", context);

            command.Execute();

            Console.WriteLine(context.GetRegisterByName("ip").Value.ToHexString());
        }

    }
}
