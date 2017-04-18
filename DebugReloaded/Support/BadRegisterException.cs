using System;
using System.Runtime.Serialization;

namespace DebugReloaded.Containers {
    [Serializable]
    internal class BadRegisterException : Exception {
        private byte[] bytes;
        private Register register;

        public BadRegisterException() {
        }

        public BadRegisterException(string message) : base(message) {
        }

        public BadRegisterException(string message, Exception innerException) : base(message, innerException) {
        }

        public BadRegisterException(Register register, byte[] bytes, string v) : base(v) {
            this.register = register;
            this.bytes = bytes;
        }

        protected BadRegisterException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}