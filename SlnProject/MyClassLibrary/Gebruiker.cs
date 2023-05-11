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
        public string Profielfoto { get; set; }
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

        public static Gebruiker GetGebruiker(string email, string paswoord)
        {
            string gehashtPaswoord = ComputeSha256Hash(paswoord);
            string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BuurlenenDB;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Gebruiker] WHERE Email = @email AND Paswoord = @paswoord", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@paswoord", paswoord);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Gebruiker gebruiker = new Gebruiker
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Voornaam = reader.GetString(reader.GetOrdinal("Voornaam")),
                                Achternaam = reader.GetString(reader.GetOrdinal("Achternaam")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Paswoord = reader.GetString(reader.GetOrdinal("Paswoord")),
                                Aanmaakdatum = reader.GetDateTime(reader.GetOrdinal("Aanmaakdatum")),
                                Profielfoto = reader.IsDBNull(reader.GetOrdinal("Profielfoto")) ? null : reader.GetString(reader.GetOrdinal("Profielfoto")),
                                Geslacht = (GeslachtType)reader.GetByte(reader.GetOrdinal("Geslacht"))
                            };

                            return gebruiker;
                        }
                    }
                }
            }

            return null;
        }


    }
}
