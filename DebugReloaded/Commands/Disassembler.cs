using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public class Disassembler {
        public static AssemblableCommand Dissassemble(ApplicationContext ctx, IMemorizable pointer) {
            AssemblableCommand command;
            int inizioPar = -1;

            var allcmds = ctx.CommandTemplList.Where(el => {
                bool ret = false;

                for (int i = 0; true; i += 2) {
                    if (pointer.Length < i || el.OpCode.Length <= i || el.OpCode.Substring(i, 2).Contains('$')) {
                        inizioPar = i;
                        break;
                    }

                    ret = MySupport.ByteArrayToString(pointer.GetValues(i / 2, 1)) == el.OpCode.Substring(i, 2);
                }
                return ret;
            }).ToList();

            if (allcmds.Count > 1)
                throw new Exception("Multple commands with a single opCode");


            CommandTemplate template = allcmds[0];

            if(!template.OpCode.Contains("$"))
                return new AssemblableCommand(template);

            inizioPar = template.OpCode.IndexOf("$", inizioPar);

            int spar = -1;

            var parms = new string[2];

            if (inizioPar != template.OpCode.Length / 2) {
                int np = sbyte.Parse(template.OpCode[inizioPar + 3].ToString()) - 1;

                sbyte p2size =
                    sbyte.Parse(template.ParTypes[np].ToString().Substring(template.ParTypes[np].ToString().Length - 2));
                p2size /= 8;

                parms[np] = template.OpCode.Substring(inizioPar + 5, 2) != "le"
                    ? MySupport.ByteArrayToString(pointer.GetValues(inizioPar / 2, p2size))
                    : MySupport.ByteArrayToString(pointer.GetValues(inizioPar / 2, p2size).Reverse().ToArray());


                inizioPar = template.OpCode.IndexOf("$", template.OpCode.IndexOf("$", inizioPar + 1) + 1);
                spar = (inizioPar / 2) + p2size;
            }

            // Parameters are always attached.

            if (inizioPar != template.OpCode.Length / 2 && inizioPar != -1) {
                int np = sbyte.Parse(template.OpCode[inizioPar + 3].ToString()) - 1;

                sbyte p2size =
                    sbyte.Parse(template.ParTypes[np].ToString().Substring(template.ParTypes[np].ToString().Length - 2));
                p2size /= 8;

                parms[np] = template.OpCode.Substring(inizioPar + 5, 2) != "le"
                    ? MySupport.ByteArrayToString(pointer.GetValues(spar / 2, p2size))
                    : MySupport.ByteArrayToString(pointer.GetValues(spar / 2, p2size).Reverse().ToArray());
            }

            command = new AssemblableCommand(template, ctx, template.ParTypes, parms);

            return command;
        }
    }
}