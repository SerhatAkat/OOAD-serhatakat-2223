﻿using System;
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
    /// Interaction logic for VoertuigenPage.xaml
    /// </summary>
    public partial class VoertuigenPage : Page
    {
        public static VoertuigenPage instance;
        private int userId;

        private WrapPanel cardPanel;
        private List<Voertuig> voertuigen;

        public VoertuigenPage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            UpdateVoertuigen();
            instance = this;
        }

        public void UpdateVoertuigen()
        {
            pnlItems.Children.Clear();

            // Maak een nieuwe StackPanel voor de button en de cards
            StackPanel mainPanel = new StackPanel();
            mainPanel.Orientation = Orientation.Vertical;

            Button addButton = new Button();
            addButton.Content = "Toevoegen +";
            addButton.FontSize = 16;
            addButton.Width = 150;
            addButton.Height = 40;
            addButton.Margin = new Thickness(5, 10, 0, 0);
            addButton.HorizontalAlignment = HorizontalAlignment.Left;
            addButton.Click += AddButton_Click;

            mainPanel.Children.Add(addButton);

            // Maak een nieuwe WrapPanel voor de kaarten
            cardPanel = new WrapPanel();

            // Haal de lijst met voertuigen op van de huidige ingelogde gebruiker
            List<Voertuig> voertuigen = new List<Voertuig>();

            try
            {
                voertuigen = Voertuig.GetAllVoertuigenOwnedByGebruiker(userId);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Er is een fout opgetreden bij het ophalen van de voertuigen voor gebruiker {userId}: {ex.Message}", "Databasefout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een onverwachte fout opgetreden: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            foreach (Voertuig voertuig in voertuigen)
            {
                Foto foto = Foto.GetFotoForVoertuig(voertuig.Id);

                BitmapImage bitmap = new BitmapImage();
                if (foto != null)
                {
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
                }
                else
                {
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
                deleteBtn.Click += DeleteBtn_Click;
                deleteBtn.Tag = voertuig.Id;

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
                editBtn.Click += EditBtn_Click;
                editBtn.Tag = voertuig;

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
                infoBtn.Click += BtnInfo_Click;
                infoBtn.Tag = voertuig.Id;

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

                cardPanel.Children.Add(border);
            }

            // Toevoegen cardPanel aan mainPanel
            mainPanel.Children.Add(cardPanel);

            // Toevoegen mainPanel aan pnlItems
            pnlItems.Children.Add(mainPanel);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "Voertuig type",
                Width = 350,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            Button motorButton = new Button
            {
                Content = "Gemotoriseerd",
                Margin = new Thickness(0, 10, 0, 0),
                Width = 150,
                Height = 40
            };
            motorButton.Click += MotorButton_Click;

            Button getrokkenButton = new Button
            {
                Content = "Getrokken",
                Margin = new Thickness(0, 10, 0, 0),
                Width = 150,
                Height = 40
            };
            getrokkenButton.Click += GetrokkenButton_Click;

            // Voeg buttons toe aan StackPanel
            stackPanel.Children.Add(motorButton);
            stackPanel.Children.Add(getrokkenButton);

            // Stel de StackPanel in als de Content van het venster
            window.Content = stackPanel;
            window.ShowDialog();
        }

        private void MotorButton_Click(object sender, RoutedEventArgs e)
        {
            Gebruiker gebruiker = Gebruiker.GetGebruikerById(this.userId);
            ToevoegenMotor window = new ToevoegenMotor(gebruiker);
            window.ShowDialog();
        }

        private void GetrokkenButton_Click(object sender, RoutedEventArgs e)
        {
            Gebruiker gebruiker = Gebruiker.GetGebruikerById(this.userId);
            ToevoegenGetrokken window = new ToevoegenGetrokken(gebruiker);
            window.ShowDialog();
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            // Haal de Voertuig ID uit de Tag van de knop
            Button knop = sender as Button;
            int voertuigId = int.Parse(knop.Tag.ToString());

            // Zoek het Voertuig in de list
            List<Voertuig> alleVoertuigen = new List<Voertuig>();

            try
            {
                alleVoertuigen = Voertuig.GetAllVoertuigen();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Er is een fout opgetreden bij het ophalen van alle voertuigen: {ex.Message}", "Databasefout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een onverwachte fout opgetreden: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MotorInfo pagina = new MotorInfo(gevondenVoertuig, userId, true);
                    this.NavigationService.Navigate(pagina);
                }
                else if (gevondenVoertuig.Type == 2)
                {
                    GetrokkenInfo pagina = new GetrokkenInfo(gevondenVoertuig, userId, true);
                    this.NavigationService.Navigate(pagina);
                }
            }
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            // Haal de Voertuig ID uit de tag van de knop
            Button knop = sender as Button;
            int voertuigId = int.Parse(knop.Tag.ToString());

            Voertuig.DeleteVoertuig(voertuigId);
            UpdateVoertuigen();
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            // Identificeer het te bewerken voertuig
            Button knop = sender as Button;
            Voertuig teBewerkenVoertuig = (Voertuig)knop.Tag;

            // Maak een nieuw ToevoegenMotor of ToevoegenGetrokken venster aan, afhankelijk van het type voertuig
            if (teBewerkenVoertuig.Type == 1)
            {
                EditMotor editWindow = new EditMotor(teBewerkenVoertuig);
                editWindow.ShowDialog();
                if (editWindow.DialogResult == true)
                {
                    teBewerkenVoertuig.UpdateGemotoriseerd(teBewerkenVoertuig.Id);
                }
            }
            else if (teBewerkenVoertuig.Type == 2)
            {
                EditGetrokken editWindow = new EditGetrokken(teBewerkenVoertuig);
                editWindow.ShowDialog();
                if (editWindow.DialogResult == true)
                {
                    teBewerkenVoertuig.UpdateGetrokken(teBewerkenVoertuig.Id);
                }
            }

            UpdateVoertuigen();
        }
    }
}
