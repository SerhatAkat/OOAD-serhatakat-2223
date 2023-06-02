using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gebruiker ingelogdeGebruiker;

        public MainWindow(Gebruiker gebruiker)
        {
            InitializeComponent();
            ingelogdeGebruiker = gebruiker;
            MainFrame.Navigate(new HomePage(ingelogdeGebruiker));
            ToonProfielFoto();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new HomePage(ingelogdeGebruiker);
        }

        private void BtnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OntleningenPage(ingelogdeGebruiker.Id);
        }

        private void BtnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new VoertuigenPage(ingelogdeGebruiker.Id);
        }
        private void ToonProfielFoto()
        {
            byte[] imageData = ingelogdeGebruiker.Profielfoto;
            if (imageData != null && imageData.Length > 0)
            {
                BitmapImage imageSource = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.StreamSource = stream;
                    imageSource.EndInit();
                }
                imgProfiel.Source = imageSource;
            }
        }
    }
}
