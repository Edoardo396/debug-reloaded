using DebugReloadedCore.Support;

namespace DebugReloadedCore.Containers {
    public class Memory : IMemorizable {
        protected byte[] content;

        public string Name { get; set; }

        public byte this[int index] => this.GetValue(index);

        public Memory(int byteSize) {
            content = new byte[byteSize];
        }

        public Memory(byte[] contentBytes) {
            content = contentBytes;
        }

        public int Length => content.Length;

        public byte[] GetValues(int index, int howmany) {
            var bytes = new byte[howmany];

            for (var i = 0; i < howmany; i++)
                bytes[i] = content[i + index];


            return bytes;
        }

        public virtual void SetValues(int index, byte[] bytes) {
            for (var i = 0; i < bytes.Length; i++)
                content[i + index] = bytes[i];
        }

        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, index, howmany);
        }

        public void Reset() {
            for (var i = 0; i < content.Length; i++)
                content[i] = 0x00;
        }

        public byte GetValue(int index) {
            return this.GetValues(index, 1)[0];
        }

        public virtual void SetValue(int index, byte value) {
            this.SetValues(index, new[] {value});
        }

        public string Dump(int startindex, int howmany) {
            string str = string.Empty;

            for (var i = 0; i < howmany; i++)
                str += $"{content[i + startindex]:X2}-";

            str = str.Remove(str.Length - 1);

            return str;
        }

        public string Dump() {
            return this.Dump(0, content.Length);
        }

        public void SetValuesLE(int startIndex, byte[] _bytes) {
            var bytes = new byte[2];

            bytes = _bytes.Length == 1 ? new byte[] {0, _bytes[0]} : _bytes;

            if (bytes.Length != 2)
                throw new BadMemoryException(this, startIndex,
                    "In little endian mode yot can't set more then 2 values at a time");

            content[startIndex] = bytes[1];
            content[startIndex + 1] = bytes[0];
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