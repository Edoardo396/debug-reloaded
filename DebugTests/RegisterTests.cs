using System;
using DebugReloaded.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTests {
    [TestClass]
    public class RegisterTests {
        [TestMethod]
        public void CreateRegisterTest() {
            byte a = 0xff;
            byte b = 0xbb;

            var reg = new Register() {H = a, L = b};

            Assert.IsTrue(reg.ToString().ToLower() == "ffbb");
            Assert.IsTrue(reg.Value[0] == a && reg.Value[1] == b);
        }

        [TestMethod]
        public void CreateRegisterNameTest() {
            byte a = 0xff;
            byte b = 0xbb;

            var reg = new Register("test") {H = a, L = b};

            Assert.IsTrue(reg.ToString().ToLower() == "ffbb");
            Assert.IsTrue(reg.Value[0] == a && reg.Value[1] == b);
            Assert.IsTrue(reg.Name == "test");
        }
    }
}