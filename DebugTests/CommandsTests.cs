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
        public void MovTestMemToReg() {

            context.mainMemory.SetValues(100, new byte[] {0xff,0xee} );

            MovCommand cmd = new MovCommand(new List<string>() {"ax","[100]"});

            cmd.Execute(context);

            Assert.IsTrue(context.GetRegisterByName("ax").ToString() == "EEFF");

            context.mainMemory.SetValues(300, new byte[] { 0x07, 0x66});

            MovCommand cmd2 = new MovCommand(new List<string>() { "ax", "[300]" });

            cmd2.Execute(context);

            Assert.IsTrue(context.GetRegisterByName("ax").ToString() == "6607");
        }

        [TestMethod]
        public void MovTestRegToMem() {

            context.GetRegisterByName("ax").SetValue("DDAA");
            
            MovCommand cmd = new MovCommand("[100]", "ax");

            cmd.Execute(context);

            Assert.IsTrue(context.mainMemory.Dump(100, 2) == "AA-DD");
        }

        [TestMethod]
        public void MovTestImmToMem() {

            MovCommand cmd = new MovCommand("[100]", "100");

            cmd.Execute(context);

            Assert.IsTrue(context.mainMemory.Dump(100, 2) == "64-00");
        }



    }
}
