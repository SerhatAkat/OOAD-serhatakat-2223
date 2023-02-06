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
        private bool CheckForm()
        {
            if (dtmDatum.SelectedDate == null)
            {
                txtBMessageDeadline.Foreground = Brushes.Red;
                txtBMessageDeadline.Text = "gelieve een deadline te kiezen";
                return false;
            }
            if (rbnAdam.IsChecked == false && rbnBilal.IsChecked == false && rbnChelsey.IsChecked == false)
            {
                txtBMessageUitvoerder.Foreground = Brushes.Red;
                txtBMessageUitvoerder.Text = "gelieve een uitvoerder te kiezen";
                return false;
            }
            else
            {
                return true;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

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
        }
    }
}
