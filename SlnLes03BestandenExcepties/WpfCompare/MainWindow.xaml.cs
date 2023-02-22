using System;
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
            var files = Directory.GetFiles(folderPath, "*.txt");
            foreach (var file in files)
            {
                listBox.Items.Add(Path.GetFileName(file));
            }
        }
        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var fileName = (string)listBox.SelectedItem;
            var filePath = Path.Combine(folderPath, fileName);
            var fileContents = File.ReadAllText(filePath);

            if (listBox == lstFiles1)
            {
                lstSummary1.Items.Clear();
                foreach (var line in fileContents.Split('\n'))
                {
                    lstSummary1.Items.Add(line);

                }
            }
            else if (listBox == lstFiles2)
            {
                lstSummary2.Items.Clear();
                foreach (var line in fileContents.Split('\n'))
                {
                    lstSummary2.Items.Add(line);
                }
            }
        }

        private void btnCompare_Click(object sender, RoutedEventArgs e)
        {
            var differences = lstSummary1.Items.Cast<string>()
                .Select((line, index) => new { Line = line, Index = index })
                .Where(item => lstSummary2.Items.Count > item.Index && item.Line != lstSummary2.Items[item.Index] as string)
                .Select(item => item.Index);

            lstSummary2.SelectedItem = null;
            foreach (var index in differences)
            {
                lstSummary2.SelectedItem = lstSummary2.Items[index];
            }
        }
    }
}
