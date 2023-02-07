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
        Stack<ListBoxItem> deletedItems = new Stack<ListBoxItem>();
        // checkForm methode
        private bool CheckForm()
        {
            bool formValid = true;
            if (dtmDatum.SelectedDate == null)
            {
                txtBMessageDeadline.Foreground = Brushes.Red;
                txtBMessageDeadline.Text = "gelieve een deadline te kiezen";
                formValid = false;
            }
            if (rbnAdam.IsChecked == false && rbnBilal.IsChecked == false && rbnChelsey.IsChecked == false)
            {
                txtBMessageUitvoerder.Foreground = Brushes.Red;
                txtBMessageUitvoerder.Text = "gelieve een uitvoerder te kiezen";
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

        private void dtmDatum_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            txtBMessageDeadline.Text = "";
        }

        private void rbnAdam_Checked(object sender, RoutedEventArgs e)
        {
            txtBMessageUitvoerder.Text = "";
        }

        private void rbnBilal_Checked(object sender, RoutedEventArgs e)
        {
            txtBMessageUitvoerder.Text = "";
        }

        private void rbnChelsey_Checked(object sender, RoutedEventArgs e)
        {
            txtBMessageUitvoerder.Text = "";
        }
    }
}