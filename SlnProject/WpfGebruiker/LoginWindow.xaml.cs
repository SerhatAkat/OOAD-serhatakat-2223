using System;
using System.Data.SqlClient;
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

            try
            {
                Gebruiker gebruiker = Gebruiker.GetGebruiker(email, paswoord);

                if (gebruiker != null)
                {
                    MainWindow mainWindow = new MainWindow(gebruiker);
                    mainWindow.Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ongeldige inloggegevens. Probeer het opnieuw.");
                    txtPaswoord.Password = "";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Er is een fout opgetreden tijdens het verbinden met de database: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er is een onverwachte fout opgetreden: " + ex.Message);
            }
        }
    }
}
