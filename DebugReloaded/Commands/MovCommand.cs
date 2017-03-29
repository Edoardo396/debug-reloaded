using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Configuration;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public class MovCommand : Command {
        public override string Name => "mov";

        public MovCommand(List<string> parameters) : this(parameters, null) {
        }

        public MovCommand(params string[] parameters) : base(parameters) {

        }

        public MovCommand(List<string> parameters, byte[] byteCodes) : base(parameters, byteCodes) {
        }

        public override void Execute(ApplicationContext context) {

            // TODO Indexed reference is missing

            
        }

        public override byte[] CovertToMachineLanguage() {

            byte[] bytes = new byte[2];
            throw new RowNotInTableException();






        }
    }
}