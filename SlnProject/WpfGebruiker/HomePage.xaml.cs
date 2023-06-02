using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FontAwesome.WPF;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private Gebruiker userid;
        public HomePage(Gebruiker userId)
        {
            this.userid = userId;
            InitializeComponent();

            UpdateVoertuigen();
        }

        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            UpdateVoertuigen();
        }

        private void UpdateVoertuigen()
        {
            bool tonenGemotoriseerd = chkGemotoriseerd.IsChecked == true;
            bool tonenGetrokken = chkGetrokken.IsChecked == true;

            pnlItems.Children.Clear();

            // Haal de lijst met voertuigen op
            List<Voertuig> voertuigen = null;
            try
            {
                voertuigen = Voertuig.GetVoertuigenVanAnderen(userid.Id);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Databasefout bij het ophalen van voertuigen: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden bij het ophalen van voertuigen: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            foreach (Voertuig voertuig in voertuigen)
            {
                if ((tonenGemotoriseerd && voertuig.Type == 1) ||
                    (tonenGetrokken && voertuig.Type == 2) ||
                    (!tonenGemotoriseerd && !tonenGetrokken))
                {
                    Foto foto = Foto.GetFotoForVoertuig(voertuig.Id);

                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream mem = new MemoryStream(foto.Image))
                    {
                        mem.Position = 0;
                        bitmap.BeginInit();
                        bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.UriSource = null;
                        bitmap.StreamSource = mem;
                        bitmap.EndInit();
                    }
                    bitmap.Freeze();

                    Image img = new Image();
                    img.Width = 120;
                    img.Source = bitmap;

                    TextBlock txtNaam = new TextBlock();
                    txtNaam.Text = voertuig.Naam;
                    txtNaam.TextWrapping = TextWrapping.Wrap;
                    txtNaam.FontWeight = FontWeights.Bold;
                    txtNaam.VerticalAlignment = VerticalAlignment.Center;
                    txtNaam.Margin = new Thickness(10, 10, 0, 0);

                    TextBlock txtMerk = new TextBlock();
                    txtMerk.Text = "Merk: " + voertuig.Merk;
                    txtMerk.TextWrapping = TextWrapping.Wrap;
                    txtMerk.VerticalAlignment = VerticalAlignment.Center;
                    txtMerk.Margin = new Thickness(10, 10, 0, 0);

                    TextBlock txtModel = new TextBlock();
                    txtModel.Text = "Model: " + voertuig.Model;
                    txtModel.Margin = new Thickness(10, 10, 0, 0);
                    txtModel.TextWrapping = TextWrapping.Wrap;

                    // Maak een knop voor voertuig info
                    Button btn = new Button();
                    btn.Tag = voertuig.Id;
                    btn.Width = 30;
                    btn.Height = 30;
                    btn.HorizontalAlignment = HorizontalAlignment.Right;

                    btn.Click += VoertuigInfoButton_Click;

                    // Voeg een FontAwesome icoon toe aan de knop
                    ImageAwesome infoIcon = new ImageAwesome();
                    infoIcon.Icon = FontAwesomeIcon.InfoCircle;
                    infoIcon.Width = 16;
                    infoIcon.Height = 16;
                    infoIcon.Foreground = Brushes.Black;

                    // Maak een StackPanel om de labels te groeperen
                    StackPanel pnl = new StackPanel();
                    pnl.Orientation = Orientation.Vertical;
                    pnl.Children.Add(txtNaam);
                    pnl.Children.Add(txtMerk);
                    pnl.Children.Add(txtModel);

                    // Maak de inhoud van de knop het icoon
                    btn.Content = infoIcon;

                    // Maak een Grid om de afbeelding, button en het StackPanel te groeperen
                    Grid grid = new Grid();
                    grid.Width = 300;
                    grid.Height = 150;
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30, GridUnitType.Pixel) });
                    img.SetValue(Grid.ColumnProperty, 0);
                    pnl.SetValue(Grid.ColumnProperty, 1);
                    btn.SetValue(Grid.ColumnProperty, 2);
                    grid.Children.Add(img);
                    grid.Children.Add(pnl);
                    grid.Children.Add(btn);

                    // Voeg een Border toe
                    Border border = new Border();
                    border.Width = 300;
                    border.Height = 150;
                    border.BorderBrush = Brushes.Black;
                    border.BorderThickness = new Thickness(1);
                    border.Margin = new Thickness(5);
                    border.HorizontalAlignment = HorizontalAlignment.Center;
                    border.VerticalAlignment = VerticalAlignment.Center;
                    border.Child = grid;

                    // Voeg de Border toe aan het hoofdpaneel
                    pnlItems.Children.Add(border);
                }
            }
        }

        private void VoertuigInfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Haal de Voertuig ID uit de Tag van de knop
            Button knop = sender as Button;
            int voertuigId = int.Parse(knop.Tag.ToString());

            // Zoek het Voertuig in de list
            List<Voertuig> alleVoertuigen = null;
            try
            {
                alleVoertuigen = Voertuig.GetAllVoertuigen();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Databasefout bij het ophalen van alle voertuigen: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden bij het ophalen van alle voertuigen: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Voertuig gevondenVoertuig = null;
            foreach (Voertuig voertuig in alleVoertuigen)
            {
                if (voertuig.Id == voertuigId)
                {
                    gevondenVoertuig = voertuig;
                    break;
                }
            }

            if (gevondenVoertuig != null)
            {
                if (gevondenVoertuig.Type == 1)
                {
                    MotorInfo pagina = new MotorInfo(gevondenVoertuig, userid.Id);
                    this.NavigationService.Navigate(pagina);
                }
                else if (gevondenVoertuig.Type == 2)
                {
                    GetrokkenInfo pagina = new GetrokkenInfo(gevondenVoertuig, userid.Id);
                    this.NavigationService.Navigate(pagina);
                }
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
