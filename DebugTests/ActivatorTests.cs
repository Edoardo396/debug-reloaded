using System;
using DebugReloaded.Commands.AssemblyCommands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTests.DebugCommandsTests;

namespace DebugTests {


    [TestClass]
    public class ActivatorTests {


        [TestMethod]
        public void MOVTest() {
            var cmd = AssemblyExecutableCommand.GetCommandFromName("mov ax,bx", context);

            Console.WriteLine(cmd.GetType().FullName);
        }
    }
}
