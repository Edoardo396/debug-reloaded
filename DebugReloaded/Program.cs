using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Containers;
using DebugReloaded.Interface;
using DebugReloaded.Support;

namespace DebugReloaded {
    class Program {
        static void Main(string[] args) {
            
            ConsoleLogger.Write("Avvio in corso, aspetta mentre carico il database: ", "MESSAGE", ConsoleColor.Blue);

            // Console.Write("Ready: ");
            ApplicationContext context = new ApplicationContext();
            context.Interpreter.WaitForCommands();

            Console.ReadKey();
        }
    }
}
