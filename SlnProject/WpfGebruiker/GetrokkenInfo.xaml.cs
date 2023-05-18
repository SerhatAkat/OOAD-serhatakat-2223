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
    /// Interaction logic for GetrokkenInfo.xaml
    /// </summary>
    public partial class GetrokkenInfo : Page
    {
        public GetrokkenInfo(Voertuig voertuig)
        {
            InitializeComponent();

            LoadFotosForVoertuig(voertuig.Id);

            lblGetrokkenNaam.Content = voertuig.Naam;
            lblGetrokkenBeschrijving.Content = "Beschrijving: " + voertuig.Beschrijving;
            lblGetrokkenMerk.Content = "Merk: " + (string.IsNullOrEmpty(voertuig.Merk) ? "n.v.t." : voertuig.Merk);

            if (voertuig.Geremd.HasValue)
            {
                lblGetrokkenGeremd.Content = "Geremd: " + (voertuig.Geremd.Value ? "Ja" : "Nee");
            }
            else
            {
                lblGetrokkenGeremd.Content = "Geremd: Geen informatie beschikbaar";
            }
            lblGetrokkenModel.Content = "Model: " + (string.IsNullOrEmpty(voertuig.Model) ? "n.v.t." : voertuig.Model);
            lblGetrokkenAfmetingen.Content = "Afmetingen: " + (string.IsNullOrEmpty(voertuig.Afmetingen) ? "n.v.t." : voertuig.Afmetingen);
            lblGetrokkenGewicht.Content = "Gewicht: " + (voertuig.Gewicht.HasValue ? voertuig.Gewicht.Value.ToString() : "n.v.t.") + " kg";
            lblGetrokkenBouwjaar.Content = "Bouwjaar: " + voertuig.Bouwjaar;
            lblGetrokkenBelasting.Content = "Max. belasting: " + (voertuig.MaxBelasting.HasValue ? voertuig.MaxBelasting.Value.ToString() : "n.v.t.") + " kg";
            string eigenaarNaam = Gebruiker.GetGebruikerNaamById(voertuig.Eigenaar);
            lblGetrokkenEigenaar.Content = "Eigenaar: " + (eigenaarNaam ?? "Onbekend");
        }

        private void LoadFotosForVoertuig(int voertuigId)
        {
            List<Foto> fotos = Foto.GetFotosForVoertuig(voertuigId); // Dit moet een lijst van Foto objecten teruggeven die overeenkomen met het gegeven voertuig ID

            foreach (Foto foto in fotos)
            {
                Image img = new Image();
                img.Source = ConvertByteArrayToBitmapImage(foto.Image);
                img.Width = 150;
                img.Margin = new Thickness(0, 0, 10, 0);
                stkGetrokkenFotos.Children.Add(img);
            }
        }

        private BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }
    }
}
