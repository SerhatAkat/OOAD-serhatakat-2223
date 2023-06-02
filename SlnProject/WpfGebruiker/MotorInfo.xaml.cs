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
    /// Interaction logic for MotorInfo.xaml
    /// </summary>
    public partial class MotorInfo : Page
    {
        private Voertuig huidigeVoertuig;
        private int gebruikerId;

        public MotorInfo(Voertuig voertuig, int gebruikerID, bool isReadOnly = false)
        {
            huidigeVoertuig = voertuig;
            gebruikerId = gebruikerID;

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

        private void BtnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime currentDate = DateTime.Today;
                if (dtmVan.SelectedDate.HasValue && dtmTot.SelectedDate.HasValue)
                {
                    if (dtmVan.SelectedDate.Value.Date < dtmTot.SelectedDate.Value.Date && dtmVan.SelectedDate.Value.Date >= currentDate)
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
                        if (dtmVan.SelectedDate.Value.Date < currentDate)
                        {
                            MessageBox.Show("Gelieve een toekomstige datum te kiezen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("De einddatum moet na de begindatum zijn.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vul de begindatum en einddatum in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
