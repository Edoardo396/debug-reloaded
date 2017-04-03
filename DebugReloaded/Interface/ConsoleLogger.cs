using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReloaded.Interface {
   public static class ConsoleLogger {

       public static void Write(object text, string type, ConsoleColor color = ConsoleColor.White) {

           var oldcolor = Console.ForegroundColor;

           Console.ForegroundColor = color;

            Console.WriteLine($"{DateTime.Now:T} [{type}] {text.ToString()}");

           Console.ForegroundColor = oldcolor;
       }




    }
}
