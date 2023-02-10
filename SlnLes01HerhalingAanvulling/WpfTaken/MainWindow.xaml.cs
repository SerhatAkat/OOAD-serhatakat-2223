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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTaken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // globale variable om te zien of button toevoegen is geklikt of niet.
        int isclicked = 0;
        Stack<ListBoxItem> deletedItems = new Stack<ListBoxItem>();
        // checkForm methode
        private bool CheckForm()
        {
            bool formValid = true;
            txtBMessage.Text = "";
            txtBMessage.Foreground = Brushes.Red;
            if (txtTaak.Text == "")
            {
                txtBMessage.Text += "gelieve een Taak in te vullen \n";
                formValid = false;
            }
            if (cbxPrioriteit.SelectedIndex == -1)
            {
                txtBMessage.Text += "gelieve een Prioriteit aan te duiden \n";
                formValid = false;
            }
            if (dtmDatum.SelectedDate == null)
            {
                txtBMessage.Text += "gelieve een deadline te kiezen \n";
                formValid = false;
            }
            if (rbnAdam.IsChecked == false && rbnBilal.IsChecked == false && rbnChelsey.IsChecked == false)
            {
                txtBMessage.Text += "gelieve een uitvoerder te kiezen \n";
                formValid = false;
            }
            return formValid;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        // items toevoegen in de lijst
        private void btnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            isclicked = 1;
            if (CheckForm() == true && rbnAdam.IsChecked == true)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = $"{txtTaak.Text} ({dtmDatum.SelectedDate}; door: {rbnAdam.Content})";
                lstTaken.Items.Add(item);
                if (cbxPrioriteit.SelectedIndex == 0)
                {
                    item.Background = Brushes.Red;
                }
                else if (cbxPrioriteit.SelectedIndex == 1)
                {
                    item.Background = Brushes.LightYellow;
                }
                else if (cbxPrioriteit.SelectedIndex == 2)
                {
                    item.Background = Brushes.Green;
                }
            }
            else if (CheckForm() == true && rbnBilal.IsChecked == true)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = $"{txtTaak.Text} ({dtmDatum.SelectedDate}; door: {rbnBilal.Content})";
                lstTaken.Items.Add(item);
                if (cbxPrioriteit.SelectedIndex == 0)
                {
                    item.Background = Brushes.Red;
                }
                else if (cbxPrioriteit.SelectedIndex == 1)
                {
                    item.Background = Brushes.LightYellow;
                }
                else if (cbxPrioriteit.SelectedIndex == 2)
                {
                    item.Background = Brushes.Green;
                }
            }
            else if (CheckForm() == true && rbnChelsey.IsChecked == true)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = $"{txtTaak.Text} ({dtmDatum.SelectedDate}; door: {rbnChelsey.Content})";
                lstTaken.Items.Add(item);
                if (cbxPrioriteit.SelectedIndex == 0)
                {
                    item.Background = Brushes.Red;
                }
                else if (cbxPrioriteit.SelectedIndex == 1)
                {
                    item.Background = Brushes.LightYellow;
                }
                else if (cbxPrioriteit.SelectedIndex == 2)
                {
                    item.Background = Brushes.Green;
                }
            }
            if (lstTaken.Items.Count > 0)
            {
                btnVerwijderen.IsEnabled = true;
            }
            else if (lstTaken.Items.Count == 0)
            {
                btnVerwijderen.IsEnabled = false;
            }
            txtTaak.Text = "";
            cbxPrioriteit.SelectedIndex = -1;
            dtmDatum.SelectedDate = null;
            rbnAdam.IsChecked = false;
            rbnBilal.IsChecked = false;
            rbnChelsey.IsChecked = false;
            CheckForm();
        }
        //geselecteerde item uit de lijst verwijderen en bijhouden met Push()
        private void btnVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = lstTaken.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                deletedItems.Push(selectedItem);
                lstTaken.Items.Remove(selectedItem);
            }
            ButtonStatus();
        }
        //verwijderde items terugzetten met Pop()
        private void btnTerugzetten_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = lstTaken.SelectedItem as ListBoxItem;
            if (selectedItem != null)
            {
                deletedItems.Push(selectedItem);
                lstTaken.Items.Remove(selectedItem);
            }
            if (deletedItems.Count > 0)
            {
                ListBoxItem restoredItem = deletedItems.Pop();
                lstTaken.Items.Add(restoredItem);
                btnVerwijderen.IsEnabled = true;
            }
            if (deletedItems.Count <= 0)
            {
                btnTerugzetten.IsEnabled = false;
            }
        }
        private void ButtonStatus()
        {
            if (lstTaken.Items.Count == 0)
            {
                btnVerwijderen.IsEnabled = false;
            }
            if (deletedItems.Count > 0)
            {
                btnTerugzetten.IsEnabled = true;
            }
            if (deletedItems.Count < 0)
            {
                btnTerugzetten.IsEnabled = false;
            }
        }
        private void dtmDatum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isclicked == 1)
            {
                CheckForm();
            }
        }
        private void rbnAdam_Checked(object sender, RoutedEventArgs e)
        {
            if (isclicked == 1)
            {
                CheckForm();
            }
        }
        private void rbnBilal_Checked(object sender, RoutedEventArgs e)
        {
            if (isclicked == 1)
            {
                CheckForm();
            }
        }
        private void rbnChelsey_Checked(object sender, RoutedEventArgs e)
        {
            if (isclicked == 1)
            {
                CheckForm();
            }
        }
        private void txtTaak_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isclicked == 1)
            {
                CheckForm();
            }
        }
        private void cbxPrioriteit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isclicked == 1)
            {
                CheckForm();
            }
        }
    }
}