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

            context.MainMemory.SetValues(100, new byte[] {0xff, 0x55, 0x21, 0b10001, 0xab});

            MemoryRangePointer pointer = context.MainMemory.ExtractMemoryPointer(100, 5);

            pointer.SetValue(2, 0x00);
        }

        [TestMethod()]
        public void UpdateTest() {

            context.MainMemory.SetValues(100, new byte[] { 0xff, 0x55, 0x21, 0b10001, 0xab });

            MemoryRangePointer pointer = context.MainMemory.ExtractMemoryPointer(100, 5);

            pointer[4] = 0x50;

            pointer.SetValue(2, 0x00);

            Assert.IsTrue(context.MainMemory[102] == (byte)0 && context.MainMemory[104] == (byte)80);
        }
    }
}