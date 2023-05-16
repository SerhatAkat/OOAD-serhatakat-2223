using System;
using System.Windows;
using System.Windows.Controls;

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

            MyClassLibrary.Gebruiker gebruiker = MyClassLibrary.Gebruiker.GetGebruiker(email, paswoord);

            if (gebruiker != null)
            {
                MainWindow mainWindow = new MainWindow(gebruiker); // Ervan uitgaande dat uw MainWindow een Gebruiker als parameter accepteert
                mainWindow.Show();

                this.Close(); // Sluit het loginvenster
            }
            else if (gebruiker == null)
            {
                MessageBox.Show("Ongeldige inloggegevens. Probeer het opnieuw.");
                txtPaswoord.Password = "";
            }
        }

    }
}
