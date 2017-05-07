namespace DebugReloaded.Containers {
    public class Memory : IMemorizable {

        /// <summary>Contenuto Memoria</summary>
        protected byte[] content;

        /// <summary>Nome Memoria</summary>
        public string Name { get; set; }

        /// <summary>Overload []</summary>
        public byte this[int index] => this.GetValue(index);

        /// <summary>
        /// Crea memoria vuota
        /// </summary>
        /// <param name="byteSize">Dimensione memoria</param>
        public Memory(int byteSize) {
            content = new byte[byteSize];
        }

        /// <summary>
        /// Crea memoria utilizzando i bytes dati.
        /// </summary>
        /// <param name="contentBytes">Contenuto memoria</param>
        public Memory(byte[] contentBytes) {
            content = (byte[])contentBytes.Clone();
        }

        /// <summary>Dimensione Memoria</summary>
        public int Length => content.Length;

        /// <summary>
        /// Ottiene valori memoria
        /// </summary>
        /// <param name="index">Indice valore</param>
        /// <param name="howmany">Quanti valori</param>
        public byte[] GetValues(int index, int howmany) {
            var bytes = new byte[howmany];

            for (var i = 0; i < howmany; i++)
                bytes[i] = content[i + index];


            return bytes;
        }

        /// <summary>
        /// Imposta valori memoria
        /// </summary>
        /// <param name="index">Indice valore</param>
        /// <param name="bytes">Valori da impostare</param>
        public virtual void SetValues(int index, byte[] bytes) {
            for (var i = 0; i < bytes.Length; i++)
                content[i + index] = bytes[i];
        }

        /// <summary>
        /// Estrai un MemoryRangePointer
        /// </summary>
        /// <param name="index">Posizione iniziale</param>
        /// <param name="howmany">Numero di bytes da estrarre</param>
        public MemoryRangePointer ExtractMemoryPointer(int index, int howmany) {
            return new MemoryRangePointer(this, index, howmany);
        }

        /// <summary>
        /// Cancella tutta la memoria
        /// </summary>
        public void Reset() {
            for (var i = 0; i < content.Length; i++)
                content[i] = 0x00;
        }

        /// <summary>
        /// Ottinene un singolo byte
        /// </summary>
        /// <param name="index">indice del byte></param>
        public byte GetValue(int index) {
            return this.GetValues(index, 1)[0];
        }
        
        /// <summary>
        /// Imposta un singolo valore
        /// </summary>
        /// <param name="index">Indice byte</param>
        /// <param name="value">Valore da impostare</param>
        public virtual void SetValue(int index, byte value) {
            this.SetValues(index, new[] {value});
        }

        /// <summary>
        /// Ottiene il contenuto della memoria come stringa
        /// </summary>
        /// <param name="startindex">Inizio dump</param>
        /// <param name="howmany">Quanti bytes</param>
        /// <returns>Stringa che identifica la memoria</returns>
        public string Dump(int startindex, int howmany) {
            string str = string.Empty;

            for (var i = 0; i < howmany; i++)
                str += $"{content[i + startindex]:X2}-";

            str = str.Remove(str.Length - 1);

            return str;
        }

        /// <summary>
        /// Fa il dump di tutta la memoria (Equivale a Dump(0, mem.Length))
        /// </summary>
        /// <returns></returns>
        public string Dump() {
            return this.Dump(0, content.Length);
        }

        /// <summary>
        /// Imposta in memoria i valori in LE
        /// </summary>
        /// <param name="startIndex">Indice</param>
        /// <param name="_bytes">Valori da impostae</param>
        public void SetValuesLE(int startIndex, byte[] _bytes) {
            var bytes = new byte[2];

            bytes = _bytes.Length == 1 ? new byte[] {0, _bytes[0]} : _bytes;

            if (bytes.Length != 2)
                throw new BadMemoryException(this, startIndex,
                    "In little endian mode yot can't set more then 2 values at a time");

            content[startIndex] = bytes[1];
            content[startIndex + 1] = bytes[0];
        }

        /// <summary>
        /// Ottine i valori in LE
        /// </summary>
        /// <param name="startindex">Indice di partenza</param>
        /// <returns></returns>
        public byte[] GetValuesLE(int startindex) {
            return new[] {content[startindex + 1], content[startindex]};
        }

        /// <summary>
        /// Estrae una memoria con i valori specificati
        /// </summary>
        /// <param name="startindex">Indice</param>
        /// <param name="howmany">Numero di bytes</param>
        /// <returns></returns>
        public Memory SubMemory(int startindex, int howmany) {
            byte[] bytes = this.GetValues(startindex, howmany);

            return new Memory(bytes);
        }
    }
}