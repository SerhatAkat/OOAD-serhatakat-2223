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
    /// Interaction logic for MotorInfo.xaml
    /// </summary>
    public partial class MotorInfo : Page
    {
        private Voertuig huidigeVoertuig;
        private int gebruikerId;

        public MotorInfo(Voertuig voertuig, int gebruikerID)
        {
            huidigeVoertuig = voertuig;
            gebruikerId = gebruikerID;

            InitializeComponent();
            LoadFotosForVoertuig(voertuig.Id);

            lblMotorNaam.Content = voertuig.Naam;
            lblMotorBeschrijving.Content = "Beschrijving: " + voertuig.Beschrijving;
            lblMotorMerk.Content = "Merk: " + (string.IsNullOrEmpty(voertuig.Merk) ? "n.v.t." : voertuig.Merk);
            lblMotorBouwjaar.Content = "Bouwjaar: " + voertuig.Bouwjaar;
            lblMotorModel.Content = "Model: " + (string.IsNullOrEmpty(voertuig.Model) ? "n.v.t." : voertuig.Model);
            lblMotorTransmissie.Content = "Transmissie: " + (voertuig.TransmissieType == null ? "n.v.t." : voertuig.TransmissieType.ToString());
            lblMotorBrandstof.Content = "Brandstof: " + (voertuig.BrandstofType == null ? "n.v.t." : voertuig.BrandstofType.ToString());
            string eigenaarNaam = Gebruiker.GetGebruikerNaamById(voertuig.Eigenaar);
            lblMotorEigenaar.Content = "Eigenaar: " + (eigenaarNaam ?? "Onbekend");
        }
        private void LoadFotosForVoertuig(int voertuigId)
        {
            List<Foto> fotos = Foto.GetFotosForVoertuig(voertuigId);

            foreach (Foto foto in fotos)
            {
                Image img = new Image();
                img.Source = ConvertByteArrayToBitmapImage(foto.Image);
                img.Width = 150;
                img.Margin = new Thickness(0, 0, 10, 0);
                stkMotorFotos.Children.Add(img);
            }
        }

        private BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }

        private void btnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            if (dtmVan.SelectedDate.HasValue && dtmTot.SelectedDate.HasValue)
            {
                if (dtmVan.SelectedDate.Value >= dtmVan.SelectedDate.Value)
                {
                    Ontlening nieuweOntlening = new Ontlening
                    {
                        Id = huidigeVoertuig.Id,
                        Vanaf = dtmVan.SelectedDate.Value.Date,
                        Tot = dtmTot.SelectedDate.Value.Date,
                        Bericht = txtBericht.Text,
                        OntleningStatus = Ontlening.Status.InAanvraag,
                        Aanvrager = Gebruiker.GetGebruikerById(gebruikerId)
                    };

                    Ontlening.VoegOntleningToe(nieuweOntlening);

                    dtmVan.SelectedDate = null;
                    dtmTot.SelectedDate = null;
                    txtBericht.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("De einddatum moet na de begindatum zijn.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vul de begindatum en einddatum in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
