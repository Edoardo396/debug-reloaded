using System;
using System.Collections.Generic;
using System.Linq;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public class AddCommand : Command {

        public override string Name => "add";

        public AddCommand(List<string> parameters) : base(parameters) {

        }

        public AddCommand(params string[] parameters) : base(parameters) {

        }

        public AddCommand(List<string> parameters, byte[] byteCodes) : base(parameters, byteCodes) {

        }

        public override void Execute(ApplicationContext context) {

            

            
        }

        public override byte[] CovertToMachineLanguage() {
            throw new System.NotImplementedException();
        }
    }
}