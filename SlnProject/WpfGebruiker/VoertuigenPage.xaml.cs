using System;
using System.Collections.Generic;
using System.IO;
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
using FontAwesome.WPF;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for VoertuigenPage.xaml
    /// </summary>
    public partial class VoertuigenPage : Page
    {
        private int userId;

        public VoertuigenPage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            UpdateVoertuigen();
        }

        private void UpdateVoertuigen()
        {
            pnlItems.Children.Clear();

            // Haal de lijst met voertuigen op van de huidige ingelogde gebruiker
            List<Voertuig> voertuigen = Voertuig.GetAllVoertuigenOwnedByGebruiker(userId);

            foreach (Voertuig voertuig in voertuigen)
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
                txtNaam.Margin = new Thickness(10, 20, 0, 0);

                TextBlock txtMerk = new TextBlock();
                txtMerk.Text = "Merk: " + voertuig.Merk;
                txtMerk.TextWrapping = TextWrapping.Wrap;
                txtMerk.Margin = new Thickness(10, 10, 0, 0);

                TextBlock txtModel = new TextBlock();
                txtModel.Text = "Model: " + voertuig.Model;
                txtModel.Margin = new Thickness(10, 10, 0, 0);

                Button deleteBtn = new Button();
                deleteBtn.Content = new ImageAwesome
                {
                    Icon = FontAwesomeIcon.Trash,
                    Width = 16,
                    Height = 16,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(5)
                };
                deleteBtn.Margin = new Thickness(20, 0, 0, 0);

                Button editBtn = new Button();
                editBtn.Content = new ImageAwesome
                {
                    Icon = FontAwesomeIcon.Pencil,
                    Width = 16,
                    Height = 16,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(5)

                };
                editBtn.Margin = new Thickness(5, 0, 0, 0);

                Button infoBtn = new Button();
                infoBtn.Content = new ImageAwesome
                {
                    Icon = FontAwesomeIcon.InfoCircle,
                    Width = 16,
                    Height = 16,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(5)

                };
                infoBtn.Margin = new Thickness(5, 0, 0, 0);

                StackPanel infoPanel = new StackPanel();
                infoPanel.Orientation = Orientation.Horizontal;
                infoPanel.Margin = new Thickness(0, 20, 0, 0);
                infoPanel.Children.Add(deleteBtn);
                infoPanel.Children.Add(editBtn);
                infoPanel.Children.Add(infoBtn);

                StackPanel textPanel = new StackPanel();
                textPanel.Orientation = Orientation.Vertical;
                textPanel.Children.Add(txtNaam);
                textPanel.Children.Add(txtMerk);
                textPanel.Children.Add(txtModel);
                textPanel.Children.Add(infoPanel);

                Grid grid = new Grid();
                grid.Width = 300;
                grid.Height = 150;
                grid.Margin = new Thickness(20, 0, 0, 0);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                img.SetValue(Grid.ColumnProperty, 0);
                textPanel.SetValue(Grid.ColumnProperty, 1);
                grid.Children.Add(img);
                grid.Children.Add(textPanel);

                Border border = new Border();
                border.Width = 300;
                border.Height = 150;
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(1);
                border.Margin = new Thickness(5);
                border.HorizontalAlignment = HorizontalAlignment.Center;
                border.VerticalAlignment = VerticalAlignment.Center;
                border.Child = grid;

                pnlItems.Children.Add(border);
            }
        }

    }
}
