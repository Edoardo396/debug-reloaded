using System;
using DebugReloaded.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTests {
    [TestClass]
    public class FlagTests {
        [TestMethod]
        public void MainFlagTest() {
            Flag flag = new Flag("testflag");

            Assert.IsTrue(flag.Name == "testflag");

            flag.SetValue(true);
            Assert.IsTrue(flag.Value);

            Assert.IsTrue(flag.GetValues(-1, -1)[0] == (byte) 1);

            flag.SetValues(-1, new byte[] {0});
            Assert.IsTrue(!flag.Value);
        }
    }
}