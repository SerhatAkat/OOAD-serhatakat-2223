using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for EditGetrokken.xaml
    /// </summary>
    public partial class EditGetrokken : Window
    {
        private Gebruiker currentId;
        private Voertuig teBewerkenVoertuig;

        public EditGetrokken(Gebruiker userId)
        {
            InitializeComponent();
            currentId = userId;
            btnUploaden.Click += BtnUploaden_Click;
        }
        public EditGetrokken(Voertuig voertuig)
        {
            InitializeComponent();
            teBewerkenVoertuig = voertuig;

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
            List<Foto> fotos;
            try
            {
                fotos = Foto.GetFotosForVoertuig(voertuig.Id);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("Er is een fout opgetreden bij het ophalen van de foto's uit de database: " + ex.Message, "Databasefout", MessageBoxButton.OK, MessageBoxImage.Error);
                fotos = new List<Foto>(); // Maak een lege lijst in geval van fout
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een algemene fout opgetreden: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                fotos = new List<Foto>(); // Maak een lege lijst in geval van fout
            }

            List<BitmapImage> images2 = new List<BitmapImage>();

            foreach (Foto foto in fotos)
            {
                images2.Add(ConvertToBitmapImage(foto.Image));
            }

            if (images2.Count >= 1)
            {
                img1.Source = images2[0];
            }
            if (images2.Count >= 2)
            {
                img1.Source = images2[0];

                img2.Source = images2[1];
            }
            if (images2.Count >= 3)
            {
                img1.Source = images2[0];

                img2.Source = images2[1];

                img3.Source = images2[2];
            }

            if (fotos.Count >= 1)
            {
                Foto afb1 = fotos[0];
                btnVerwijder1.Tag = afb1;
            }
            if (fotos.Count >= 2)
            {
                Foto afb2 = fotos[1];
                btnVerwijder2.Tag = afb2;
            }
            if (fotos.Count >= 3)
            {
                Foto afb3 = fotos[2];
                btnVerwijder3.Tag = afb3;
            }

            currentId = new Gebruiker { Id = voertuig.Id };
        }
        private BitmapImage ConvertToBitmapImage(byte[] imageBytes)
        {
            if (imageBytes != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    memoryStream.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                }
                bitmapImage.Freeze(); // Freeze the BitmapImage to improve performance and allow cross-thread access if needed
                return bitmapImage;
            }
            else
            {
                return null;
            }
        }

        private void BtnUploaden_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Toegang geweigerd: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Bestandsleesfout: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void VerwijderAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Foto foto = button.Tag as Foto;

            switch (button.Name)
            {
                case "btnVerwijder1":
                    img1.Source = null;
                    if (foto != null);
                    break;
                case "btnVerwijder2":
                    img2.Source = null;
                    if (foto != null);
                    break;
                case "btnVerwijder3":
                    img3.Source = null;
                    if (foto != null);
                    break;
            }
        }

        private byte[] ImageToByte(Image img)
        {
            if (img.Source == null)
            {
                return null;
            }
            else
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)img.Source));
                byte[] imageData;
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    imageData = ms.ToArray();
                }
                return imageData;
            }
        }
        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            Voertuig huidigVoertuig = teBewerkenVoertuig;
            List<Foto> bestaandeFotos;
            try
            {
                bestaandeFotos = Foto.GetFotosForVoertuig(teBewerkenVoertuig.Id);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("Er is een fout opgetreden bij het ophalen van de bestaande foto's uit de database: " + ex.Message, "Databasefout", MessageBoxButton.OK, MessageBoxImage.Error);
                bestaandeFotos = new List<Foto>(); // Lege lijst in geval van fout
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een algemene fout opgetreden: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                bestaandeFotos = new List<Foto>(); // Lege lijst in geval van fout
            }

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
                huidigVoertuig.Id = currentId.Id;
                huidigVoertuig.Naam = txtNaam.Text;
                huidigVoertuig.Merk = txtMerk.Text;
                huidigVoertuig.Model = txtModel.Text;
                huidigVoertuig.Beschrijving = txtBeschrijving.Text;
                huidigVoertuig.Afmetingen = txtAfmetingen.Text;
                huidigVoertuig.Geremd = rbnJa.IsChecked == true;

                if (!string.IsNullOrEmpty(txtGewicht.Text))
                    huidigVoertuig.Gewicht = Convert.ToInt32(txtGewicht.Text);
                if (!string.IsNullOrEmpty(txtMax.Text))
                    huidigVoertuig.MaxBelasting = Convert.ToInt32(txtMax.Text);
                if (!int.TryParse(txtBouwjaar.Text, out int bouwjaar))
                {
                    MessageBox.Show("Gelieve een geldig bouwjaar in te vullen.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                huidigVoertuig.Bouwjaar = bouwjaar;

                // Update het voertuig in de database
                huidigVoertuig.UpdateGetrokken(teBewerkenVoertuig.Id);

                if (img1.Source != null)
                {
                    byte[] foto1 = ImageToByte(img1);

                    if (btnVerwijder1.Tag != null)
                    {
                        Foto.EditFoto(((Foto)btnVerwijder1.Tag).Id, foto1);
                    }
                    else
                    {
                        Foto.AddFoto(foto1, teBewerkenVoertuig.Id);
                    }
                }
                else
                {
                    if (((Foto)btnVerwijder1.Tag) != null)
                    {
                        Foto.VerwijderFotoByFotoId(((Foto)btnVerwijder1.Tag).Id);
                    }
                }

                if (img2.Source != null)
                {
                    byte[] foto2 = ImageToByte(img2);

                    if (btnVerwijder2.Tag != null)
                    {
                        Foto.EditFoto(((Foto)btnVerwijder2.Tag).Id, foto2);
                    }
                    else
                    {
                        Foto.AddFoto(foto2, teBewerkenVoertuig.Id);
                    }
                }
                else
                {
                    if (((Foto)btnVerwijder2.Tag) != null)
                    {
                        Foto.VerwijderFotoByFotoId(((Foto)btnVerwijder2.Tag).Id);
                    }
                }

                if (img3.Source != null)
                {
                    byte[] foto3 = ImageToByte(img3);

                    if (btnVerwijder3.Tag != null)
                    {
                        Foto.EditFoto(((Foto)btnVerwijder3.Tag).Id, foto3);
                    }
                    else
                    {
                        Foto.AddFoto(foto3, teBewerkenVoertuig.Id);
                    }
                }
                else
                {
                    if (((Foto)btnVerwijder3.Tag) != null)
                    {
                        Foto.VerwijderFotoByFotoId(((Foto)btnVerwijder3.Tag).Id);
                    }
                }

                VoertuigenPage.instance.UpdateVoertuigen();
                Close();
            }
        }
    }
}
