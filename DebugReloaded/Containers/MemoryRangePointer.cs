﻿using System;
using System.Runtime.CompilerServices;

namespace DebugReloaded.Containers {
    public class MemoryRangePointer : Memory {
        private readonly IMemorizable originalMemory;

        public bool AutoUpdate { get; set; } = true;
        public int From { get; private set; }
        public int To { get; private set; }

        public byte this[int index] {
            get { return this.GetValues(index, 1)[0]; }
            set { this.SetValues(index, new byte[] {value}); }
        }

        public MemoryRangePointer(IMemorizable originalMemory, int from, int howmany) : base(howmany) {
            this.originalMemory = originalMemory;
            From = from;
            To = from + howmany;

            content = originalMemory.GetValues(from, howmany);
        }

        public void Update() {
            originalMemory.SetValues(From, content);
        }

        public override void SetValues(int index, byte[] value) {
            base.SetValues(index, value);

            if (AutoUpdate)
                this.Update();
        }
    }
}