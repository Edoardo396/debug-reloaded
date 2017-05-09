using System;
using System.Collections.Generic;
using System.Linq;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public class Disassembler {
        public static List<AssemblableCommand> MultiCommandDisassembler(ApplicationContext ctx, IMemorizable pointer) {
            var cmds = new List<AssemblableCommand>();

            for (var index = 0; index < pointer.Length;) {
                int length;

                try {
                    length = GetCommandLength(ctx, pointer.ExtractMemoryPointer(index, pointer.Length - index));
                } catch {
                    // ConsoleLogger.Write("Cannot find a command with OpCode " + (pointer.ExtractMemoryPointer(index, pointer.Length - index)).ToString(), "ERROR", ConsoleColor.Red);
                    index += 1;
                    cmds.Add(new AssemblableCommand(CommandTemplate.UNKNOWN));
                    continue;
                }

                if (length > pointer.Length - index)
                    break;

                cmds.Add(Dissassemble(ctx, pointer.ExtractMemoryPointer(index, length)));

                index += length;
            }

            return cmds;
        }

        public static AssemblableCommand DisassembleNextCommand(ApplicationContext context, IMemorizable memory) {
            AssemblableCommand cmd;
            uint lenght = 0;

            try {
                lenght = (uint) GetCommandLength(context, memory.ExtractMemoryPointer(0, memory.Length));
            } catch {
                cmd = new AssemblableCommand(CommandTemplate.UNKNOWN);
                return cmd;
            }

            cmd = Dissassemble(context, memory.ExtractMemoryPointer(0, (int)lenght));

            return cmd;
        }

        public static int GetCommandLength(ApplicationContext ctx, IMemorizable pointer) {
            int GetLenghtParms(CommandTemplate lenTemplate) {
                if (lenTemplate.OpCode.Count(c => c == '$') / 2 == 2) {
                    return
                        int.Parse(
                            lenTemplate.ParTypes[0].ToString()
                                .Substring(lenTemplate.ParTypes[0].ToString().Length - 2, 2)) / 8 +
                        int.Parse(
                            lenTemplate.ParTypes[1].ToString()
                                .Substring(lenTemplate.ParTypes[1].ToString().Length - 2, 2)) / 8;
                }

                if (lenTemplate.OpCode.Count(c => c == '$') / 2 == 0)
                    return 0;

                int np = sbyte.Parse(lenTemplate.OpCode[lenTemplate.OpCode.IndexOf("$") + 3].ToString()) - 1;
                return
                    int.Parse(
                        lenTemplate.ParTypes[np].ToString().Substring(lenTemplate.ParTypes[np].ToString().Length - 2, 2)) /
                    8;
            }

            CommandTemplate template =
                ctx.CommandTemplList.Find(c => c.OpCode.StartsWith(MySupport.ByteArrayToString(pointer.GetValues(0, 1))));

            // Params are always attched.

            string commandNoParms = template.OpCode.Contains("$")
                ? template.OpCode.Remove(template.OpCode.IndexOf("$")) : template.OpCode;

            return commandNoParms.Length / 2 + GetLenghtParms(template);
        }


        public static AssemblableCommand Dissassemble(ApplicationContext ctx, IMemorizable pointer) {
            AssemblableCommand command;
            int inizioPar = -1;

            List<CommandTemplate> allcmds = ctx.CommandTemplList.Where(el => {
                var ret = true;

                for (var i = 0;; i += 2) {
                    if (pointer.Length < i || el.OpCode.Length <= i || el.OpCode.Substring(i, 2).Contains('$') || !ret) {
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

            if (!template.OpCode.Contains("$"))
                return new AssemblableCommand(template);

            inizioPar = template.OpCode.IndexOf("$" /*, inizioPar*/); // TOLTO A CASO PERCHE NON ANDAVA

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
                spar = inizioPar / 2 + p2size;
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