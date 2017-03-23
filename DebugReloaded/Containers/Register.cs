using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReloaded.Containers {
    public class Register {

        private byte[] content = new byte[2] {0x00, 0x00};

        public string Name { get; set; } = null;
        public byte H { get { return content[0]; } set { content[0] = value; } }
        public byte L { get { return content[1]; } set { content[1] = value; } }
        public byte[] Value => content;

        public Register(string name) {
            Name = name;
        }

        public Register() {
                
        }

        public override string ToString() {
            return $"{content[0]:X}{content[1]:X}";
        }
    }
}
