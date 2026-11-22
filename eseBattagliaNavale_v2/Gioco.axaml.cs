using Avalonia.Controls;
using Avalonia.Media;

namespace eseBattagliaNavale_v2
{
    public partial class Gioco : Window
    {
        private GrigliaGioco campoP1;
        private GrigliaGioco campoP2;
        private Button[,] matricedBottoni = new Button[10, 10];

        private int turnoGiocatore = 1; // 1 = Giocatore 1 spara, 2 = Giocatore 2 spara

        public Gioco() : this(new GrigliaGioco(), new GrigliaGioco()) { }

        public Gioco(GrigliaGioco p1, GrigliaGioco p2)
        {
            InitializeComponent();
            campoP1 = p1;
            campoP2 = p2;

            // Iscriviamo entrambi i campi agli eventi
            campoP1.OnNaveColpita += GestisciRisultatoSparo;
            campoP1.OnAcqua += GestisciRisultatoSparo;
            campoP2.OnNaveColpita += GestisciRisultatoSparo;
            campoP2.OnAcqua += GestisciRisultatoSparo;

            CreaGrigliaBattaglia();
            AggiornaInterfaccia();
        }

        private void CreaGrigliaBattaglia()
        {
            grdBattaglia.Children.Clear();
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
                        Background = Brushes.LightSteelBlue
                    };

                    // Se è il turno del G1, spara sulla flotta del G2. Se è il turno del G2, spara sulla flotta del G1.
                    btn.Click += (s, e) =>
                    {
                        if (turnoGiocatore == 1)
                            campoP2.Spara(riga, col);
                        else
                            campoP1.Spara(riga, col);
                    };

                    matricedBottoni[r, c] = btn;
                    grdBattaglia.Children.Add(btn);
                }
            }
        }

        private void GestisciRisultatoSparo(int x, int y, string messaggio, bool isColpito)
        {
            // COLORE DIFFERENZIATO: Rosso per colpito, Blu oceano per l'acqua
            matricedBottoni[x, y].Background = isColpito ? Brushes.Crimson : Brushes.DodgerBlue;
            matricedBottoni[x, y].IsEnabled = false;

            lstLog.Items.Insert(0, $"[G{turnoGiocatore}] {messaggio}");

            // Controllo Vittoria
            if (turnoGiocatore == 1 && campoP2.NaviRimanenti == 0)
            {
                lstLog.Items.Insert(0, "VITTORIA! Il Giocatore 1 ha distrutto la flotta avversaria!");
                DisabilitaGriglia();
                return;
            }
            else if (turnoGiocatore == 2 && campoP1.NaviRimanenti == 0)
            {
                lstLog.Items.Insert(0, "VITTORIA! Il Giocatore 2 ha distrutto la flotta avversaria!");
                DisabilitaGriglia();
                return;
            }

            // Cambio turno
            turnoGiocatore = (turnoGiocatore == 1) ? 2 : 1;
            AggiornaInterfaccia();
            SincronizzaGrigliaVisiva();
        }

        private void AggiornaInterfaccia()
        {
            if (turnoGiocatore == 1)
            {
                txtMosse.Text = $"Turno: GIOCATORE 1 (Mosse G1: {campoP2.mosseEffettuate})";
                txtNaviRimanenti.Text = $"Navi nemiche rimaste: {campoP2.NaviRimanenti}";
            }
            else
            {
                txtMosse.Text = $"Turno: GIOCATORE 2 (Mosse G2: {campoP1.mosseEffettuate})";
                txtNaviRimanenti.Text = $"Navi nemiche rimaste: {campoP1.NaviRimanenti}";
            }
        }

        // Aggiorna i colori dei bottoni in base allo stato del campo del giocatore di turno
        private void SincronizzaGrigliaVisiva()
        {
            GrigliaGioco campoCorrente = (turnoGiocatore == 1) ? campoP2 : campoP1;

            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 10; c++)
                {
                    StatoCella stato = campoCorrente.matriceGioco[r, c].stato;
                    if (stato == StatoCella.Colpita)
                    {
                        matricedBottoni[r, c].Background = Brushes.Crimson;
                        matricedBottoni[r, c].IsEnabled = false;
                    }
                    else if (stato == StatoCella.Acqua)
                    {
                        matricedBottoni[r, c].Background = Brushes.DodgerBlue;
                        matricedBottoni[r, c].IsEnabled = false;
                    }
                    else
                    {
                        // Le navi non colpite rimangono nascoste (colore standard)
                        matricedBottoni[r, c].Background = Brushes.LightSteelBlue;
                        matricedBottoni[r, c].IsEnabled = true;
                    }
                }
            }
        }

        private void DisabilitaGriglia()
        {
            for (int r = 0; r < 10; r++)
                for (int c = 0; c < 10; c++)
                    matricedBottoni[r, c].IsEnabled = false;
        }
    }
}