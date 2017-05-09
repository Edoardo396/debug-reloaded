using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.Linq;
using DebugReloaded.Support;

namespace DebugReloaded.Containers {
    /// <summary>
    /// Registro da 2 byte
    /// </summary>
    public class Register : IMemorizable {
        /// <summary>Nome del registro</summary>
        public string Name { get; set; }

        /// <summary>Parte alta</summary>
        public byte H {
            get { return Value[0]; }
            set { Value[0] = value; }
        }

        /// <summary>Parte bassa</summary>
        public byte L {
            get { return Value[1]; }
            set { Value[1] = value; }
        }

        /// <summary>Valore del registro</summary>
        public byte[] Value { get; private set; } = new byte[2] {0x00, 0x00};

        public Register(string name) {
            Name = name;
        }

        public Register() {
        }

        /// <summary>
        /// imposta i valori (IMemorizable)
        /// </summary>
        /// <param name="index">Che valore impostare</param>
        /// <param name="value">Valore</param>
        public void SetValues(int index, byte[] value) {
            var bytes = new byte[2];

            value = MySupport.Normalize(value);

            bytes = value.Length == 1 ? new byte[] {0, value[0]} : value;

            if (bytes.Length != 2)
                throw new BadRegisterException(this, value, "Register is 2 bytes only!");

            this.SetValue(value);
        }

        /// <summary>
        /// Ottene il valore (IMemorizable)
        /// </summary>
        /// <returns>Value</returns>
        public byte[] GetValues(int index = 0, int howmany = 0) {
            return Value;
        }

        /// <summary>
        /// Ne Estrae un puntatore in posizione relativa
        /// </summary>
        /// <param name="index">Da che posizione estrarre</param>
        /// <param name="howmany">Quanti byte estrarre</param>
        /// <returns></returns
        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, index, howmany);
        }

        /// <summary>Lunghezza (IMemorizable)</summary>
        public int Length => 2;

        /// <summary>
        /// imposta i valori (IMemorizable)
        /// </summary>
        /// <param name="value">Valore</param>
        public void SetValue(byte[] _bytes) {
            _bytes = MySupport.Normalize(_bytes);

            var bytes = new byte[2];


            bytes = _bytes.Length == 1 ? new byte[] {0, _bytes[0]} : _bytes;

            if (bytes.Length != 2)
                throw new BadRegisterException(this, bytes, "Cannot insert more than 2 bytes in a register");

            Value = bytes;
        }

        /// <summary>
        /// imposta i valori (IMemorizable)
        /// </summary>
        /// <param name="index">Che valore impostare</param>
        /// <param name="value">Valore (Verrà convertito)</param>
        public virtual void SetValue(string sbytes) {
            if (sbytes.Length != 4)
                throw new BadRegisterException(this, null, "Cannot insert more than 2 bytes in a register");

            Value = sbytes.ToByteArray();
        }

        public override string ToString() {
            return $"{Value[0]:X2}{Value[1]:X2}";
        }

        public static Register operator +(Register a, byte[] b) {
            int value = Int32.Parse(a.Value.ToHexString(), NumberStyles.HexNumber);

            value += Int32.Parse(b.ToHexString(), NumberStyles.HexNumber);

            var ret = new Register(a.Name);

            ret.SetValue(BitConverter.GetBytes(value).Reverse().ToArray());

            return ret;
        }
    }
}