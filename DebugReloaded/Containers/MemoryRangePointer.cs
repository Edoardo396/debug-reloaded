namespace DebugReloaded.Containers {

    /// <summary>
    /// Ottiene puntatore a un memorizzabile, quando viene modificato il puntatore le modifiche vengono salvatre sulla memoria originale.
    /// </summary>
    public class MemoryRangePointer : Memory {

        /// <summary>
        /// Memoria originale
        /// </summary>
        private readonly IMemorizable originalMemory;

        /// <summary>
        /// Aggiornare memoria automaticamente?
        /// </summary>
        public bool AutoUpdate { get; set; } = true;

        /// <summary>
        /// Primo byte
        /// </summary>
        public int From { get; }
        /// <summary>
        /// Ultimo byte
        /// </summary>
        public int To { get; private set; }

        /// <summary>
        /// Overload [] operator
        /// </summary>
        public new byte this[int index] {
            get { return this.GetValues(index, 1)[0]; }
            set { this.SetValues(index, new[] {value}); }
        }


        public MemoryRangePointer(IMemorizable originalMemory, int from, int howmany) : base(howmany) {
            this.originalMemory = originalMemory;
            From = from;
            To = from + howmany;

            content = originalMemory.GetValues(from, howmany);
        }

        /// <summary>
        /// Aggiorna memoria originale
        /// </summary>
        public void Update() {
            originalMemory.SetValues(From, content);
        }

        /// <summary>
        /// Imposta valori e aggiorna (se AutoUpdate)
        /// </summary>
        /// <param name="index">Primo byte</param>
        /// <param name="value">Valori</param>
        public override void SetValues(int index, byte[] value) {
            base.SetValues(index, value);

            if (AutoUpdate)
                this.Update();
        }
    }
}