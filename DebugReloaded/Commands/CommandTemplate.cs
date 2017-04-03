using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;
using DebugReloaded.Support;

namespace DebugReloaded.Commands {
    public class CommandTemplate {


        public static ApplicationContext ctx;
        public string Name { get; private set; }
        public DataType[] ParTypes { get; private set; }
        public string[] ParSpecific { get; private set; }
        public string OpCode { get; private set; }

        public CommandTemplate(string name, DataType[] parTypes, string[] parSpecific, string opCode) {
            Name = name;
            ParTypes = parTypes;
            ParSpecific = parSpecific;
            OpCode = opCode;
        }

        public CommandTemplate(string name, string[] parTypes, string[] parSpecific, string opCode) {
            ParTypes = new DataType[parTypes.Length];

            for (int i = 0; i < ParTypes.Length; i++) {
                ParTypes[i] = DataTypeParse(parTypes[i]);
            }

            Name = name;
            ParSpecific = parSpecific;
            OpCode = opCode;
        }

        public static DataType GetDTFromArgument(string srg) {
            
            if(ctx.Registers.Find(r => r.Name == srg) != null)
                return DataType.Register16;

            if(srg.IndexOf('[') < srg.IndexOf(']') && srg.IndexOf(']') != -1 && srg.IndexOf('[') != -1)
                return DataType.Memory16;

            if(int.TryParse(srg,NumberStyles.HexNumber,CultureInfo.InvariantCulture,out int res))
                return DataType.Immediate16;

            throw new Exception(); 
        }


        public static DataType DataTypeParse(string str) {
            switch (str) {
                case "r16":
                    return DataType.Register16;
                case "r8":
                    return DataType.Register8;
                case "i16":
                    return DataType.Immediate16;
                case "i8":
                    return DataType.Immediate8;
                case "f":
                    return DataType.Flag;
                case "m8":
                    return DataType.Memory8;
                case "m16":
                    return DataType.Memory16;
                case "none":
                    return DataType.None;
                default:
                    throw new ArgumentException("type not found");
            }
        }

        public static string DataTypeToString(DataType dt) {
            switch (dt) {
                case DataType.Register8:
                    return "r8";
                case DataType.Register16:
                    return "r16";
                case DataType.Memory8:
                    return "m8";
                case DataType.Memory16:
                    return "m16";
                case DataType.Immediate8:
                    return "i8";
                case DataType.Immediate16:
                    return "i16";
                case DataType.Flag:
                    return "f";
                case DataType.None:
                    return "none";
                default:
                    throw new ArgumentException("type not found");
            }
        }
        public static List<CommandTemplate> GetCommandsFromXML(XDocument doc) {
            List<CommandTemplate> commands = new List<CommandTemplate>();

            var commandsNode = doc.Element("database").Element("Commands");

            foreach (var element in commandsNode.Elements("command"))
                commands.Add(new CommandTemplate(element.Attribute("name").Value,
                    new[] {element.Attribute("op1").Value, element.Attribute("op2").Value},
                    new[] {element.Attribute("op1spec").Value, element.Attribute("op2spec").Value},
                    element.Attribute("opcode").Value));

            return commands;
        }

        public AssemblableCommand ToAssemblableCommand(string query) {

            string[] param = query.Split(' ')[1].Split(',');

            return new AssemblableCommand(this);
        }

    }
}