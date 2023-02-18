using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace WpfFileInfo
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

        private void btnBestand_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Tekstbestanden|*.TXT;*.TEXT";

            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
                string content = File.ReadAllText(fileName);
                int teller = 0;

                for (int i = 0; i < content.Length - 1; i++)
                {
                    if (content[i] == ' ' && Char.IsLetter(content[i + 1]) && (i > 0))
                    {
                        teller++;
                    }
                }

                string[] lines = content.Split();
                Dictionary<string, int> AantalWoorden = new Dictionary<string, int>();
                foreach (string line in lines)
                {
                    if (line == "") continue;
                    if (AantalWoorden.ContainsKey(line.ToLower()))
                        AantalWoorden[line.ToLower()]++;
                    else
                        AantalWoorden[line.ToLower()] = 1;
                }
                AantalWoorden = AantalWoorden.OrderByDescending(a => a.Value).Take(3).ToDictionary(a => a.Key, x => x.Value);

                FileInfo fi = new FileInfo(fileName);
                lblNaam.Content = $"bestandsnaam: {fi.Name}";
                lblExtensie.Content = $"extensie: {fi.Extension}";
                lblDatum.Content = $"gemaakt op: {fi.CreationTime}";
                lblMap.Content = $"mapnaam: {fi.Directory.Name}";
                lblAantal.Content = $"aantal woorden: {teller}";
                lblWoorden.Content = $"meest voorkomende woorden: {string.Join(", ", AantalWoorden.Keys)}";
            }
        }
    }
}
