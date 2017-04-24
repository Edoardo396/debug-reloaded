using System;
using System.Linq;
using DebugReloadedCore.Support;

namespace DebugReloadedCore.Containers {
    public class Register : IMemorizable {
        public string Name { get; set; }

        public byte H {
            get { return Value[0]; }
            set { Value[0] = value; }
        }

        public byte L {
            get { return Value[1]; }
            set { Value[1] = value; }
        }

        public byte[] Value { get; private set; } = new byte[2] {0x00, 0x00};


        public Register(string name) {
            Name = name;
        }

        public Register() {
        }

        public void SetValues(int index, byte[] value) {
            var bytes = new byte[2];

            bytes = value.Length == 1 ? new byte[] {0, value[0]} : value;

            if (bytes.Length != 2)
                throw new BadRegisterException(this, value, "Register is 2 bytes only!");

            this.SetValue(value);
        }

        public byte[] GetValues(int index = 0, int howmany = 0) {
            return Value;
        }

        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, index, howmany);
        }

        public int Length => 2;

        public void SetValue(byte[] _bytes) {
            var bytes = new byte[2];

            bytes = _bytes.Length == 1 ? new byte[] {0, _bytes[0]} : _bytes;

            if (bytes.Length != 2)
                throw new BadRegisterException(this, bytes, "Cannot insert more than 2 bytes in a register");

            Value = bytes;
        }

        public void SetValue(string sbytes) {
            if (sbytes.Length != 4)
                throw new BadRegisterException(this, null, "Cannot insert more than 2 bytes in a register");

            byte[] bytes =
                Enumerable.Range(0, sbytes.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(sbytes.Substring(x, 2), 16))
                    .ToArray();

            Value = bytes;
        }

        public override string ToString() {
            return $"{Value[0]:X2}{Value[1]:X2}";
        }
    }
}