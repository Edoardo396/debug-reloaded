using System;
using DebugReloaded.Commands;
using DebugReloaded.Containers;
using DebugReloaded.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace DebugTests {
    [TestClass]
    public class DissassemblerTests {
        [TestMethod]
        public void SimpleMOVDisTest() {
            Memory mem = new Memory(new byte[] {0x89, 0xd8});

            AssemblableCommand cmd = Disassembler.Dissassemble(DebugCommandsTests.context,
                mem.ExtractMemoryPointer(0, 2));

            Assert.IsTrue(cmd.ToString() == "mov ax,bx");
        }

        [TestMethod]
        public void MediumMOVTest() {
            Memory mem = new Memory(new byte[] {0xb8, 0xaa, 0xff});

            AssemblableCommand cmd = Disassembler.Dissassemble(DebugCommandsTests.context,
                mem.ExtractMemoryPointer(0, 3));

            Assert.IsTrue(cmd.ToString() == "mov ax,FFAA");
        }

        [TestMethod]
        public void DoubleParameterMOVTest() {
            Memory mem = new Memory(new byte[] {0xc7, 0x06, 0x00, 0x01, 0xaa,0xff});

            AssemblableCommand cmd = Disassembler.Dissassemble(DebugCommandsTests.context,
                mem.ExtractMemoryPointer(0, 6));

            Console.WriteLine(cmd.ToString());
            Assert.IsTrue(cmd.ToString() == "mov [0100],FFAA");
        }


        [TestMethod]
        public void INCDECTest() {
            Memory mem = new Memory(new byte[] { 0x40 });

            AssemblableCommand cmd = Disassembler.Dissassemble(DebugCommandsTests.context, mem.ExtractMemoryPointer(0,1));
            Console.WriteLine(cmd.ToString());
            Assert.IsTrue(cmd.ToString() == "inc ax");
        }

        [TestMethod]
        public void REGMEMMOVTest() {
            Memory mem = new Memory(new byte[] { 0xa1, 0x00, 0x02 });

            AssemblableCommand cmd = Disassembler.Dissassemble(DebugCommandsTests.context,
                mem.ExtractMemoryPointer(0, 3));

            Console.WriteLine(cmd.ToString());
            Assert.IsTrue(cmd.ToString() == "mov ax,[0200]");
        }

        [TestMethod]
        public void REGIMMMOVTest() {
            Memory mem = new Memory(new byte[] { 0xbb, 0x56, 0x78 });

            AssemblableCommand cmd = Disassembler.Dissassemble(DebugCommandsTests.context,
                mem.ExtractMemoryPointer(0, 3));

            Console.WriteLine(cmd.ToString());
            Assert.IsTrue(cmd.ToString() == "mov bx,7856");
        }

        [TestMethod]
        public void TestCommandLength() {

            Memory mem = new Memory(new byte[] { 0xc7, 0x06, 0x00, 0x01, 0xaa, 0xff, 0x0, 0x87, 0x97, 0xba });

            int length = Disassembler.GetCommandLength(DebugCommandsTests.context, mem);

            Assert.IsTrue(length == 6);
        }

        [TestMethod]
        public void MultipleDissasmblerTest() {
            
            Memory mem = new Memory(MySupport.GetBytesArrayFromString("89D889C1B8FF56404341C70600018967"));

            var cmds = Disassembler.MultiCommandDisassembler(DebugCommandsTests.context, mem);

            foreach (var cmd in cmds) 
                Console.WriteLine(cmd);
            

            Assert.IsTrue(cmds.Count == 7);
        }
    }
}