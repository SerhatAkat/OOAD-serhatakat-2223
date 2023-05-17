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
using MyClassLibrary;

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
                btn.Click += VoertuigInfoButton_Click;

                // Maak een StackPanel om de afbeelding en de labels te groeperen
                StackPanel pnl = new StackPanel();
                pnl.Orientation = Orientation.Vertical;
                pnl.Children.Add(lblNaam);
                pnl.Children.Add(lblMerk);
                pnl.Children.Add(lblModel);
                pnl.Children.Add(btn);

                // Maak een Grid om de afbeelding en het StackPanel te groeperen
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                img.SetValue(Grid.ColumnProperty, 0);
                pnl.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(img);
                grid.Children.Add(pnl);

                // Voeg een Border toe
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1);
                border.Margin = new Thickness(5);
                border.HorizontalAlignment = HorizontalAlignment.Center;
                border.VerticalAlignment = VerticalAlignment.Center;
                border.Child = grid;

                // Voeg de Border toe aan het hoofdpaneel
                pnlItems.Children.Add(border);
            }
        }

        private void VoertuigInfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Haal de Voertuig's ID uit de Tag van de knop
            Button knop = sender as Button;
            int voertuigId = int.Parse(knop.Tag.ToString());

            // Zoek het Voertuig in je lijst
            List<Voertuig> alleVoertuigen = Voertuig.GetAllVoertuigen();
            Voertuig gevondenVoertuig = null;
            foreach (Voertuig voertuig in alleVoertuigen)
            {
                if (voertuig.Id == voertuigId)
                {
                    gevondenVoertuig = voertuig;
                    break;
                }
            }

            if (gevondenVoertuig != null)
            {
                // Afhankelijk van het type van het Voertuig, navigeer naar de juiste pagina
                if (gevondenVoertuig.Type == 1)
                {
                    // Vervang MotorInfoPage met de daadwerkelijke naam van je pagina
                    MotorInfo pagina = new MotorInfo(gevondenVoertuig);
                    this.NavigationService.Navigate(pagina);
                }
                else if (gevondenVoertuig.Type == 2)
                {
                    // Vervang GetrokkenPage met de daadwerkelijke naam van je pagina
                    GetrokkenInfo pagina = new GetrokkenInfo(gevondenVoertuig);
                    this.NavigationService.Navigate(pagina);
                }
            }
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
