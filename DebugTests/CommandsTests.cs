using System;
using System.Collections.Generic;
using DebugReloaded.Commands;
using DebugReloaded.Commands.AssemblyCommands;
using DebugReloaded.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DebugTests.DebugCommandsTests;

namespace DebugTests {
    [TestClass]
    public class CommandsTests {
        [TestMethod]
        public void DefaultAssembleableCommandTestMOV() {
            var cmd = new AssemblableCommand(context, "mov ax,bx");

            cmd.Assemble();

            context.MainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.MainMemory.Dump(300, 2) == "89-D8");
        }


        [TestMethod]
        public void ImmediateAssembleableCommandTestMOV() {
            var cmd = new AssemblableCommand(context, "mov ax,0001");

            context.MainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.MainMemory.Dump(300, 3) == "B8-01-00");
        }

        [TestMethod]
        public void ImmediateAssembleableCommandTestSIMOV() {
            var cmd = new AssemblableCommand(context, "mov si,B7");

            context.MainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.MainMemory.Dump(300, 3) == "BE-B7-00");
        }

        [TestMethod]
        public void ImmediateMemoryCommandTestMOV() {
            var cmd = new AssemblableCommand(context, "mov [0123],00FF");

            context.MainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.MainMemory.Dump(300, 6) == "C7-06-23-01-FF-00");
        }

        [TestMethod]
        public void MemoryAssembleableCommandTestMOV() {
            var cmd = new AssemblableCommand(context, "mov ax,[100]");

            context.MainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.MainMemory.Dump(300, 3) == "A1-00-01");
        }

        [TestMethod]
        public void GetMemorizableTest() {
            string[] memz = {"ax,[100]", "ax,ff", "[100],ax"};

            foreach (var s in memz)
                foreach (var cmd in AssemblyExecutableCommand.GetIMemorizablesFromCommand(s, context))
                    Console.WriteLine(cmd);
        }
    }
}