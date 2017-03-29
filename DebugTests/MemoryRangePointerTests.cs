using Microsoft.VisualStudio.TestTools.UnitTesting;
using DebugReloaded.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DebugTests.DebugCommandsTests;

namespace DebugTests {
    [TestClass()]
    public class MemoryRangePointerTests {
        [TestMethod()]
        public void MemoryRangePointerTest() {

            context.mainMemory.SetValues(100, new byte[] {0xff, 0x55, 0x21, 0b10001, 0xab});

            MemoryRangePointer pointer = context.mainMemory.ExtractMemoryPointer(100, 5);

            pointer.SetValue(2, 0x00);
        }

        [TestMethod()]
        public void UpdateTest() {

            context.mainMemory.SetValues(100, new byte[] { 0xff, 0x55, 0x21, 0b10001, 0xab });

            MemoryRangePointer pointer = context.mainMemory.ExtractMemoryPointer(100, 5);

            pointer[4] = 0x50;

            pointer.SetValue(2, 0x00);

            Assert.IsTrue(context.mainMemory[102] == (byte)0 && context.mainMemory[104] == (byte)80);
        }
    }
}