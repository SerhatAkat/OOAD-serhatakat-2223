using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for OntleningenPage.xaml
    /// </summary>
    public partial class OntleningenPage : Page
    {
        private List<Ontlening> aanvragen;
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
            try
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
            catch (SqlException ex)
            {
                MessageBox.Show("Databasefout bij het annuleren van de ontlending: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden bij het annuleren van de ontlending: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LaadOntleningen()
        {
            MijnOntleningenListBox.Items.Clear();
            List<Ontlening> mijnOntleningen;
            try
            {
                mijnOntleningen = Ontlening.GetOntleningen(gebruikerId);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Databasefout bij het ophalen van de ontleningen: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden bij het ophalen van de ontleningen: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
            List<Ontlening> mijnOntleningen = new List<Ontlening>();
            foreach (Voertuig voertuig in Voertuig.GetVoertuigenByGebruikerId(gebruikerId))
            {
                mijnOntleningen.AddRange(Ontlening.GetAlleOntleningByVoertuigId(voertuig.Id));
            }

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
                if (ontl.Aanvrager_Id != gebruikerId && ontl.OntleningStatus == Ontlening.Status.InAanvraag)
                {
                    ListBoxItem item = new ListBoxItem();
                    TextBlock tb = new TextBlock();
                    item.Content = tb;
                    item.Tag = ontl;
                    AanvragenListBox.Items.Add(item);
                    tb.Foreground = Brushes.Orange;
                    Gebruiker ontleningAanvrager = Gebruiker.GetGebruikerById(ontl.Aanvrager_Id);
                    string gebruikerNaam = $"{ontleningAanvrager.Voornaam.Substring(0, 1)}.{ontleningAanvrager.Achternaam}";
                    string displayText = $"{ontl.VoertuigNaam} - {ontl.Vanaf.ToString("yyyy-MM-dd 00:00")} - {ontl.Tot.ToString("yyyy-MM-dd 00:00")} door {gebruikerNaam}";
                    tb.Text = displayText.ToString();
                }
            }
        }

        private void AanvragenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AanvragenListBox.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontlening)
            {
                // Gebruik ontlening.Aanvrager_Id in plaats van gebruikerId
                Gebruiker aanvraag = Gebruiker.GetGebruikerById(ontlening.Aanvrager_Id);
                string gebruikerNaam = $"{aanvraag.Voornaam} {aanvraag.Achternaam}";
                lblVoertuig.Content = $"Voertuig: {ontlening.VoertuigNaam}";
                lblPeriode.Content = $"Periode: {ontlening.Vanaf.ToString("yyyy-MM-dd 00:00")} - {ontlening.Tot.ToString("yyyy-MM-dd 00:00")}";
                lblAanvrager.Content = $"Aanvrager: {gebruikerNaam}";
                lblBericht.Content = $"Bericht: {ontlening.Bericht}";
            }
            else
            {
                lblVoertuig.Content = "Voertuig:";
                lblPeriode.Content = "Periode:";
                lblAanvrager.Content = "Aanvrager:";
                lblBericht.Content = "Bericht:";
            }
        }

        private void BtnAccepteren_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AanvragenListBox.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontlening)
                {
                    ontlening.OntleningStatus = Ontlening.Status.Goedgekeurd;
                    Ontlening.WijzigStatus(ontlening);

                    AanvragenListBox.Items.Remove(item);
                    LoadAanvragen();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAfwijzen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AanvragenListBox.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontlening)
                {
                    ontlening.OntleningStatus = Ontlening.Status.Verworpen;
                    Ontlening.WijzigStatus(ontlening);

                    AanvragenListBox.Items.Remove(item);
                    LaadOntleningen();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een fout opgetreden: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}