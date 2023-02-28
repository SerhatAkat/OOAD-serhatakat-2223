using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;


namespace WpfCopy
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
        string content = "";
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private void btnKies_Click(object sender, RoutedEventArgs e)
        {
            string chosenFileName = "";
            btnGo.IsEnabled = true;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";

            if (dialog.ShowDialog() == true)
            {
                chosenFileName = dialog.FileName;
                txtFile.Text = chosenFileName;

                try
                {
                    string filePath = System.IO.Path.Combine(folderPath, chosenFileName);
                    content = File.ReadAllText(filePath);
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(
                        $"{ex.FileName} kon niet gevonden worden", // boodschap
                        "Oeps!", // titel
                        MessageBoxButton.OK, // buttons
                        MessageBoxImage.Error
                    ); // error icoon
                }

            }
            else
            {
                lblMsg.Content = "Kies een bestand";
            }

        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            dialog.FileName = "savedfile.txt";
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, content);
                lblMsg.Content = "Bestand is overgezet";
            }
            else
            {
                lblMsg.Content = "Bestand is niet overgezet";
            }

        }
    }
}
