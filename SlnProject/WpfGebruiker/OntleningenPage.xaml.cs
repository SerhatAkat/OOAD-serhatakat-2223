﻿using System;
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
            List<string> mijnOntleningen = Ontlening.GetOntleningen(gebruikerId);
            foreach (string ontl in mijnOntleningen)
            {
                MijnOntleningenListBox.Items.Add(ontl);
            }
        }
    }
}
