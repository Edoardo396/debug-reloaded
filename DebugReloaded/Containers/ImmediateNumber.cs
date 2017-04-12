using System;
using DebugReloaded.Interface;

namespace DebugReloaded.Containers {
    public class ImmediateNumber : IMemorizable {

        private byte[] content;

        public ImmediateNumber(byte[] content, bool isWord) {
            if (content.Length > 2 || content.Length < 1)
                throw new Exception("Bad length");
            this.content = content;

            if (isWord && content.Length == 1)
                content = new byte[] {0x0, content[0]};

        }

        public virtual void SetValues(int index, byte[] bytes) {
            ConsoleLogger.Write("Assigning a value to a number. Be Careful", "WARNING", ConsoleColor.Yellow);
            for (int i = 0; i < bytes.Length; i++)
                content[i + index] = bytes[i];
        }

        public byte[] GetValues(int index, int howmany) {
            byte[] bytes = new byte[howmany];

            for (int i = 0; i < howmany; i++)
                bytes[i] = content[i + index];


            return bytes;
        }

        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, index, howmany);
        }

        public int Length => content.Length;

    }
}