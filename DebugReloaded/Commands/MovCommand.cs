using System;
using System.Collections.Generic;
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

            (IValuable container, int location) source = context.GetLocationFromString(Parameters[1]);

            (IValuable container, int location) destination = context.GetLocationFromString(Parameters[0]);

            List<byte> locValue = BitConverter.GetBytes(source.Item2).Reverse().ToList();
            locValue.RemoveAll(e => e == 0);

            destination.container.ValSetValues(destination.location, source.Item1 != null ? source.Item1.ValGetValues(source.Item2, 2) : locValue.ToArray());
        }

        public override byte[] CovertToMachineLanguage() {
            throw new System.NotImplementedException();
        }
    }
}