using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for ToevoegenMotor.xaml
    /// </summary>
    public partial class ToevoegenMotor : Window
    {
        public ToevoegenMotor()
        {
            InitializeComponent();
            btnUploaden.Click += BtnUploaden_Click;
        }
        private void BtnUploaden_Click(object sender, RoutedEventArgs e)
        {
            int loadedImagesCount = 0;

            if (img1.Source != null) loadedImagesCount++;
            if (img2.Source != null) loadedImagesCount++;
            if (img3.Source != null) loadedImagesCount++;

            // Als er al 1, 2 of 3 afbeeldingen zijn, hoef je het dialoogvenster niet te openen
            if (loadedImagesCount > 0)
            {
                return;
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

                // Code zoals voorheen
                if (images.Count > 0)
                {
                    img1.Source = images[0];
                }
                if (images.Count > 1)
                {
                    img2.Source = images[1];
                }
                if (images.Count > 2)
                {
                    img3.Source = images[2];
                }
            }
        }
        private void VerwijderAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            switch (button.Name)
            {
                case "btnVerwijder1":
                    if (img1.Source != null) img1.Source = null;  // Check if an image exists before trying to remove it
                    break;
                case "btnVerwijder2":
                    if (img2.Source != null) img2.Source = null;  // Check if an image exists before trying to remove it
                    break;
                case "btnVerwijder3":
                    if (img3.Source != null) img3.Source = null;  // Check if an image exists before trying to remove it
                    break;
            }
        }
    }
}
