using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyClassLibrary.Voertuig;

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
            Manueel = 1,
            Automatisch
        }
        public enum Brandstof
        {
            Benzine = 1,
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
                                Beschrijving = reader.GetString(reader.GetOrdinal("Beschrijving")),
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

        public static List<Voertuig> GetVoertuigenVanAnderen(int ingelogdeGebruikerId)
        {
            List<Voertuig> voertuigen = new List<Voertuig>();

            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [Voertuig] WHERE eigenaar_id <> @UserId", conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", ingelogdeGebruikerId));

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Voertuig voertuig = new Voertuig
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Naam = reader.GetString(reader.GetOrdinal("Naam")),
                                Beschrijving = reader.GetString(reader.GetOrdinal("Beschrijving")),
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

        public static List<Voertuig> GetVoertuigenByGebruikerId(int id)
        {
            List<Voertuig> voertuigen = new List<Voertuig>();
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand("SELECT * FROM Voertuig WHERE eigenaar_id = @id", conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Voertuig voertuig = new Voertuig();
                            voertuig.Id = Convert.ToInt32(reader["id"]);
                            voertuig.Naam = Convert.ToString(reader["naam"]);
                            voertuig.Beschrijving = Convert.ToString(reader["beschrijving"]);
                            voertuig.Bouwjaar = Convert.ToInt32(reader["bouwjaar"]);
                            voertuig.Merk = reader["merk"] == DBNull.Value ? null : Convert.ToString(reader["merk"]);
                            voertuig.Model = reader["model"] == DBNull.Value ? null : Convert.ToString(reader["model"]);
                            voertuig.Type = Convert.ToInt32(reader["type"]);
                            voertuig.TransmissieType = reader["transmissie"] == DBNull.Value ? null : (Transmissie?)Convert.ToInt32(reader["transmissie"]);
                            voertuig.BrandstofType = reader["brandstof"] == DBNull.Value ? null : (Brandstof?)Convert.ToInt32(reader["brandstof"]);
                            voertuig.Gewicht = reader["gewicht"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["gewicht"]);
                            voertuig.MaxBelasting = reader["maxbelasting"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["maxbelasting"]);
                            voertuig.Afmetingen = Convert.ToString(reader["afmetingen"]);
                            voertuig.Geremd = reader["geremd"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(reader["geremd"]);
                            voertuig.Eigenaar = Convert.ToInt32(reader["eigenaar_id"]);
                            voertuigen.Add(voertuig);
                        }
                    }
                }
            }
            return voertuigen;
        }

        public static List<Voertuig> GetAllVoertuigenOwnedByGebruiker(int userId)
        {
            List<Voertuig> voertuigen = new List<Voertuig>();
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM [Voertuig] WHERE eigenaar_id = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Voertuig voertuig = new Voertuig();
                    voertuig.Id = Convert.ToInt32(reader["id"]);
                    voertuig.Naam = Convert.ToString(reader["naam"]);
                    voertuig.Beschrijving = Convert.ToString(reader["beschrijving"]);
                    voertuig.Bouwjaar = Convert.ToInt32(reader["bouwjaar"]);
                    voertuig.Merk = Convert.ToString(reader["merk"]);
                    voertuig.Model = Convert.ToString(reader["model"]);
                    voertuig.Type = Convert.ToInt32(reader["type"]);
                    voertuig.Eigenaar = Convert.ToInt32(reader["eigenaar_id"]);
                    voertuig.TransmissieType = reader["transmissie"] == DBNull.Value ? null : (Transmissie?)(int)reader["transmissie"];
                    voertuig.BrandstofType = reader["brandstof"] == DBNull.Value ? null : (Brandstof?)(int)reader["brandstof"];
                    voertuig.Gewicht = reader["gewicht"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["gewicht"]);
                    voertuig.MaxBelasting = reader["maxbelasting"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["maxbelasting"]);
                    voertuig.Afmetingen = Convert.ToString(reader["afmetingen"]);
                    voertuig.Geremd = reader["geremd"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(reader["geremd"]);

                    voertuigen.Add(voertuig);
                }
            }
            return voertuigen;
        }
        public static bool DeleteVoertuig(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Voertuig WHERE id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
