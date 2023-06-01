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
using System.Windows.Shapes;
using Microsoft.Win32;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for ToevoegenGetrokken.xaml
    /// </summary>
    public partial class ToevoegenGetrokken : Window
    {
        private Gebruiker currentId;

        public ToevoegenGetrokken(Gebruiker userId)
        {
            InitializeComponent();
            btnUploaden.Click += BtnUploaden_Click;
            currentId = userId;
        }
        public ToevoegenGetrokken(Voertuig voertuig)
        {
            InitializeComponent();
            btnUploaden.Click += BtnUploaden_Click;

            txtNaam.Text = voertuig.Naam;
            txtMerk.Text = voertuig.Merk;
            txtModel.Text = voertuig.Model;
            txtBeschrijving.Text = voertuig.Beschrijving;
            txtAfmetingen.Text = voertuig.Afmetingen;
            rbnJa.IsChecked = voertuig.Geremd;
            rbnNee.IsChecked = !voertuig.Geremd;
            txtGewicht.Text = voertuig.Gewicht?.ToString();
            txtMax.Text = voertuig.MaxBelasting?.ToString();
            txtBouwjaar.Text = voertuig.Bouwjaar.ToString();

            // Haal de afbeeldingen op voor dit voertuig
            List<Foto> fotos = Foto.GetFotosForVoertuig(voertuig.Id);

            // Zet de byte-arrays om in afbeeldingen en stel de bron van de afbeeldingscontrols in
            for (int i = 0; i < fotos.Count; i++)
            {
                byte[] afbeeldingData = fotos[i].Image;
                BitmapImage bitmap = new BitmapImage();

                using (var stream = new MemoryStream(afbeeldingData))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }

                switch (i)
                {
                    case 0:
                        img1.Source = bitmap;
                        break;
                    case 1:
                        img2.Source = bitmap;
                        break;
                    case 2:
                        img3.Source = bitmap;
                        break;
                }
            }

            currentId = new Gebruiker { Id = voertuig.Id };
        }

        private void BtnUploaden_Click(object sender, RoutedEventArgs e)
        {
            // Controleer of minstens één afbeelding al een bron heeft
            if (img1.Source != null || img2.Source != null || img3.Source != null)
            {
                return; // Als dat zo is, doe niets
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                List<BitmapImage> images = new List<BitmapImage>();
                foreach (string filename in openFileDialog.FileNames)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(filename);
                    bitmap.EndInit();
                    images.Add(bitmap);
                }

                // Alleen de afbeeldingsbron instellen als deze nog niet is ingesteld
                if (images.Count > 0 && img1.Source == null)
                {
                    img1.Source = images[0];
                }
                if (images.Count > 1 && img2.Source == null)
                {
                    img2.Source = images[1];
                }
                if (images.Count > 2 && img3.Source == null)
                {
                    img3.Source = images[2];
                }
            }
        }


        private void VerwijderAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Name)
            {
                case "btnVerwijder1":
                    if (img1.Source != null) img1.Source = null;
                    break;
                case "btnVerwijder2":
                    if (img2.Source != null) img2.Source = null;
                    break;
                case "btnVerwijder3":
                    if (img3.Source != null) img3.Source = null;
                    break;
            }
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            // Reset error labels
            lblNaamError.Content = "";
            lblBeschrijvingError.Content = "";
            lblBouwjaarError.Content = "";

            bool isValid = true;

            // Validatie
            if (string.IsNullOrEmpty(txtNaam.Text))
            {
                lblNaamError.Content = "Gelieve een naam te geven.";
                isValid = false;
            }
            if (string.IsNullOrEmpty(txtBeschrijving.Text))
            {
                lblBeschrijvingError.Content = "Gelieve een beschrijving te geven.";
                isValid = false;
            }
            if (string.IsNullOrEmpty(txtBouwjaar.Text))
            {
                lblBouwjaarError.Content = "Gelieve een bouwjaar in te vullen.";
                isValid = false;
            }

            if (img1.Source == null && img2.Source == null && img3.Source == null)
            {
                lblImageError.Content = "Kies ten minste 1 afbeelding";
                isValid = false;
            }

            // Voer de rest van de methode alleen uit als alle velden zijn gevalideerd
            if (isValid)
            {
                Voertuig nieuwVoertuig = new Voertuig
                {
                    Naam = txtNaam.Text,
                    Merk = txtMerk.Text,
                    Model = txtModel.Text,
                    Beschrijving = txtBeschrijving.Text,
                    Afmetingen = txtAfmetingen.Text,
                    Geremd = rbnJa.IsChecked == true,
                };

                if (!string.IsNullOrEmpty(txtGewicht.Text)) nieuwVoertuig.Gewicht = (int?)Convert.ToInt32(txtGewicht.Text);
                if (!string.IsNullOrEmpty(txtMax.Text)) nieuwVoertuig.MaxBelasting = (int?)Convert.ToInt32(txtMax.Text);
                if (!int.TryParse(txtBouwjaar.Text, out int bouwjaar))
                {
                    MessageBox.Show("Gelieve een geldig bouwjaar in te vullen.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int voertuigId = nieuwVoertuig.ToevoegenGetrokkenVoertuig(nieuwVoertuig, currentId.Id);

                // Controleer of voertuig succesvol is toegevoegd
                if (voertuigId > 0)
                {
                    Foto foto = new Foto();

                    // Converteer elke afbeelding naar een byte array en voeg ze toe aan de database
                    foreach (Image img in new[] { img1, img2, img3 })
                    {
                        if (img.Source != null)
                        {
                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)img.Source));

                            using (var stream = new MemoryStream())
                            {
                                encoder.Save(stream);
                                byte[] imgData = stream.ToArray();
                                Foto.AddFoto(imgData, voertuigId);
                            }
                        }
                    }
                }
                VoertuigenPage.instance.UpdateVoertuigen();
                Close();
            }
        }

        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
