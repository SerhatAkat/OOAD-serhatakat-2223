using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

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

                Label lblNaam = new Label();
                lblNaam.Content = voertuig.Naam;
                lblNaam.FontWeight = FontWeights.Bold;

                Label lblMerk = new Label();
                lblMerk.Content = voertuig.Merk;

                Label lblModel = new Label();
                lblModel.Content = voertuig.Model;

                Button btn = new Button();
                btn.Tag = voertuig.Id;
                btn.Content = "Info";
                btn.Click += VoertuigChosen;

                // Maak een StackPanel om de labels en de knop te groeperen
                StackPanel pnlLabels = new StackPanel();
                pnlLabels.Children.Add(lblNaam);
                pnlLabels.Children.Add(lblMerk);
                pnlLabels.Children.Add(lblModel);
                pnlLabels.Children.Add(btn);

                // Maak een WrapPanel om de afbeelding en de StackPanel te groeperen
                WrapPanel pnl = new WrapPanel();
                pnl.Margin = new Thickness(0, 0, 20, 20);
                pnl.Children.Add(img);
                pnl.Children.Add(pnlLabels);

                // Voeg de WrapPanel toe aan het hoofdpaneel
                pnlItems.Children.Add(pnl);
            }
        }
        private void VoertuigChosen(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            MessageBox.Show($"Voertuig {clicked.Tag} geselecteerd");
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
