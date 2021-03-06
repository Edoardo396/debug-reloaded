﻿using System;

namespace DebugReloaded.Containers {
    public class Flag : IMemorizable {
        public string Name { get; }


        public bool Value { get; set; }

        public Flag() {
        }

        public Flag(string name) {
            Name = name;
        }

        public void SetValues(int index, byte[] value) {
            this.SetValue(value[0]);
        }

        public byte[] GetValues(int index, int howmany) {
            return new[] { (byte)(Value ? 1 : 0) };
        }

        public void Set() {
            this.Value = true;
        }

        public void Reset() {
            Value = false;
        }

        [Obsolete("Dont use that. For flag is dangeous and useless, yo can use Value prop instead or the flag itself. It is here only for inplement IMemorizable", true)]
        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, 0, 1);
        }

        public int Length => 1;

        public void SetValue(bool value) {
            Value = value;
        }

        public void SetValue(byte value) {
            Value = value != 0;
        }

        public override string ToString() {
            return Value ? Name.ToUpper() : "N" + char.ToUpper(Name[0]);
        }
    }
}