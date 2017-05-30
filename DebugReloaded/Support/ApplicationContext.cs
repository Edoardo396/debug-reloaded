using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using DebugReloaded.Commands;
using DebugReloaded.Containers;
using DebugReloaded.Interface;
using System.IO;
using System.Net.Mime;

namespace DebugReloaded.Support {
    /// <summary>
    /// Contesto dell'applicazione, contiene tutti i dati dell'applicazione corrente.
    /// </summary>
    public class ApplicationContext {

        /// <summary>Attiva modalità debug.</summary>
        public static readonly bool Verbose = true;

        /// <summary>Documento XML con i comandi</summary>
        public static XDocument doc;

        ///<summary>Definisce il percorso del database</summary>
        private static string docLoc =
            @"..\..\Commands\AssemblyCommands.xml";


        /// <summary>Dimensione memoria RAM</summary>
        public static readonly int memSize = 65535;

        /// <summary>Assemblatore di comandi assembly</summary>
        public Assembler CommandAssembler;

        /// <summary>Interprete comandi debug</summary>
        public CommandInterpreter Interpreter;

        /// <summary>Memoria RAM principale</summary>
        public Memory MainMemory = new Memory(memSize);

        /// <summary>Lista domandi da eseguire (non usati in questa versione)</summary>
        /// public List<CommandTemplate> Program = new List<CommandTemplate>();
        /// <summary>Versione Applicazione</summary>
        public static Version AppVersion
            => AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;

        /// <summary>Lista template comandi da file</summary>
        public List<CommandTemplate> CommandTemplList { get; }

        /// <summary>Lista dei registri</summary>
        public List<Register> Registers { get; } = new List<Register> {
            new Register("ax"),
            new Register("bx"),
            new Register("cx"),
            new Register("dx"),
            new Register("si"),
            new Register("di"),
            new Register("cs"),
            new Register("ds"),
            new Register("ip")
        };

        public List<Flag> Flags { get; } = new List<Flag> {
            new Flag("cf"),
            new Flag("af"),
            new Flag("pf"),
            new Flag("zf"),
            new Flag("sf"),
            new Flag("of")
        };


        public ApplicationContext() {
            ConsoleLogger.Write("Potranno essere assemblati ed eseguiti solo i comandi presenti nel database!",
                "WARNING", ConsoleColor.DarkYellow);

            while (!File.Exists(docLoc)) {
                ConsoleLogger.Write("File di database non trovato", "ERROR", ConsoleColor.Red);
                docLoc = MySupport.CWR("Inserire percorso database => ");
            }

            try {
                doc = XDocument.Load(docLoc);
            }
            catch (Exception e) {
                ConsoleLogger.Write("Non sono riuscito a leggere il database correttmente, controllane le struttura.\nDettagli: "  + e.Message, "ERROR", ConsoleColor.Red);
                Environment.Exit(1);
            }

            Interpreter = new CommandInterpreter(this);
            CommandAssembler = new Assembler(this);
            // TODO REPLACE WITH PARAMS
            CommandTemplate.ctx = this;
            CommandTemplList = CommandTemplate.GetCommandsFromXML(doc);

            ConsoleLogger.Write("Oggetti costruiti con successo, in attesa di comandi: ", "MESSAGE", ConsoleColor.Green);
        }

        /// <summary>Shortcut per ottenere il registro dato un nome</summary>
        public Register GetRegisterByName(string name) {
            return Registers.Find(r => r.Name == name);
        }

        public Flag GetFlagByName(string name) {
            return Flags.Find(r => r.Name == name);
        }
    }
}