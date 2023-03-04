using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfFocusTab
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
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            txtBox.Background = Brushes.LimeGreen;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            txtBox.Background = Brushes.Transparent;
        }
    }
}
