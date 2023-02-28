using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public MainWindow()
        {
            InitializeComponent();
            PopulateFileList(lstFiles1);
            PopulateFileList(lstFiles2);
            lstFiles1.SelectionChanged += FileList_SelectionChanged;
            lstFiles2.SelectionChanged += FileList_SelectionChanged;
        }
        private void PopulateFileList(ListBox listBox)
        {
            listBox.Items.Clear();
            string[] files = Directory.GetFiles(folderPath, "*.txt");
            foreach (string file in files)
            {
                listBox.Items.Add(Path.GetFileName(file));
            }
        }
        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            string fileName = (string)listBox.SelectedItem;
            string filePath = Path.Combine(folderPath, fileName);
            string fileContents = File.ReadAllText(filePath);

            if (listBox == lstFiles1)
            {
                lstSummary1.Items.Clear();
                foreach (string line in fileContents.Split('\n'))
                {
                    lstSummary1.Items.Add(line);

                }
            }
            else if (listBox == lstFiles2)
            {
                lstSummary2.Items.Clear();
                foreach (string line in fileContents.Split('\n'))
                {
                    lstSummary2.Items.Add(line);
                }
            }
        }

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            List<string> differences = new List<string>();
            foreach (string line in lstSummary1.Items.Cast<string>())
            {
                int index = lstSummary1.Items.IndexOf(line);
                if (lstSummary2.Items.Count > index && line != lstSummary2.Items[index] as string)
                {
                    differences.Add(line);
                }
            }

            lstSummary2.SelectedItem = null;
            foreach (string line in differences)
            {
                int index = lstSummary1.Items.IndexOf(line);
                lstSummary2.SelectedItem = lstSummary2.Items[index];
            }
        }
    }
}
