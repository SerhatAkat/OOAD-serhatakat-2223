using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfKerstverlichting
{
    public partial class MainWindow : Window
    {
        private Random rnd;
        private List<Ellipse> lights;
        private bool blinking;

        public MainWindow()
        {
            InitializeComponent();
            this.Show();
            rnd = new Random();
            lights = new List<Ellipse>();

            for (int i = 0; i < 40; i++)
            {
                Ellipse newLight = new Ellipse();
                newLight.Width = 10;
                newLight.Height = 10;
                newLight.Fill = Brushes.Gray;
                newLight.Stroke = Brushes.Black;

                double xPos = 0;
                double yPos = 0;
                bool isWhitePixel = true;

                while (isWhitePixel)
                {
                    xPos = rnd.Next(0, (int)imgTree.ActualWidth);
                    yPos = rnd.Next(0, (int)imgTree.ActualHeight);
                    isWhitePixel = PixelIsWhite(imgTree, (int)xPos, (int)yPos);
                }

                newLight.SetValue(Canvas.LeftProperty, (double)xPos);
                newLight.SetValue(Canvas.TopProperty, (double)yPos);
                cnvTree.Children.Add(newLight);
                lights.Add(newLight);
            }

            blinking = false;
            btnSwitch.Click += BtnSwitch_Click;
        }

        private async void BtnSwitch_Click(object sender, RoutedEventArgs e)
        {
            blinking = !blinking;
            btnSwitch.Content = blinking ? "SWITCH OFF" : "SWITCH ON";

            while (blinking)
            {
                foreach (Ellipse light in lights)
                {
                    light.Fill = (rnd.Next(0, 2) == 1) ? Brushes.White : Brushes.Gray;
                }
                await Task.Delay(500);
            }

            foreach (Ellipse light in lights)
            {
                light.Fill = Brushes.Gray;
            }
        }

        public static bool PixelIsWhite(Image img, int x, int y)
        {
            BitmapSource source = img.Source as BitmapSource;
            Color color = Colors.White;
            CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
            byte[] pixels = new byte[4];
            cb.CopyPixels(pixels, 4, 0);
            color = Color.FromRgb(pixels[2], pixels[1], pixels[0]);
            return color.ToString() == "#FFFFFFFF";
        }
    }
}
