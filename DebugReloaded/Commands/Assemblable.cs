namespace DebugReloaded.Commands {
    public interface Assemblable {
        byte[] Assemble();
    }

    public interface Disassemblable {
        byte[] Disassemble();
    }
}