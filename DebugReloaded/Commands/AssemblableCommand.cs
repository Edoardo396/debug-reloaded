using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using DebugReloaded.Support;
using Microsoft.SqlServer.Server;

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

        public byte[] Assemble() {

            // IMPLEMENT WORD PTR BYTE PTR

            string GetParameter(string content) {
                if (content == "op1")
                    return this.parms[0];
                return content == "op2" ? this.parms[1] : "";
            }

            string parseStr = selectedCommand.OpCode;

            while (true) {
                var limits = ReplaceLimits(parseStr);

                if (limits.Item1 == -1)
                    break;

                string operand = parseStr.Substring(limits.Item1 + 1, limits.Item2 - limits.Item1 - 1);

                int dp = operand.IndexOf(":");

                string par;

                if (dp == -1)
                    par = GetParameter(operand);
                else {

                    par = GetParameter(operand.Substring(0, dp));

                    string val = par.Replace("[", "").Replace("]", "");
                    MySupport.NormalizeValueString(ref val);

                    byte[] parBytes = MySupport.GetBytesArrayFromString(val);

                    if (operand.Substring(dp + 1, operand.Length - dp - 1) == "le")
                        parseStr = parseStr.Replace($"${operand}$", MySupport.ByteArrayToString(parBytes.Reverse().ToArray()));
                    else
                        parseStr = parseStr.Replace(operand, MySupport.ByteArrayToString(parBytes.ToArray()));
                }
            }


            return MySupport.GetBytesArrayFromString(parseStr);
        }

        private Tuple<int, int> ReplaceLimits(string opCode) {
            int i1 = opCode.IndexOf("$");
            return new Tuple<int, int>(i1, opCode.IndexOf("$", i1 + 1));
        }
    }
}