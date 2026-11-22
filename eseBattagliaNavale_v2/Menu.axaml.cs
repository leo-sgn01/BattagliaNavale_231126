using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace eseBattagliaNavale_v2;

public partial class Main : Window
{
    public Main()
    {
        InitializeComponent();
    }

    private void BtnInizia_Click(object? sender, RoutedEventArgs e)
    {
        Inserimento finestraInsert = new Inserimento();
        finestraInsert.Show();
        this.Close();
    }
}