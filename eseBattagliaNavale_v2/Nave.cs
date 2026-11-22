using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace eseBattagliaNavale_v2
{
    public class Nave
    {
        public int lunghezzaNave { get; }
        public int partiColpite { get; private set; }
        public bool isAffondata => partiColpite >= lunghezzaNave;

        public List<Cella> coordinate { get; } = new List<Cella>();

        public Nave (int l)
        {
            this.lunghezzaNave = l;
            this.partiColpite = 0;
        }

        public void RegistraColpo()
        {
            partiColpite++;
        }
    }
}