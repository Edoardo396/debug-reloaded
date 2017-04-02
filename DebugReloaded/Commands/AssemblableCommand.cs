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

        private DataType[] parmsDT;
        private string[] parmsDTS;
        private string[] parms;

        public AssemblableCommand(CommandTemplate cmd) {
            selectedCommand = cmd;
        }

        public AssemblableCommand(ApplicationContext ctx, string instruct) {
            context = ctx;

            string cmd = instruct.Split((char) 32)[0];
            parms = instruct.Split((char) 32)[1].Split(',');

            parmsDT = new DataType[2];

            for (var i = 0; i < parms.Length; i++)
                parmsDT[i] = CommandTemplate.GetDTFromArgument(parms[i]);

            parmsDTS = new string[2];

            for (var i = 0; i < parms.Length; i++)
                parmsDTS[i] = CommandTemplate.DataTypeToString(parmsDT[i]);

//            selectedCommand = context.CommandTemplList.Find(
//                c => c.Name == cmd.Replace(" ", "") &&
//                    c.ParTypes.SequenceEqual(parmsDT) &&
//                    (c.ParSpecific.SequenceEqual(parms) || (c.ParSpecific[0] == "any" && c.ParSpecific[1] == "any")));

            selectedCommand =
                context.CommandTemplList.Find(
                    c =>
                        c.Name == cmd.Replace(" ", "") && c.ParTypes.SequenceEqual(parmsDT) &&
                        (c.ParSpecific[0] == parms[0] || c.ParSpecific[0] == "any") &&
                        (c.ParSpecific[1] == parms[1] || c.ParSpecific[1] == "any"));


            if (selectedCommand == null)
                throw new Exception("No Command matching  criteria.");
        }

        public string Disassemble() {
            throw new System.NotImplementedException();
        }

        public byte[] Assemble() {

            byte[] final;

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

            string parms = GetParameter(fpar).Replace("[","").Replace("]", "");

            MySupport.NormalizeValueString(ref parms);

            byte[] paramsBytes = MySupport.GetBytesArrayFromString(parms);

            if (format == "le")
                paramsBytes = paramsBytes.Reverse().ToArray();

            final =
                MySupport.GetBytesArrayFromString(selectedCommand.OpCode.Substring(0, first_dollar))
                    .Concat(paramsBytes).ToArray();

                
            return final;
        }
    }
}