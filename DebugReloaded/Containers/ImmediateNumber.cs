using System;
using DebugReloaded.Interface;

namespace DebugReloaded.Containers {

    /// <summary>
    /// Raprresenta un valore Immediato
    /// </summary>
    public sealed class ImmediateNumber : IMemorizable {
        /// <summary>Contenuto</summary>
        private readonly byte[] content;

        /// <summary>
        /// Crea un valore immediato
        /// </summary>
        /// <param name="content">Contenuto</param>
        /// <param name="isWord">Specificare se è un byte o una word</param>
        public ImmediateNumber(byte[] content, bool isWord) {
            if (content.Length > 2 || content.Length < 1)
                throw new Exception("Bad length");
            this.content = content;

            if (isWord && content.Length == 1)
                content = new byte[] {0x0, content[0]};
        }

        /// <summary>
        /// Imposta il valore immediato specificato
        /// </summary>
        /// <param name="index">Indice</param>
        /// <param name="bytes">bytes</param>
        public void SetValues(int index, byte[] bytes) {
            ConsoleLogger.Write("Assigning a value to a number. Be Careful", "WARNING", ConsoleColor.Yellow);
            for (var i = 0; i < bytes.Length; i++)
                content[i + index] = bytes[i];
        }

        /// <summary>
        /// Ottiene valore
        /// </summary>
        /// <param name="index">Indice valore</param>
        /// <param name="howmany">Quanti valori</param>
        /// <returns></returns>
        public byte[] GetValues(int index, int howmany) {
            var bytes = new byte[howmany];

            for (var i = 0; i < howmany; i++)
                bytes[i] = content[i + index];


            return bytes;
        }

        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, index, howmany);
        }

        /// <summary>
        /// Lunghezza Valore
        /// </summary>
        public int Length => content.Length;
    }
}