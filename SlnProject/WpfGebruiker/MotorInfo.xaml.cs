using System;
using System.Collections.Generic;
using System.IO;
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
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MotorInfo.xaml
    /// </summary>
    public partial class MotorInfo : Page
    {
        public MotorInfo(Voertuig voertuig)
        {
            InitializeComponent();

            lblMotorNaam.Content = voertuig.Naam;
            lblMotorBeschrijving.Content = "Beschrijving: " + voertuig.Beschrijving;
            lblMotorMerk.Content = "Merk: " + voertuig.Merk;
            lblMotorBouwjaar.Content = "Bouwjaar: " + voertuig.Bouwjaar;
            lblMotorModel.Content = "Model: " + voertuig.Model;
            lblMotorTransmissie.Content = "Transmissie: " + voertuig.TransmissieType?.ToString();
            lblMotorBrandstof.Content = "Brandstof: " + voertuig.BrandstofType?.ToString();
            lblMotorEigenaar.Content = "Eigenaar: " + voertuig.Eigenaar;

            Foto foto = Foto.GetFotoForVoertuig(voertuig.Id);
            if (foto != null && foto.Image != null)
            {
                BitmapImage bitmap = new BitmapImage();
                using (var mem = new MemoryStream(foto.Image))
                {
                    mem.Position = 0;
                    bitmap.BeginInit();
                    bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = null;
                    bitmap.StreamSource = mem;
                    bitmap.EndInit();
                }
                bitmap.Freeze();
                imgMotorFoto.Source = bitmap;
            }
        }
    }
}
