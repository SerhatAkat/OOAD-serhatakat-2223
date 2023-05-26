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

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new HomePage(ingelogdeGebruiker);
        }

        private void btnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new OntleningenPage(ingelogdeGebruiker.Id);
        }


        private void btnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new VoertuigenPage();
        }
    }
}
