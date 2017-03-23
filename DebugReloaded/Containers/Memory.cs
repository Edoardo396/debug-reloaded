using System.Collections;

namespace DebugReloaded.Containers {
    public class Memory {

        private byte[] content;

        public string Name { get; set; }

        public byte this[int index] => this.GetValue(index);

        public Memory(int byteSize) {
            content = new byte[byteSize];
        }

        public Memory(byte[] contentBytes) {
            this.content = contentBytes;
        }

        public void Reset() {
            for (int i = 0; i < content.Length; i++)
                content[i] = 0x00;
        }

        public byte[] GetValues(int index, int howmany) {
            byte[] bytes = new byte[howmany];

            for (int i = 0; i < howmany; i++)
                bytes[i] = content[i + index];


            return bytes;
        }

        public byte GetValue(int index) {
            return this.GetValues(index, 1)[0];
        }

        public void SetValues(int index, byte[] bytes) {
            for (int i = 0; i < bytes.Length; i++)
                content[i + index] = bytes[i];
        }

        public void SetValue(int index, byte value) {
            SetValues(index, new[] {value});
        }

        public string Dump(int startindex, int howmany) {
            string str = string.Empty;

            for (int i = 0; i < howmany; i++) 
                str += $"{content[i + startindex]:X}";

            return str;
        }

        public void SetValuesLE(int startIndex, byte[] bytes) {

            if (bytes.Length != 2)
                throw new BadMemoryException(this, startIndex, "In little endian mode yot can't set more then 2 values at a time");

            content[startIndex] = bytes[1];
            content[startIndex+1] = bytes[0];
        }

        public byte[] GetValuesLE(int startindex) {
            return new[] {content[startindex + 1], content[startindex]};
        }

        public Memory SubMemory(int startindex, int howmany) {

            byte[] bytes = this.GetValues(startindex, howmany);

            return new Memory(bytes);
        }
     
    }
}