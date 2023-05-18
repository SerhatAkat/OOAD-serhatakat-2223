using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public class Ontlening
    {
        public int Id { get; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public int Voertuig_Id { get; }
        public int Aanvrager_Id { get; }

        public enum Status
        {
            InAanvraag,
            Goedgekeurd,
            Verworpen
        }

        public static List<string> GetOntleningen(int gebruikerId)
        {
            List<string> ontleningen = new List<string>();

            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("SELECT Voertuig.naam, Ontlening.Vanaf, Ontlening.Tot FROM Ontlening INNER JOIN Voertuig ON Ontlening.Voertuig_Id = Voertuig.Id WHERE Ontlening.Aanvrager_Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", gebruikerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ontleningen.Add($"{reader.GetString(0)} - {reader.GetDateTime(1).ToString("dd/MM/yyyy HH:mm")} tot {reader.GetDateTime(2).ToString("dd/MM/yyyy HH:mm")}");

                        }
                    }
                }
            }
            return ontleningen;
        }
    }
}
