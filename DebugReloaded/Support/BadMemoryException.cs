using System;
using System.Runtime.Serialization;

namespace DebugReloaded.Containers {

    [Serializable]
    internal class BadMemoryException : Exception {

        private Memory memory;
        private int startIndex;

        public BadMemoryException() {
        }

        public BadMemoryException(string message) : base(message) {

        }

        public BadMemoryException(Memory memory, int startIndex, string message) : base(message) {
            this.memory = memory;
            this.startIndex = startIndex;
        }

        public BadMemoryException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BadMemoryException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}