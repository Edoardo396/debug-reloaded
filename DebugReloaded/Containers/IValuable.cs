using System.Security.Cryptography.X509Certificates;

namespace DebugReloaded.Containers {
    public interface IValuable {

        void ValSetValues(int index, byte[] bytes);

        byte[] ValGetValues(int index, int howmany);       
    }
}