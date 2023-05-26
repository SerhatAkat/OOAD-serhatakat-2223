using System;
using System.Windows;
using System.Windows.Controls;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            txtEmail.TextChanged += TxtEmail_TextChanged;
            txtPaswoord.PasswordChanged += TxtPaswoord_PasswordChanged;
            txtEmail.Text = "teo@cmb.be";
            txtPaswoord.Password = "test345";
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblEmail.Content = string.Empty;
            }
        }
        private void TxtPaswoord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPaswoord.Password))
            {
                lblPaswoord.Content = string.Empty;
            }
        }

        private void Inloggen_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string paswoord = txtPaswoord.Password;

            bool isEmailEmpty = string.IsNullOrWhiteSpace(email);
            bool isPasswordEmpty = string.IsNullOrWhiteSpace(paswoord);

            if (isEmailEmpty)
            {
                lblEmail.Content = "Gelieve een email in te vullen";
            }

            if (isPasswordEmpty)
            {
                lblPaswoord.Content = "Gelieve een paswoord in te vullen";
            }

            if (isEmailEmpty || isPasswordEmpty)
            {
                return;
            }

            Gebruiker gebruiker = Gebruiker.GetGebruiker(email, paswoord);

            if (gebruiker != null)
            {
                MainWindow mainWindow = new MainWindow(gebruiker);
                mainWindow.Show();

                this.Close();
            }
            else if (gebruiker == null)
            {
                MessageBox.Show("Ongeldige inloggegevens. Probeer het opnieuw.");
                txtPaswoord.Password = "";
            }
        }
    }
}
