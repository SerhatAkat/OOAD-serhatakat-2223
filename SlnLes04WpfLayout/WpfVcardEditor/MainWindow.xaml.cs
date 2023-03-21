﻿using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();

        bool checkWijziging = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow About = new AboutWindow();
            About.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultaat = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten?", "Afsluiten", MessageBoxButton.OKCancel);
            if (resultaat == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog.Filter = "vCard files (*.vcf)|*.vcf|All files (*.*)|*.*";
            string workEmailPrefix = "EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:";
            string homeEmailPrefix = "EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:";
            string workPhonePrefix = "TEL;TYPE=WORK,VOICE:";
            string homePhonePrefix = "TEL;TYPE=HOME,VOICE:";
            string companyPrefix = "ORG;CHARSET=UTF-8:";
            string titlePrefix = "TITLE;CHARSET=UTF-8:";
            string facebookPrefix = "X-SOCIALPROFILE;TYPE=facebook:";
            string linkedinPrefix = "X-SOCIALPROFILE;TYPE=linkedin:";
            string instagramPrefix = "X-SOCIALPROFILE;TYPE=instagram:";
            string youtubePrefix = "X-SOCIALPROFILE;TYPE=youtube:";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    lblKaart.Content = Path.GetFullPath(openFileDialog.FileName);
                    string fileName = openFileDialog.FileName;
                    string[] lines = File.ReadAllLines(fileName);

                    foreach (string line in lines)
                    {
                        // naam
                        if (line.StartsWith("N;"))
                        {
                            string[] parts = line.Split(';', ':');
                            string lastName = parts[2];
                            string firstName = parts[3];
                            txtVoornaam.Text = firstName;
                            txtAchternaam.Text = lastName;
                        }

                        // emails
                        else if (line.StartsWith(workEmailPrefix))
                        {
                            string workEmail = line.Substring(39);
                            txtWerkemail.Text = workEmail;
                        }
                        else if (line.StartsWith(homeEmailPrefix))
                        {
                            string homeEmail = line.Substring(39);
                            txtEmail.Text = homeEmail;
                        }

                        // telefoon
                        else if (line.StartsWith(homePhonePrefix))
                        {
                            string homePhone = line.Substring(20);
                            txtTelefoon.Text = homePhone;
                        }
                        else if (line.StartsWith(workPhonePrefix))
                        {
                            string workPhone = line.Substring(20);
                            txtWerktelefoon.Text = workPhone;
                        }

                        // bedrijf
                        else if (line.StartsWith(companyPrefix))
                        {
                            string company = line.Substring(18);
                            txtBedrijf.Text = company;
                        }

                        // titel
                        else if (line.StartsWith(titlePrefix))
                        {
                            string title = line.Substring(20);
                            txtJobtitel.Text = title;
                        }

                        // sociale media
                        else if (line.StartsWith(facebookPrefix))
                        {
                            string facebook = line.Substring(30);
                            txtFacebook.Text = facebook;
                        }
                        else if (line.StartsWith(instagramPrefix))
                        {
                            string instagram = line.Substring(31);
                            txtInstagram.Text = instagram;
                        }
                        else if (line.StartsWith(linkedinPrefix))
                        {
                            string linkedin = line.Substring(30);
                            txtLindkedin.Text = linkedin;
                        }
                        else if (line.StartsWith(youtubePrefix))
                        {
                            string youtube = line.Substring(29);
                            txtYoutube.Text = youtube;
                        }

                        // geslacht
                        else if (line.Contains("GENDER:M"))
                        {
                            rbnMan.IsChecked = true;
                        }
                        else if (line.Contains("GENDER:F"))
                        {
                            rbnVrouw.IsChecked = true;
                        }
                        else if (line.Contains("GENDER:O"))
                        {
                            rbnOnbekend.IsChecked = true;
                        }

                        // geboortedatum
                        else if (line.StartsWith("BDAY:"))
                        {
                            string dateField = line.Substring(5);
                            DateTime birthDate = DateTime.ParseExact(dateField, "yyyyMMdd", CultureInfo.InvariantCulture);
                            datGeboortedatum.Text = birthDate.ToString("dd/MM/yyyy");
                        }

                        // afbeelding
                        else if (line.StartsWith("PHOTO;"))
                        {
                            string photoLine = line.Replace("PHOTO;ENCODING=b;TYPE=JPEG:", "");
                            byte[] photoBytes = Convert.FromBase64String(photoLine);
                            BitmapImage image = new BitmapImage();
                            using (MemoryStream ms = new MemoryStream(photoBytes))
                            {
                                ms.Position = 0;
                                image.BeginInit();
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.StreamSource = ms;
                                image.EndInit();
                            }
                            imgFoto.Source = image;
                            ShowImageName(image);
                        }
                    }
                    btnSave.IsEnabled = true;
                    btnSaveAs.IsEnabled = true;
                    checkWijziging = false;
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        $"{ex.FileName} niet gevonden", // boodschap
                        "Oeps!", // titel
                        MessageBoxButton.OK, // buttons
                        MessageBoxImage.Error);
                }
                btnSave.IsEnabled = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile(openFileDialog.FileName);
            MessageBox.Show("Bestand is opgeslagen");
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "vCard files (*.vcf)|*.vcf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    SaveToFile(saveFileDialog.FileName);
                    MessageBox.Show("Bestand is opgeslagen");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ERROR: Kan bestand niet overschrijven!{ex.Message}", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SaveToFile(string fileName)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine($"BEGIN:VCARD");
                    sw.WriteLine($"VERSION:3.0");
                    if (txtVoornaam.Text != "" && txtAchternaam.Text != "")
                    {
                        sw.WriteLine($"FN;CHARSET=UTF-8:{txtVoornaam.Text} {txtAchternaam.Text}");
                        sw.WriteLine($"N;CHARSET=UTF-8:{txtAchternaam.Text};{txtVoornaam.Text};;;");
                        sw.WriteLine($"NICKNAME;CHARSET=UTF-8:{txtVoornaam.Text}");
                    }
                    if (rbnMan.IsChecked == true)
                    {
                        sw.WriteLine("GENDER:M");
                    }
                    else if (rbnVrouw.IsChecked == true)
                    {
                        sw.WriteLine("GENDER:F");
                    }
                    else if (rbnOnbekend.IsChecked == true)
                    {
                        sw.WriteLine("GENDER:O");
                    }
                    DateTime birthDate;
                    if (DateTime.TryParseExact(datGeboortedatum.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                    {
                        sw.WriteLine($"BDAY:{birthDate.ToString("yyyyMMdd")}");
                    }
                    if (txtEmail.Text != "")
                    {
                        sw.WriteLine($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{txtEmail.Text}");
                    }
                    if (txtWerkemail.Text != "")
                    {
                        sw.WriteLine($"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{txtWerkemail.Text}");
                    }
                    if (txtTelefoon.Text != "")
                    {
                        sw.WriteLine($"TEL;TYPE=HOME,VOICE:{txtTelefoon.Text}");
                    }
                    if (txtWerktelefoon.Text != "")
                    {
                        sw.WriteLine($"TEL;TYPE=WORK,VOICE:{txtWerktelefoon.Text}");
                    }
                    if (txtJobtitel.Text != "")
                    {
                        sw.WriteLine($"TITLE;CHARSET=UTF-8:{txtJobtitel.Text}");
                    }
                    if (txtBedrijf.Text != "")
                    {
                        sw.WriteLine($"ORG;CHARSET=UTF-8:{txtBedrijf.Text}");
                    }
                    if (txtFacebook.Text != "")
                    {
                        sw.WriteLine($"X-SOCIALPROFILE;TYPE=facebook:{txtFacebook.Text}");
                    }
                    if (txtLindkedin.Text != "")
                    {
                        sw.WriteLine($"X-SOCIALPROFILE;TYPE=linkedin:{txtLindkedin.Text}");
                    }
                    if (txtInstagram.Text != "")
                    {
                        sw.WriteLine($"X-SOCIALPROFILE;TYPE=instagram:{txtInstagram.Text}");
                    }
                    if (txtYoutube.Text != "")
                    {
                        sw.WriteLine($"X-SOCIALPROFILE;TYPE=youtube:{txtYoutube.Text}");
                    }
                    if (imgFoto.Source != null)
                    {
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imgFoto.Source));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            encoder.Save(ms);
                            var imageData = ms.ToArray();
                            sw.WriteLine($"PHOTO;ENCODING=b;TYPE=JPEG:{ Convert.ToBase64String(imageData)}");
                        }
                    }
                    sw.WriteLine("END:VCARD");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR: Kan bestand niet overschrijven!{ex.Message}", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (checkWijziging == true)
            {
                if (MessageBox.Show("Er zijn onopgeslagen wijzigingen. Wilt u doorgaan en deze wijzigingen negeren?", "Waarschuwing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
            }
            ClearAll();

            btnSave.IsEnabled = false;
            btnSaveAs.IsEnabled = false;
            checkWijziging = false;
        }

        private void ClearAll()
        {
            txtVoornaam.Text = "";
            txtAchternaam.Text = "";
            datGeboortedatum.SelectedDate = null;
            rbnMan.IsChecked = false;
            rbnVrouw.IsChecked = false;
            rbnOnbekend.IsChecked = false;
            txtEmail.Text = "";
            txtTelefoon.Text = "";
            txtWerktelefoon.Text = "";
            txtBedrijf.Text = "";
            txtJobtitel.Text = "";
            txtWerkemail.Text = "";
            txtFacebook.Text = "";
            txtInstagram.Text = "";
            txtLindkedin.Text = "";
            txtYoutube.Text = "";
            lblFoto.Content = "";
            imgFoto.Source = null;
        }

        private void Card_Changed(object sender, EventArgs e)
        {
            checkWijziging = true;
            btnSave.IsEnabled = true;
        }

        private void btnSelecteer_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Afbeeldingen|*.jpg;*.jpeg;*.png;*.gif";
            if (dlg.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new System.Uri(dlg.FileName));
                imgFoto.Source = bitmap;
                lblFoto.Content = dlg.FileName;
            }
        }
        private void ShowImageName(BitmapImage image)
        {
            if (image != null && image.UriSource != null)
            {
                string imageName = Path.GetFileName(image.UriSource.LocalPath);
                lblFoto.Content = imageName;
            }
        }
    }
}
