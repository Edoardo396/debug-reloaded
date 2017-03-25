using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public abstract class Command {
        public abstract string Name { get; }
        public List<string> Parameters { get; set; }
        public byte[] ByteCodes { get; }

        protected Command(List<string> parameters) {
            Parameters = parameters;
        }

        protected Command(params string[] parameters) {
            Parameters = parameters.ToList();
        }

        protected Command(List<string> parameters, byte[] byteCodes) {
            Parameters = parameters;
            ByteCodes = byteCodes;
        }

        public abstract void Execute(ApplicationContext context);
        public abstract byte[] CovertToMachineLanguage();
    }
}
