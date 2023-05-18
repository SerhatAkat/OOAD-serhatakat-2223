using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Voertuig
    {
        // gemeenschappelijk
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public int Bouwjaar { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public int Type { get; set; }
        public int Eigenaar { get; set; }

        // motorvoertuig
        public enum Transmissie
        {
            Manueel,
            Automatisch
        }
        public enum Brandstof
        {
            Benzine,
            Diesel,
            LPG
        }

        public Transmissie? TransmissieType { get; set; }
        public Brandstof? BrandstofType { get; set; }



        // getrokken voertuig
        public int? Gewicht { get; set; }
        public int? MaxBelasting { get; set; }
        public string Afmetingen { get; set; }
        public bool? Geremd { get; set; }

        public static List<Voertuig> GetAllVoertuigen()
        {
            List<Voertuig> voertuigen = new List<Voertuig>();

            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Voertuig]", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Voertuig voertuig = new Voertuig
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Naam = reader.GetString(reader.GetOrdinal("Naam")),
                                Beschrijving = reader.IsDBNull(reader.GetOrdinal("Beschrijving")) ? null : reader.GetString(reader.GetOrdinal("Beschrijving")),
                                Bouwjaar = reader.IsDBNull(reader.GetOrdinal("Bouwjaar")) ? 0 : reader.GetInt32(reader.GetOrdinal("Bouwjaar")),
                                Merk = reader.IsDBNull(reader.GetOrdinal("Merk")) ? null : reader.GetString(reader.GetOrdinal("Merk")),
                                Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader.GetString(reader.GetOrdinal("Model")),
                                Type = reader.GetInt32(reader.GetOrdinal("type")),
                                Eigenaar = reader.GetInt32(reader.GetOrdinal("eigenaar_id")),
                                TransmissieType = reader.IsDBNull(reader.GetOrdinal("Transmissie")) ? (Transmissie?)null : (Transmissie)reader.GetInt32(reader.GetOrdinal("Transmissie")),
                                BrandstofType = reader.IsDBNull(reader.GetOrdinal("Brandstof")) ? (Brandstof?)null : (Brandstof)reader.GetInt32(reader.GetOrdinal("Brandstof")),
                                Gewicht = reader.IsDBNull(reader.GetOrdinal("Gewicht")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Gewicht")),
                                MaxBelasting = reader.IsDBNull(reader.GetOrdinal("MaxBelasting")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("MaxBelasting")),
                                Afmetingen = reader.IsDBNull(reader.GetOrdinal("Afmetingen")) ? null : reader.GetString(reader.GetOrdinal("Afmetingen")),
                                Geremd = reader.IsDBNull(reader.GetOrdinal("Geremd")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("Geremd"))
                            };

                            voertuigen.Add(voertuig);
                        }
                    }
                }
            }

            return voertuigen;
        }

    }
}
