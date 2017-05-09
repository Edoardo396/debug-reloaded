using System;
using System.Linq;
using System.Text;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public class AssemblableCommand : Assemblable {

        private readonly string[] parms;

        protected readonly ApplicationContext Context;

        private readonly DataType[] parmsDt;
        protected readonly CommandTemplate selectedCommand;

        public int Length => this.Assemble().Length;

        public AssemblableCommand(CommandTemplate cmd, ApplicationContext ctx = null) {
            selectedCommand = cmd;
            Context = ctx;
        }

        public AssemblableCommand(CommandTemplate selectedCommand, ApplicationContext context, DataType[] parmsDt,
            string[] parms) {
            this.selectedCommand = selectedCommand;
            this.Context = context;
            this.parmsDt = parmsDt;
            this.parms = parms;
        }

        public AssemblableCommand(ApplicationContext ctx, string instruct) {
            Context = ctx;

            string cmd = instruct.Split((char) 32)[0];
            parms = instruct.Split((char) 32)[1].Split(',');

            parmsDt = new DataType[2] {DataType.None, DataType.None};

            for (var i = 0; i < parms.Length; i++)
                parmsDt[i] = CommandTemplate.GetDTFromArgument(parms[i]);

            var parmsDts = new string[2];

            for (var i = 0; i < parms.Length; i++)
                parmsDts[i] = CommandTemplate.DataTypeToString(parmsDt[i]);

            selectedCommand =
                Context.CommandTemplList.Find(
                    c =>
                        c.Name == cmd.Replace(" ", "") && c.ParTypes.SequenceEqual(parmsDt) &&
                        this.CheckParSpecific(c, parms));


            if (selectedCommand == null)
                throw new Exception("No Command matching criteria.");
        }

        public AssemblableCommand()
            : this(new CommandTemplate("", new[] {DataType.None, DataType.None}, new[] {"any", "any"}, string.Empty)) {
        }

        public byte[] Assemble() {
            // IMPLEMENT WORD PTR BYTE PTR

            string GetParameter(string content) {
                if (content == "op1")
                    return parms[0];
                return content == "op2" ? parms[1] : "";
            }

            string parseStr = selectedCommand.OpCode;

            while (true) {
                Tuple<int, int> limits = this.ReplaceLimits(parseStr);

                if (limits.Item1 == -1)
                    break;

                string operand = parseStr.Substring(limits.Item1 + 1, limits.Item2 - limits.Item1 - 1);

                int dp = operand.IndexOf(":");

                string par;

                if (dp == -1) {
                    par = GetParameter(operand);
                } else {
                    par = GetParameter(operand.Substring(0, dp));

                    string val = par.Replace("[", "").Replace("]", "");
                    MySupport.NormalizeValueString(ref val);

                    byte[] parBytes = MySupport.GetBytesArrayFromString(val);

                    if (operand.Substring(dp + 1, operand.Length - dp - 1) == "le")
                        parseStr = parseStr.Replace($"${operand}$",
                            MySupport.ByteArrayToString(parBytes.Reverse().ToArray()));
                    else
                        parseStr = parseStr.Replace(operand, MySupport.ByteArrayToString(parBytes.ToArray()));
                }
            }


            return MySupport.GetBytesArrayFromString(parseStr);
        }

        private bool CheckParSpecific(CommandTemplate c, string[] par) {
            return !par.Where((t, i) => c.ParSpecific[i] != t && c.ParSpecific[i] != "any").Any();
        }

        private Tuple<int, int> ReplaceLimits(string opCode) {
            int i1 = opCode.IndexOf("$");
            return new Tuple<int, int>(i1, opCode.IndexOf("$", i1 + 1));
        }

        public override string ToString() {
            var builder = new StringBuilder();

            builder.Append(selectedCommand.Name + " ");


            for (var i = 0; i < 2; i++) {
                if (selectedCommand.ParTypes[i] == DataType.None)
                    continue;

                if (selectedCommand.ParTypes[i] == DataType.Memory16 || selectedCommand.ParTypes[i] == DataType.Memory8)
                    builder.Append("[");

                builder.Append(selectedCommand.ParSpecific[i] == "any" ? parms[i] : selectedCommand.ParSpecific[i]);

                if (selectedCommand.ParTypes[i] == DataType.Memory16 || selectedCommand.ParTypes[i] == DataType.Memory8)
                    builder.Append("]");

                if (i != 1 && selectedCommand.ParTypes[i + 1] != DataType.None)
                    builder.Append(',');
            }

            return builder.ToString();
        }
    }
}