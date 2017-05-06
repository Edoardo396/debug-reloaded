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
    }
}
