using System.Collections.Generic;
using System.Linq;

namespace DebugReloaded.Interface {

    /// <summary>
    /// Definisce un comando di ^debug
    /// </summary>
    public class DebugCommand {
        public string CommandString { get; set; }

        public List<string> Parameters { get; set; }

        public DebugCommand(string queryString) {
            // NOTE Not Always true.
            CommandString = queryString[0].ToString();

            Parameters = queryString.Split((char) 32).Skip(1).ToList();
        }
    }
}