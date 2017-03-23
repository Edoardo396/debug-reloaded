using System;
using DebugReloaded.Interface;
using DebugReloaded.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTests {

    [TestClass]
    public class DebugCommandsTests {

        private static ApplicationContext context;


        [AssemblyInitialize]
        public static void Initilize(TestContext _context) {
            context = new ApplicationContext();
        }


        [TestMethod]
        public void TestRCommand() {

            context.Interpreter.ExecuteCommand(new DebugCommand("r ax"), "ffff");

            
            Assert.IsTrue(context.GetRegisterByName("ax").ToString() == "FFFF");
        }



    }
}
