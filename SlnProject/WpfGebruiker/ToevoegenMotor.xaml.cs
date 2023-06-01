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
    /// Interaction logic for ToevoegenMotor.xaml
    /// </summary>
    public partial class ToevoegenMotor : Window
    {
        private Gebruiker currentId;

        public ToevoegenMotor(Gebruiker userId)
        {
            InitializeComponent();
            btnUploaden.Click += BtnUploaden_Click;
            currentId = userId;
        }

        public ToevoegenMotor(Voertuig voertuig)
        {
            InitializeComponent();
            btnUploaden.Click += BtnUploaden_Click;

            txtNaam.Text = voertuig.Naam;
            txtMerk.Text = voertuig.Merk;
            txtModel.Text = voertuig.Model;
            txtBeschrijving.Text = voertuig.Beschrijving;
            if (voertuig.BrandstofType.HasValue)
                cbxBrandstof.SelectedIndex = (int)voertuig.BrandstofType;
            else
                cbxBrandstof.SelectedIndex = -1;

            if (voertuig.TransmissieType.HasValue)
                cbxTransmissie.SelectedIndex = (int)voertuig.TransmissieType;
            else
                cbxTransmissie.SelectedIndex = -1;

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
            int loadedImagesCount = 0;

            if (img1.Source != null) loadedImagesCount++;
            if (img2.Source != null) loadedImagesCount++;
            if (img3.Source != null) loadedImagesCount++;

            // Als er al 1, 2 of 3 afbeeldingen zijn, hoef je het dialoogvenster niet te openen
            if (loadedImagesCount > 0)
            {
                return;
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

                if (images.Count > 0)
                {
                    img1.Source = images[0];
                }
                if (images.Count > 1)
                {
                    img2.Source = images[1];
                }
                if (images.Count > 2)
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
                lblImageError.Content = "Gelieve ten minste 1 afbeelding te kiezen";
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
                };
                if (nieuwVoertuig.BrandstofType.HasValue)
                    cbxBrandstof.SelectedIndex = (int)nieuwVoertuig.BrandstofType;
                else
                    cbxBrandstof.SelectedIndex = -1; // Of een andere standaardwaarde

                if (nieuwVoertuig.TransmissieType.HasValue)
                    cbxTransmissie.SelectedIndex = (int)nieuwVoertuig.TransmissieType;
                else
                    cbxTransmissie.SelectedIndex = -1; // Of een andere standaardwaarde

                if (!int.TryParse(txtBouwjaar.Text, out int bouwjaar))
                {
                    MessageBox.Show("Vul een geldig bouwjaar in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                nieuwVoertuig.Bouwjaar = bouwjaar;
                int voertuigId = nieuwVoertuig.ToevoegenGemotoriseerdVoertuig(nieuwVoertuig, currentId.Id);

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
                VoertuigenPage.Instance.UpdateVoertuigen();
                Close();
            }
        }

        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
