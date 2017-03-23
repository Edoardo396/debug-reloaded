using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReloaded.Interface {
    public class DebugCommand {
        
        public string CommandString { get; set; }

        public List<string> Parameters { get; set; }

        public DebugCommand(string queryString) {

            // TODO Not Always true.
            CommandString = queryString[0].ToString();

            Parameters = queryString.Split((char) 32).Skip(1).ToList();
        }

    }
}
