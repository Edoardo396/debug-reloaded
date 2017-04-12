using System.Security.Cryptography.X509Certificates;

namespace DebugReloaded.Containers {
    public interface IMemorizable {
        void SetValues(int index, byte[] value);
        byte[] GetValues(int index, int howmany);
        int Length { get; }
        MemoryRangePointer ExtractMemoryPointer(int index, int howmany);
    }
}