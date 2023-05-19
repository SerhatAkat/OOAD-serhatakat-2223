using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public class Ontlening
    {
        public int Id { get; set; }
        public DateTime Vanaf { get; set; }
        public DateTime Tot { get; set; }
        public string Bericht { get; set; }
        public string VoertuigNaam { get; set; }  // Nieuwe veld voor de naam van het voertuig
        public int Aanvrager_Id { get; set; }
        public Status OntleningStatus { get; set; }

        public enum Status
        {
            InAanvraag = 1,
            Goedgekeurd = 2,
            Verworpen = 3
        }

        public static List<Ontlening> GetOntleningen(int gebruikerId)
        {
            List<Ontlening> ontleningen = new List<Ontlening>();

            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("SELECT Voertuig.naam, Ontlening.Vanaf, Ontlening.Tot, Ontlening.Status FROM Ontlening INNER JOIN Voertuig ON Ontlening.Voertuig_Id = Voertuig.Id WHERE Ontlening.Aanvrager_Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", gebruikerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ontlening ontl = new Ontlening
                            {
                                VoertuigNaam = reader.GetString(reader.GetOrdinal("naam")),
                                Vanaf = reader.GetDateTime(reader.GetOrdinal("Vanaf")),
                                Tot = reader.GetDateTime(reader.GetOrdinal("Tot")),
                                OntleningStatus = (Status)reader.GetByte(reader.GetOrdinal("Status"))
                            };
                            ontleningen.Add(ontl);
                        }
                    }
                }
            }
            return ontleningen;
        }

        public override string ToString()
        {
            return $"{VoertuigNaam} - {Vanaf.ToString("dd/MM/yyyy HH:mm")} tot {Tot.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
