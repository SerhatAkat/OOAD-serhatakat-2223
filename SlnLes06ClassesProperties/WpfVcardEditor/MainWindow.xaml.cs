using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;
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
            if (openFileDialog.ShowDialog() == true)
            {
                Vcard card = ReadVcardFromFile(openFileDialog.FileName);
                ShowVcard(card);
            }
        }

        private Vcard ToVcard()
        {
            Vcard card = new Vcard();
            card.FirstName = txtVoornaam.Text;
            card.LastName = txtAchternaam.Text;
            card.HomeEmail = txtEmail.Text;
            card.WorkEmail = txtWerkemail.Text;
            card.HomePhone = txtTelefoon.Text;
            card.WorkPhone = txtWerktelefoon.Text;
            card.JobTitel = txtJobtitel.Text;
            card.Company = txtBedrijf.Text;
            card.Facebook = txtFacebook.Text;
            card.Linkedin = txtFacebook.Text;
            card.Instagram = txtFacebook.Text;
            card.Youtube = txtFacebook.Text;
            card.BirthDate = datGeboortedatum.SelectedDate.Value;

            BitmapImage bitmap = (BitmapImage)imgFoto.Source;
            MemoryStream memoryStream = new MemoryStream();
            BitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(memoryStream);
            byte[] bytes = memoryStream.ToArray();
            string base64String = Convert.ToBase64String(bytes);
            card.Img = (BitmapSource)new ImageSourceConverter().ConvertFrom(Convert.FromBase64String(base64String));
            if (rbnMan.IsChecked == true)
            {
                card.Gender = "M";
            }
            else if (rbnVrouw.IsChecked == true)
            {
                card.Gender = "F";
            }
            else if (rbnOnbekend.IsChecked == true)
            {
                card.Gender = "O";
            }
            return card;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveToFile(openFileDialog.FileName);
                MessageBox.Show("Bestand is opgeslagen");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"ERROR: Kan bestand niet overschrijven! {ex.Message}", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                Vcard card = ToVcard();
                File.WriteAllText(fileName, card.GenerateVcardCode());
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

        private Vcard ReadVcardFromFile(string filePath)
        {
            Vcard card = new Vcard();

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
                        card.FirstName = firstName;
                        card.LastName = lastName;
                    }

                    // emails
                    else if (line.StartsWith(workEmailPrefix))
                    {
                        string workEmail = line.Substring(39);
                        card.WorkEmail = workEmail;
                    }
                    else if (line.StartsWith(homeEmailPrefix))
                    {
                        string homeEmail = line.Substring(39);
                        card.HomeEmail = homeEmail;
                    }

                    // telefoon
                    else if (line.StartsWith(homePhonePrefix))
                    {
                        string homePhone = line.Substring(20);
                        card.HomePhone = homePhone;
                    }
                    else if (line.StartsWith(workPhonePrefix))
                    {
                        string workPhone = line.Substring(20);
                        card.WorkPhone = workPhone;
                    }

                    // bedrijf
                    else if (line.StartsWith(companyPrefix))
                    {
                        string company = line.Substring(18);
                        card.Company = company;
                    }

                    // titel
                    else if (line.StartsWith(titlePrefix))
                    {
                        string title = line.Substring(20);
                        card.JobTitel = title;
                    }

                    // sociale media
                    else if (line.StartsWith(facebookPrefix))
                    {
                        string facebook = line.Substring(30);
                        card.Facebook = facebook;
                    }
                    else if (line.StartsWith(instagramPrefix))
                    {
                        string instagram = line.Substring(31);
                        card.Instagram = instagram;
                    }
                    else if (line.StartsWith(linkedinPrefix))
                    {
                        string linkedin = line.Substring(30);
                        card.Linkedin = linkedin;
                    }
                    else if (line.StartsWith(youtubePrefix))
                    {
                        string youtube = line.Substring(29);
                        card.Youtube = youtube;
                    }

                    // geslacht
                    else if (line.Contains("GENDER"))
                    {
                        string[] parts = line.Split(':', ';');
                        card.Gender = Convert.ToString(parts[1]);
                    }

                    // geboortedatum
                    else if (line.StartsWith("BDAY:"))
                    {
                        string dateField = line.Substring(5);
                        DateTime birthDate = DateTime.ParseExact(dateField, "yyyyMMdd", CultureInfo.InvariantCulture);
                        card.BirthDate = birthDate;
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
                        card.Img = image;
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

            return card;
        }

        private void ShowVcard(Vcard vcard)
        {
            txtVoornaam.Text = vcard.FirstName;
            txtAchternaam.Text = vcard.LastName;
            txtWerkemail.Text = vcard.WorkEmail;
            txtEmail.Text = vcard.HomeEmail;
            txtWerktelefoon.Text = vcard.WorkPhone;
            txtTelefoon.Text = vcard.HomePhone;
            txtJobtitel.Text = vcard.JobTitel;
            txtBedrijf.Text = vcard.Company;
            txtFacebook.Text = vcard.Facebook;
            txtLindkedin.Text = vcard.Linkedin;
            txtInstagram.Text = vcard.Instagram;
            txtYoutube.Text = vcard.Youtube;
            datGeboortedatum.SelectedDate = vcard.BirthDate;
            imgFoto.Source = vcard.Img;
            switch (vcard.Gender)
            {
                case "F":
                    rbnVrouw.IsChecked = true;
                    break;
                case "M":
                    rbnMan.IsChecked = true;
                    break;
                default:
                    rbnOnbekend.IsChecked = true;
                    break;
            }

        }
    }
}
