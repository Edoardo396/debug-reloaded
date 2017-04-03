using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public class AssemblableCommand : Assemblable {
        private CommandTemplate selectedCommand;

        private ApplicationContext context;

        private DataType[] parmsDt;
        private readonly string[] parms;

        public AssemblableCommand(CommandTemplate cmd) {
            selectedCommand = cmd;
        }

        public AssemblableCommand(ApplicationContext ctx, string instruct) {
            context = ctx;

            string cmd = instruct.Split((char) 32)[0];
            parms = instruct.Split((char) 32)[1].Split(',');

            parmsDt = new DataType[2] {DataType.None, DataType.None};

            for (var i = 0; i < parms.Length; i++)
                parmsDt[i] = CommandTemplate.GetDTFromArgument(parms[i]);

            var parmsDts = new string[2];

            for (var i = 0; i < parms.Length; i++)
                parmsDts[i] = CommandTemplate.DataTypeToString(parmsDt[i]);

            selectedCommand =
                context.CommandTemplList.Find(
                    c =>
                        c.Name == cmd.Replace(" ", "") && c.ParTypes.SequenceEqual(parmsDt) &&
                        this.CheckParSpecific(c, parms));


            if (selectedCommand == null)
                throw new Exception("No Command matching criteria.");
        }

        private bool CheckParSpecific(CommandTemplate c, string[] par) {
            return !par.Where((t, i) => c.ParSpecific[i] != t && c.ParSpecific[i] != "any").Any();
        }

        public string Disassemble() {
            throw new System.NotImplementedException();
        }

        public byte[] Assemble() {
            string GetParameter(string content) {
                if (content == "op1")
                    return this.parms[0];
                return content == "op2" ? this.parms[1] : "";
            }

            int first_dollar = selectedCommand.OpCode.IndexOf("$");

            if (first_dollar == -1)
                return MySupport.GetBytesArrayFromString(selectedCommand.OpCode);

            int last_dollar = selectedCommand.OpCode.IndexOf("$", first_dollar + 1);

            string dollarparameter = selectedCommand.OpCode.Substring(first_dollar, last_dollar + 1 - first_dollar);

            int points = dollarparameter.IndexOf(":");

            if (points == -1) {
                string fs = selectedCommand.OpCode.Replace(dollarparameter,
                    GetParameter(dollarparameter.Replace("$", string.Empty)));

                return MySupport.GetBytesArrayFromString(fs);
            }

            string fpar = selectedCommand.OpCode.Substring(first_dollar + 1, points - first_dollar + 1);

            string format = selectedCommand.OpCode.Substring(points + 3, last_dollar - (points + 3));

            string parms = GetParameter(fpar).Replace("[", "").Replace("]", "");

            MySupport.NormalizeValueString(ref parms);

            byte[] paramsBytes = MySupport.GetBytesArrayFromString(parms);

            if (paramsBytes.Length != 1 && paramsBytes.Length != 2)
                throw new Exception("Immediate params lenght must be a byte or a word.");

            if (format == "le")
                paramsBytes = paramsBytes.Reverse().ToArray();

            byte[] final =
                MySupport.GetBytesArrayFromString(selectedCommand.OpCode.Substring(0, first_dollar))
                    .Concat(paramsBytes)
                    .ToArray();


            return final;
        }
    }
}