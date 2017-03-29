namespace DebugReloaded.Containers {
    public interface IMemorizable {
        void SetValues(int index, byte[] value);
        byte[] GetValues(int index, int howmany);
        MemoryRangePointer ExtractMemoryPointer(int index, int howmany);
    }
}