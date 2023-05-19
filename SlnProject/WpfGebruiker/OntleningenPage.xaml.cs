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
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for OntleningenPage.xaml
    /// </summary>
    public partial class OntleningenPage : Page
    {
        public OntleningenPage(int gebruikerId)
        {
            InitializeComponent();
            List<Ontlening> mijnOntleningen = Ontlening.GetOntleningen(gebruikerId);
            foreach (Ontlening ontl in mijnOntleningen)
            {
                ListBoxItem item = new ListBoxItem();
                TextBlock tb = new TextBlock();
                tb.Text = ontl.ToString();

                switch (ontl.OntleningStatus)
                {
                    case Ontlening.Status.InAanvraag:
                        tb.Foreground = Brushes.Orange;
                        break;
                    case Ontlening.Status.Goedgekeurd:
                        tb.Foreground = Brushes.Green;
                        break;
                    case Ontlening.Status.Verworpen:
                        tb.Foreground = Brushes.Red;
                        break;
                }
                item.Content = tb;
                MijnOntleningenListBox.Items.Add(item);
            }
        }

    }
}
