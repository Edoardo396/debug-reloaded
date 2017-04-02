using System;
using System.Collections.Generic;
using DebugReloaded.Commands;
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

            context.mainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.mainMemory.Dump(300,2) == "89-D8");
        }


        [TestMethod]
        public void ImmediateAssembleableCommandTestMOV() {

            var cmd = new AssemblableCommand(context, "mov ax,0001");

            context.mainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.mainMemory.Dump(300, 3) == "B8-01-00");
        }

        [TestMethod]
        public void ImmediateAssembleableCommandTestSIMOV() {

            var cmd = new AssemblableCommand(context, "mov si,B7");

            context.mainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.mainMemory.Dump(300, 3) == "BE-B7-00");
        }

        [TestMethod]
        public void MemoryAssembleableCommandTestMOV() {

            var cmd = new AssemblableCommand(context, "mov ax,[100]");

            context.mainMemory.SetValues(300, cmd.Assemble());

            Assert.IsTrue(context.mainMemory.Dump(300, 3) == "A1-00-01");
        }


    }

}
