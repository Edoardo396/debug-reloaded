namespace DebugReloaded.Containers {
    public interface IMemorizable {
        int Length { get; }
        void SetValues(int index, byte[] value);
        byte[] GetValues(int index, int howmany);
        MemoryRangePointer ExtractMemoryPointer(int index, int howmany);
    }
}