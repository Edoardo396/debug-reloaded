using System;
using DebugReloaded.Interface;
using DebugReloaded.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTests {

    [TestClass]
    public class DebugCommandsTests {

        public static ApplicationContext context;


        [AssemblyInitialize]
        public static void Initilize(TestContext _context) {
            context = new ApplicationContext();
        }


        [TestMethod]
        public void TestRCommand() {

            context.Interpreter.ExecuteCommand(new DebugCommand("r ax"), "ffff");

            context.Interpreter.ExecuteCommand(new DebugCommand("r"));
            
            Assert.IsTrue(context.GetRegisterByName("ax").ToString() == "FFFF");
        }

        [TestMethod]
        public void TestDCommand() {
            context.Interpreter.ExecuteCommand(new DebugCommand("d 100"));
        }

        [TestMethod]
        public void TestECommand() {
            context.Interpreter.ExecuteCommand(new DebugCommand("e 100"), "aabbccddeeff");


            Assert.IsTrue(context.MainMemory.Dump(100, 6) == "AA-BB-CC-DD-EE-FF");
        }




    }
}
