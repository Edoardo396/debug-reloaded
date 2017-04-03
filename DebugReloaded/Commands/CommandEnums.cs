namespace DebugReloaded.Commands {

    public enum DataType {
        
        Register8, Register16, Memory8, Memory16, Immediate8, Immediate16, Flag, None

    }


    public enum AddressMode {
        
        Immediate, Register, Indexed, Direct, Indirect

    }
}