using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace MyClassLibrary
{
    public class Gebruiker
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public string Paswoord { get; set; }
        public DateTime Aanmaakdatum { get; set; }
        public GeslachtType Geslacht { get; set; }

        public enum GeslachtType
        {
            Man,
            Vrouw
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public static Gebruiker GetGebruiker(string login, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM [GEBRUIKER] WHERE email = @Email", connection);
                command.Parameters.AddWithValue("@Email", login);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Gebruiker gebruiker = new Gebruiker();
                    gebruiker.Id = (int)reader["Id"];
                    gebruiker.Voornaam = (string)reader["voornaam"];
                    gebruiker.Achternaam = (string)reader["achternaam"];
                    string storedPassword = (string)reader["paswoord"];

                    string hashedInputPassword = ToSha256(password);
                    if (hashedInputPassword == storedPassword)
                    {

                        return gebruiker;
                    }

                }
                return null;
            }
        }
        public static string GetGebruikerNaamById(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Voornaam, Achternaam FROM [dbo].[Gebruiker] WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return $"{reader.GetString(reader.GetOrdinal("Voornaam"))} {reader.GetString(reader.GetOrdinal("Achternaam"))}";
                        }
                    }
                }
            }
            return null;
        }

        public static Gebruiker GetGebruikerById(int gebruikerId)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();

                string query = "SELECT * FROM Gebruiker WHERE Id = @GebruikerId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@GebruikerId", gebruikerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Gebruiker gebruiker = new Gebruiker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Voornaam = reader.GetString(reader.GetOrdinal("Voornaam")),
                            Achternaam = reader.GetString(reader.GetOrdinal("Achternaam"))
                        };

                        return gebruiker;
                    }
                }
            }
            return null;
        }

        public static string ToSha256(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                byte[] hashedPasswordBytes = sha256.ComputeHash(inputBytes);
                string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }
    }
}