using System;
using System.Collections.Generic;
using System.Text;

namespace eseBattagliaNavale_v2
{
    public enum StatoCella
    {
        Vuota,
        Nave,
        Colpita,
        Acqua
    }

    public class Cella
    {
        public int riga { get; }
        public int colonna { get; }
        public StatoCella stato { get; set; }

        public Cella (int r, int c)
        {
            this.riga = r;
            this.colonna = c;
            stato = StatoCella.Vuota;
        }
    }
}