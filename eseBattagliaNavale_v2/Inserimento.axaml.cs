using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System.Collections.Generic;

namespace eseBattagliaNavale_v2
{
    public partial class Inserimento : Window
    {
        private GrigliaGioco campoP1 = new GrigliaGioco();
        private GrigliaGioco campoP2 = new GrigliaGioco();

        private Button[,] matricedBottoni = new Button[10, 10];
        private readonly List<int> flotta = new List<int> { 4, 3, 3, 2, 2, 1 };

        private int indiceNaveCorrente = 0;
        private bool isOrizzontale = true;
        private int turnoPosizionamento = 1; // 1 = Posiziona G1, 2 = Posiziona G2

        public Inserimento()
        {
            InitializeComponent();
            CreaGrigliaGrafica();
            AggiornaTesto();
        }

        private void CreaGrigliaGrafica()
        {
            grdPosizionamento.Children.Clear();
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    int riga = r;
                    int col = c;

                    Button btn = new Button
                    {
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                        Margin = new Avalonia.Thickness(1),
                        Background = Brushes.LightBlue
                    };

                    btn.Click += (s, e) => CellaCliccata(riga, col);
                    matricedBottoni[r, c] = btn;
                    grdPosizionamento.Children.Add(btn);
                }
            }
        }

        private void CellaCliccata(int r, int c)
        {
            if (indiceNaveCorrente >= flotta.Count) return;

            int lunghezza = flotta[indiceNaveCorrente];
            GrigliaGioco campoAttivo = (turnoPosizionamento == 1) ? campoP1 : campoP2;

            bool successo = campoAttivo.PosizionaNave(r, c, lunghezza, isOrizzontale);

            if (successo)
            {
                indiceNaveCorrente++;
                AggiornaColoriGriglia();
                AggiornaTesto();
            }
        }

        private void BtnAnnulla_Click(object? sender, RoutedEventArgs e)
        {
            GrigliaGioco campoAttivo = (turnoPosizionamento == 1) ? campoP1 : campoP2;

            if (indiceNaveCorrente > 0)
            {
                bool rimosso = campoAttivo.RimuoviUltimaNave();
                if (rimosso)
                {
                    indiceNaveCorrente--;
                    AggiornaColoriGriglia();
                    AggiornaTesto();
                }
            }
        }

        private void AggiornaColoriGriglia()
        {
            GrigliaGioco campoAttivo = (turnoPosizionamento == 1) ? campoP1 : campoP2;

            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    if (campoAttivo.matriceGioco[r, c].stato == StatoCella.Nave)
                        matricedBottoni[r, c].Background = Brushes.DarkSlateGray;
                    else
                        matricedBottoni[r, c].Background = Brushes.LightBlue;
                }
            }
        }

        private void AggiornaTesto()
        {
            if (indiceNaveCorrente < flotta.Count)
            {
                Title = $"Fase 1: Posizionamento Navi - Turno Giocatore {turnoPosizionamento}";
                txtIstruzioni.Text = $"Giocatore {turnoPosizionamento}: Posiziona nave da {flotta[indiceNaveCorrente]} celle ({indiceNaveCorrente + 1}/{flotta.Count})";
                btnVaiAlGioco.IsEnabled = false;
            }
            else
            {
                txtIstruzioni.Text = $"Flotta Giocatore {turnoPosizionamento} pronta!";
                btnVaiAlGioco.IsEnabled = true;
            }
        }

        private void BtnOrientamento_Click(object? sender, RoutedEventArgs e)
        {
            isOrizzontale = !isOrizzontale;
            btnOrientamento.Content = isOrizzontale ? "Orientamento: Orizzontale (destra)" : "Orientamento: Verticale (giù)";
        }

        private void BtnReset_Click(object? sender, RoutedEventArgs e)
        {
            if (turnoPosizionamento == 1)
                campoP1 = new GrigliaGioco();
            else
                campoP2 = new GrigliaGioco();

            indiceNaveCorrente = 0;
            AggiornaColoriGriglia();
            AggiornaTesto();
        }

        private void BtnVaiAlGioco_Click(object? sender, RoutedEventArgs e)
        {
            if (turnoPosizionamento == 1)
            {
                turnoPosizionamento = 2;
                indiceNaveCorrente = 0;
                AggiornaColoriGriglia();
                AggiornaTesto();
            }
            else
            {
                Gioco finestraGame = new Gioco(campoP1, campoP2);
                finestraGame.Show();
                this.Close();
            }
        }
    }
}