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
    /// Interaction logic for GetrokkenInfo.xaml
    /// </summary>
    public partial class GetrokkenInfo : Page
    {
        private int gebruikerId;
        private string voertuigNaam;
        private Voertuig huidigeVoertuig;

        public GetrokkenInfo(Voertuig voertuig, int gebruikerID, bool isReadOnly = false)
        {
            InitializeComponent();
            if (isReadOnly)
            {
                dtmTot.Visibility = Visibility.Collapsed;
                dtmVan.Visibility = Visibility.Collapsed;
                btnBevestigen.Visibility = Visibility.Collapsed;
                txtBericht.Visibility = Visibility.Collapsed;
                lblTot.Visibility = Visibility.Collapsed;
                lblVan.Visibility = Visibility.Collapsed;
                lblBericht.Visibility = Visibility.Collapsed;
            }
            huidigeVoertuig = voertuig;
            gebruikerId = gebruikerID;

            LoadFotosForVoertuig(voertuig.Id);

            voertuigNaam = voertuig.Naam;
            lblGetrokkenNaam.Content = voertuig.Naam;
            lblGetrokkenBeschrijving.Content = "Beschrijving: " + voertuig.Beschrijving;
            lblGetrokkenMerk.Content = "Merk: " + (string.IsNullOrEmpty(voertuig.Merk) ? "n.v.t." : voertuig.Merk);

            if (voertuig.Geremd.HasValue)
            {
                lblGetrokkenGeremd.Content = "Geremd: " + (voertuig.Geremd.Value ? "Ja" : "Nee");
            }
            else
            {
                lblGetrokkenGeremd.Content = "Geremd: Geen informatie beschikbaar";
            }
            lblGetrokkenModel.Content = "Model: " + (string.IsNullOrEmpty(voertuig.Model) ? "n.v.t." : voertuig.Model);
            lblGetrokkenAfmetingen.Content = "Afmetingen: " + (string.IsNullOrEmpty(voertuig.Afmetingen) ? "n.v.t." : voertuig.Afmetingen);
            lblGetrokkenGewicht.Content = "Gewicht: " + (voertuig.Gewicht.HasValue ? voertuig.Gewicht.Value.ToString() : "n.v.t.") + " kg";
            lblGetrokkenBouwjaar.Content = "Bouwjaar: " + voertuig.Bouwjaar;
            lblGetrokkenBelasting.Content = "Max. belasting: " + (voertuig.MaxBelasting.HasValue ? voertuig.MaxBelasting.Value.ToString() : "n.v.t.") + " kg";
            string eigenaarNaam = Gebruiker.GetGebruikerNaamById(voertuig.Eigenaar);
            lblGetrokkenEigenaar.Content = "Eigenaar: " + (eigenaarNaam ?? "Onbekend");
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
                stkGetrokkenFotos.Children.Add(img);
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
