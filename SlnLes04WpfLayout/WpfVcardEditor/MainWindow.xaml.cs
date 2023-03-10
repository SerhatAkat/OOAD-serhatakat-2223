﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
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
                string fileName = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(fileName);

                foreach(string line in lines)
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
                    else if(line.StartsWith(homePhonePrefix))
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
                }
            }
        }
    }
}
