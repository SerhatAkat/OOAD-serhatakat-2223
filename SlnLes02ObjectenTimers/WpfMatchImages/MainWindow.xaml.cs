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
using System.Windows.Threading;

namespace WpfMatchImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer= new DispatcherTimer();
        private int count = 0;
        public MainWindow()
        {
            InitializeComponent();
            btnBird.Click += Button_Click;
            btnCat.Click += Button_Click;
            btnChicken.Click += Button_Click;
            btnCow.Click += Button_Click;
            btnDog.Click += Button_Click;
            btnEgg.Click += Button_Click;
            btnHive.Click += Button_Click;
            btnHouse.Click += Button_Click;
            btnLeash.Click += Button_Click;
            btnMilk.Click += Button_Click;
            btnNight.Click += Button_Click;
            btnOwl.Click += Button_Click;
            btnPenguin.Click += Button_Click;
            btnPost.Click += Button_Click;
            btnSnow.Click += Button_Click;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            count++;
            lblTimer.Content = count;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}
