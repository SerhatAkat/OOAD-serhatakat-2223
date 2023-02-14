using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfMatchImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private Stopwatch stopwatch = new Stopwatch();
        private Button eersteButton;
        private Button secondButton;
        int AantalParen = 8;

        public MainWindow()
        {
            InitializeComponent();
            lblMessage.Content = "Er zijn 8 paren die aangeklikt moeten worden.";
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;

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
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lblTimer.Content = stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:ff");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                stopwatch.Start();
            }
            if (eersteButton == null)
            {
                eersteButton = sender as Button;
                eersteButton.IsEnabled = false;
            }
            else
            {
                secondButton = sender as Button;
                secondButton.IsEnabled = false;
                if (eersteButton.Tag.Equals(secondButton.Tag))
                {
                    eersteButton.Opacity = 0.5;
                    secondButton.Opacity = 0.5;
                    AantalParen--;
                    lblMessage.Content = $"Juist! Nog {AantalParen} te gaan";
                    if (AantalParen == 0)
                    {
                        timer.Stop();
                        stopwatch.Stop();
                        lblMessage.Content = $"Alles gevonden!";
                    }
                }
                else
                {
                    eersteButton.IsEnabled = true;
                    secondButton.IsEnabled = true;
                }
                eersteButton = null;
            }
        }
    }
}
