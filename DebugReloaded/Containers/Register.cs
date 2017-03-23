using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReloaded.Containers {
    public class Register {
        private byte[] content = new byte[2] {0x00, 0x00};

        public string Name { get; set; } = null;

        public byte H {
            get { return content[0]; }
            set { content[0] = value; }
        }

        public byte L {
            get { return content[1]; }
            set { content[1] = value; }
        }

        public byte[] Value => content;

        public void SetValue(byte[] bytes) {
            if (bytes.Length != 2)
                throw new BadRegisterException(this, bytes, "Cannot insert more than 2 bytes in a register");

            content = bytes;
        }

        public void SetValue(string sbytes) {
            if (sbytes.Length != 4)
                throw new BadRegisterException(this, null, "Cannot insert more than 2 bytes in a register");

            byte[] bytes =
                Enumerable.Range(0, sbytes.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(sbytes.Substring(x, 2), 16))
                    .ToArray();

            content = bytes;
        }


        public Register(string name) {
            Name = name;
        }

        public Register() {
        }

        public override string ToString() {
            return $"{content[0]:X2}{content[1]:X2}";
        }
    }
}