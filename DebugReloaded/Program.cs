using System;
using DebugReloaded.Interface;
using DebugReloadedCore.Support;

namespace DebugReloaded {
    internal class Program {
        private static void Main(string[] args) {
            ConsoleLogger.Write("Avvio in corso, aspetta mentre carico il database: ", "MESSAGE", ConsoleColor.Blue);

            // Console.Write("Ready: ");
            var context = new ApplicationContext();
            context.Interpreter.WaitForCommands();

            Console.ReadKey();
        }
    }
}