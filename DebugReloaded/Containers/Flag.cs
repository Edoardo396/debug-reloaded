using System;
using DebugReloaded.Containers;

namespace DebugReloaded.Containers {
    public class Flag : IMemorizable {

        
        public string Name { get; }

        public Flag() {
            
        }

        public Flag(string name) {
            Name = name;
        }

        public bool Value { get; set; }

        public void SetValue(bool value) {
            Value = value;
        }

        public void SetValue(byte value) {
            Value = value != 0;
        }

        public void SetValues(int index, byte[] value) {
            this.SetValue(value[0]);
        }

        public byte[] GetValues(int index, int howmany) {
            return new byte[] {(byte) (Value ? 1 : 0)};
        }


        [Obsolete("Dont use that. For flag is dangeous and useless, yo can use Value prop instead or the flag itself. It is here only for inplement IMemorizable", true)]
        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, 0, 1);
        }

        public int Length => 1;
    }
}