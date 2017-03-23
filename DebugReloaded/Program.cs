using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebugReloaded.Containers;
using DebugReloaded.Support;

namespace DebugReloaded {
    class Program {
        static void Main(string[] args) {
            
            ApplicationContext context = new ApplicationContext();
            context.Interpreter.WaitForCommands();

            Console.ReadKey();
        }
    }
}
