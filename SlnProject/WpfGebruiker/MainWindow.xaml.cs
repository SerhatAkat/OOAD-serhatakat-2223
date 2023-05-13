using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gebruiker ingelogdeGebruiker;

        public MainWindow(Gebruiker gebruiker)
        {
            InitializeComponent();
            ingelogdeGebruiker = gebruiker;

            // Haal de lijst met voertuigen op
            List<Voertuig> voertuigen = Voertuig.GetAllVoertuigen();
            foreach (var voertuig in voertuigen)
            {
                // Haal de bijbehorende foto op
                Foto foto = Foto.GetFotoForVoertuig(voertuig.Id);

                // Aangenomen dat foto.Image een byte array is
                BitmapImage bitmap = new BitmapImage();
                using (var mem = new MemoryStream(foto.Image))
                {
                    mem.Position = 0;
                    bitmap.BeginInit();
                    bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = null;
                    bitmap.StreamSource = mem;
                    bitmap.EndInit();
                }
                bitmap.Freeze();

                Image img = new Image();
                img.Width = 80;
                img.Source = bitmap;

                Label lbl = new Label();
                lbl.Content = $"{voertuig.Merk} {voertuig.Model}";

                Button btn = new Button();
                btn.Tag = voertuig.Id;
                btn.Content = "Info";
                btn.Click += VoertuigChosen;

                // Maak een StackPanel om de afbeelding, het label en de knop te groeperen
                StackPanel pnl = new StackPanel();
                pnl.Margin = new Thickness(0, 0, 20, 20);
                pnl.Children.Add(img);
                pnl.Children.Add(lbl);
                pnl.Children.Add(btn);

                // Voeg de StackPanel toe aan het hoofdpaneel
                pnlItems.Children.Add(pnl);
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        private void btnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OntleningenPage());
        }

        private void btnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new VoertuigenPage());
        }
        private void VoertuigChosen(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            MessageBox.Show($"Voertuig {clicked.Tag} geselecteerd");
        }

    }
}
