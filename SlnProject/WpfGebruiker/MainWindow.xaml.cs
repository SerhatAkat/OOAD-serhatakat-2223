using System.Windows;
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
    }
}
