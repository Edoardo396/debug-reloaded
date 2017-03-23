using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebugTests {
    [TestClass]
    public class MemoryTests {

        [TestMethod]
        public void MemoryGeneralTest() {
           
            var mem = new Memory(0xffff);
            mem.Reset();
        }

        [TestMethod]
        public void ValuesTest() {

            var testBytes = new byte[] {0xff, 0x50, 0x78, 0x00};

            var mem = new Memory(0xffff) {Name = "RAM"};
            mem.Reset();

            mem.SetValues(500, testBytes);

            string memDump = mem.Dump(500, 4);

            Assert.IsTrue(memDump == "FF50780");
            Assert.IsTrue(mem.GetValue(500) == 0xff);
            Assert.IsTrue(mem[501] == 0x50);
            Assert.IsTrue(BitConverter.ToString(mem.GetValues(500,4)) == BitConverter.ToString(testBytes));

            mem.SetValue(504, 0x01);
            Assert.IsTrue(mem[504] == 0x1);
        }

        [TestMethod]
        public void LittleEndianTest() {

            var testBytes = new byte[] { 0xff, 0xbb };

            var mem = new Memory(0xffff) { Name = "RAM" };
            mem.Reset();

            mem.SetValuesLE(500, testBytes);
            Assert.IsTrue(BitConverter.ToString(mem.GetValues(500,2)) == "BB-FF");
            Assert.IsTrue(BitConverter.ToString(mem.GetValuesLE(500)) == "FF-BB");      
        }


        [TestMethod]
        public void SubMemoryTest() {

            var testBytes = new byte[] { 0xff, 0xbb, 0xaa, 0xee, 0x00 };

            var mem = new Memory(0xffff) { Name = "RAM" };
            mem.Reset();

            mem.SetValues(500, testBytes);

            Assert.IsTrue(mem.SubMemory(500, 5).Dump(0, 5) == "FFBBAAEE0");

        }
    }
}