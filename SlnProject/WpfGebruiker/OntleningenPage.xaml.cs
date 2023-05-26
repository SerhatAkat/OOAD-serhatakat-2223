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
        private int gebruikerId;

        public OntleningenPage(int gebruikerId)
        {
            InitializeComponent();
            this.gebruikerId = gebruikerId;
            LaadOntleningen();
            LoadAanvragen();
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            if (MijnOntleningenListBox.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontl)
            {
                if (ontl.Tot > DateTime.Now)
                {
                    Ontlening.VerwijderOntlening(ontl.Id);
                    LaadOntleningen();
                    LoadAanvragen();
                }
            }
        }

        private void LaadOntleningen()
        {
            MijnOntleningenListBox.Items.Clear();
            List<Ontlening> mijnOntleningen = Ontlening.GetOntleningen(gebruikerId);

            // Sorteer de ontleningen op de Vanaf eigenschap, van meest recent naar oudst
            for (int i = 0; i < mijnOntleningen.Count; i++)
            {
                for (int j = i + 1; j < mijnOntleningen.Count; j++)
                {
                    if (mijnOntleningen[j].Vanaf > mijnOntleningen[i].Vanaf)
                    {
                        Ontlening temp = mijnOntleningen[i];
                        mijnOntleningen[i] = mijnOntleningen[j];
                        mijnOntleningen[j] = temp;
                    }
                }
            }

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
                item.Tag = ontl;
                MijnOntleningenListBox.Items.Add(item);
            }
        }

        private void LoadAanvragen()
        {
            AanvragenListBox.Items.Clear();
            List<Ontlening> mijnOntleningen = Ontlening.GetAanvraagdeOntleningen(gebruikerId);

            // Sorteer de ontleningen op de Vanaf eigenschap, van meest recent naar oudst
            for (int i = 0; i < mijnOntleningen.Count; i++)
            {
                for (int j = i + 1; j < mijnOntleningen.Count; j++)
                {
                    if (mijnOntleningen[j].Vanaf > mijnOntleningen[i].Vanaf)
                    {
                        Ontlening temp = mijnOntleningen[i];
                        mijnOntleningen[i] = mijnOntleningen[j];
                        mijnOntleningen[j] = temp;
                    }
                }
            }

            foreach (Ontlening ontl in mijnOntleningen)
            {
                ListBoxItem item = new ListBoxItem();
                TextBlock tb = new TextBlock();
                tb.Text = ontl.ToString();
                item.Content = tb;
                item.Tag = ontl;
                AanvragenListBox.Items.Add(item);
                tb.Foreground = Brushes.Orange;
                Gebruiker ontleningAanvrager = Gebruiker.GetGebruikerById(gebruikerId);
                string gebruikerNaam = $"{ontleningAanvrager.Voornaam.Substring(0, 1)}.{ontleningAanvrager.Achternaam}";
                string displayText = $"{ontl.VoertuigNaam} - {ontl.Vanaf.ToString("yyyy-MM-dd 00:00")} - {ontl.Tot.ToString("yyyy-MM-dd 00:00")} door {gebruikerNaam}";
                tb.Text = displayText.ToString();
            }
        }

        private void MijnOntleningenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MijnOntleningenListBox.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontl)
            {
                if (ontl.Tot > DateTime.Now)
                {
                    btnAnnuleren.IsEnabled = true;
                }
                else
                {
                    btnAnnuleren.IsEnabled = false;
                }
            }
        }

        private void BtnAccepteren_Click(object sender, RoutedEventArgs e)
        {
            if (AanvragenListBox.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontl)
            {
                if (ontl.OntleningStatus == Ontlening.Status.InAanvraag)
                {
                    ontl.OntleningStatus = Ontlening.Status.Goedgekeurd;
                    Ontlening.UpdateOntleningStatus(ontl.Id, ontl.OntleningStatus);

                    AanvragenListBox.Items.Remove(AanvragenListBox.SelectedItem);
                    LaadOntleningen();
                    LoadAanvragen();
                }
            }
        }

        private void BtnAfwijzen_Click(object sender, RoutedEventArgs e)
        {
            if (AanvragenListBox.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontl)
            {
                if (ontl.OntleningStatus == Ontlening.Status.InAanvraag)
                {
                    ontl.OntleningStatus = Ontlening.Status.Verworpen;
                    Ontlening.UpdateOntleningStatus(ontl.Id, ontl.OntleningStatus);

                    // Verwijder het geselecteerde item uit de ListBox
                    AanvragenListBox.Items.Remove(AanvragenListBox.SelectedItem);

                    LaadOntleningen();
                }
            }
        }

    }
}
