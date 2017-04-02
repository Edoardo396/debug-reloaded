using System.Security.Cryptography.X509Certificates;

namespace DebugReloaded.Commands {
    public interface Assemblable {
        byte[] Assemble();   
        string Disassemble();
    }
}