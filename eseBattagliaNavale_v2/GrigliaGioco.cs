using System;
using System.Collections.Generic;
using System.Text;

namespace eseBattagliaNavale_v2
{
    public delegate void EsitoColpoHandler(int x, int y, string messaggio, bool isColpito);

    public class GrigliaGioco
    {
        public const int dimGriglia = 10;
        public Cella[,] matriceGioco { get; private set; }
        public List<Nave> listaNavi { get; private set; }
        public int mosseEffettuate { get; private set; }
        public int NaviRimanenti
        {
            get
            {
                int conta = 0;
                for(int i = 0; i < listaNavi.Count; i++)
                {
                    if (!listaNavi[i].isAffondata)
                    {
                        conta++;
                    }
                }
                return conta;
            }
        }

        public event EsitoColpoHandler? OnNaveColpita;
        public event EsitoColpoHandler? OnAcqua;

        public GrigliaGioco()
        {
            this.matriceGioco = new Cella[dimGriglia, dimGriglia];
            for(int r = 0; r < dimGriglia; r++)
            {
                for(int c = 0;  c < dimGriglia; c++)
                {
                    matriceGioco[r, c] = new Cella(r, c);
                }
            }

            this.listaNavi = new List<Nave>();
            this.mosseEffettuate = 0;
        }

        public bool PosizionaNave(int rig, int col, int lung, bool isOrizz)
        {
            if (isOrizz && col + lung > dimGriglia) return false;
            if (!isOrizz && rig + lung > dimGriglia) return false;

            for(int i = 0; i < lung; i++)
            {
                int r = isOrizz ? rig : rig + i;
                int c = isOrizz ? col + i : col;

                if (matriceGioco[r, c].stato != StatoCella.Vuota)
                {
                    return false;
                }
            }


            Nave nuovaNave = new Nave(lung);
            for(int i = 0; i < lung; i++)
            {
                int r = isOrizz ? rig : rig + i;
                int c = isOrizz ? col + i : col;

                matriceGioco[r, c].stato = StatoCella.Nave;
                nuovaNave.coordinate.Add(matriceGioco[r, c]);
            }

            listaNavi.Add(nuovaNave);
            return true;
        }

        public Nave? TrovaNaveInPosizione(int x, int y)
        {
            for(int i = 0; i < listaNavi.Count; i++)
            {
                var nave = listaNavi[i];
                for (int j = 0; j < nave.coordinate.Count; j++)
                {
                    var coord = nave.coordinate[j];
                    if(coord.riga == x && coord.colonna == y)
                    {
                        return nave;
                    }
                }
            }

            return null;
        }

        public void Spara(int riga, int col)
        {
            Cella cella = matriceGioco[riga, col];

            if(cella.stato == StatoCella.Colpita || cella.stato == StatoCella.Acqua)
                return;

            mosseEffettuate++;

            if(cella.stato == StatoCella.Nave)
            {
                cella.stato = StatoCella.Colpita;
                Nave? naveColpita = TrovaNaveInPosizione(riga, col);
                naveColpita?.RegistraColpo();

                string mess = $"Nave colpita in posizione ({riga}, {col})";
                if(naveColpita != null && naveColpita.isAffondata)
                {
                    mess += " --> Nave affondata!";
                }

                OnNaveColpita?.Invoke(riga, col, mess, true);
            } else
            {
                cella.stato = StatoCella.Acqua;
                OnAcqua?.Invoke(riga, col, "Acqua!", false);
            }
        }

        public bool RimuoviUltimaNave()
        {
            if (listaNavi.Count == 0) return false;

            Nave ultimaNave = listaNavi[listaNavi.Count - 1];

            //annullamento posizione: celle tornano vuote
            for (int i = 0; i < ultimaNave.coordinate.Count; i++)
            {
                Cella c = ultimaNave.coordinate[i];
                matriceGioco[c.riga, c.colonna].stato = StatoCella.Vuota;
            }

            listaNavi.RemoveAt(listaNavi.Count - 1);
            return true;
        }
    }
}