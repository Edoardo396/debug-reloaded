using System.Collections;
using System.Linq;

namespace DebugReloaded.Containers {
    public class Memory  : IMemorizable {

        private byte[] content;

        public string Name { get; set; }

        public byte this[int index] => this.GetValue(index);

        public int Size => content.Length;

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
                str += $"{content[i + startindex]:X2}-";

            str = str.Remove(str.Length - 1);

            return str;
        }

        public string Dump() {
            return this.Dump(0, content.Length);
        }

        public void SetValuesLE(int startIndex, byte[] _bytes) {

            byte[] bytes = new byte[2];

            bytes = _bytes.Length == 1 ? new byte[] {0, _bytes[0]} : _bytes;

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

        // I USE TUPLES
        public static (bool isMemory, int location) IsMemoryAddress(string value) {
            int loc = -1;
            bool isMem = value.StartsWith("[") && value.EndsWith("]") && int.TryParse(value.Substring(1, value.Length - 2), out loc);
            return (isMem, loc);
        }

    }
}