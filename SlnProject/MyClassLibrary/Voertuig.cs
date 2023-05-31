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
        public static void DeleteVoertuig(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Start een nieuwe transactie
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string deleteFoto = "DELETE FROM [Foto] WHERE voertuig_id = @Id";
                    SqlCommand deleteFotoCmd = new SqlCommand(deleteFoto, conn);
                    deleteFotoCmd.Transaction = transaction;
                    deleteFotoCmd.Parameters.AddWithValue("@Id", id);
                    deleteFotoCmd.ExecuteNonQuery();

                    string deleteOntlening = "DELETE FROM [Ontlening] WHERE voertuig_id = @Id";
                    SqlCommand deleteOntleningCmd = new SqlCommand(deleteOntlening, conn);
                    deleteOntleningCmd.Transaction = transaction;
                    deleteOntleningCmd.Parameters.AddWithValue("@Id", id);
                    deleteOntleningCmd.ExecuteNonQuery();

                    // Delete the record from the Voertuig table
                    string deleteVoertuig = "DELETE FROM [Voertuig] WHERE id = @Id";
                    SqlCommand deleteVoertuigCmd = new SqlCommand(deleteVoertuig, conn);
                    deleteVoertuigCmd.Transaction = transaction;
                    deleteVoertuigCmd.Parameters.AddWithValue("@Id", id);
                    deleteVoertuigCmd.ExecuteNonQuery();

                    // Als alles succesvol is, commit de transactie
                    transaction.Commit();
                }
                catch
                {
                    // Als er iets misgaat, rol dan de transactie terug
                    transaction.Rollback();
                    throw;  // Herstel de originele uitzondering zodat deze kan worden afgehandeld door de oproepende code
                }
            }
        }

        public void ToevoegenGemotoriseerdVoertuig(Voertuig voertuig, int userId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Voertuig (naam, eigenaar_id, merk, model, bouwjaar, beschrijving, type, transmissie, brandstof) " +
                             "VALUES (@Naam, @EigenaarId, @Merk, @Model, @Bouwjaar, @Beschrijving, @Type, @Transmissie, @Brandstof)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Naam", voertuig.Naam);
                    command.Parameters.AddWithValue("@EigenaarId", userId);
                    command.Parameters.AddWithValue("@Merk", voertuig.Merk ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Model", voertuig.Model ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Bouwjaar", voertuig.Bouwjaar);
                    command.Parameters.AddWithValue("@Beschrijving", voertuig.Beschrijving);
                    command.Parameters.AddWithValue("@Type", 1);
                    command.Parameters.AddWithValue("@Transmissie", voertuig.TransmissieType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Brandstof", voertuig.BrandstofType ?? (object)DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        public int ToevoegenGetrokkenVoertuig(Voertuig voertuig, int userId)
        {
            int voertuigId = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Voertuig (eigenaar_id, type, geremd, afmetingen, maxbelasting, gewicht, model, merk, bouwjaar, beschrijving, naam) " +
                             "OUTPUT inserted.id " +
                             "VALUES (@EigenaarId, @Type, @Geremd, @Afmetingen, @MaxBelasting, @Gewicht, @Model, @Merk, @Bouwjaar, @Beschrijving, @Naam)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EigenaarId", userId);
                    command.Parameters.AddWithValue("@Type", 2);
                    command.Parameters.AddWithValue("@Geremd", voertuig.Geremd ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Afmetingen", voertuig.Afmetingen ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MaxBelasting", voertuig.MaxBelasting ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Gewicht", voertuig.Gewicht ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Model", voertuig.Model);
                    command.Parameters.AddWithValue("@Merk", voertuig.Merk);
                    command.Parameters.AddWithValue("@Bouwjaar", voertuig.Bouwjaar);
                    command.Parameters.AddWithValue("@Beschrijving", voertuig.Beschrijving);
                    command.Parameters.AddWithValue("@Naam", voertuig.Naam);
                    voertuigId = (int)command.ExecuteScalar();
                }
            }

            return voertuigId;
        }

        public void UpdateGemotoriseerd()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Voertuigen SET Naam = @Naam, Merk = @Merk, Model = @Model, Beschrijving = @Beschrijving, Bouwjaar = @Bouwjaar, TransmissieType = @TransmissieType, BrandstofType = @BrandstofType WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Naam", this.Naam);
                    cmd.Parameters.AddWithValue("@Merk", this.Merk);
                    cmd.Parameters.AddWithValue("@Model", this.Model);
                    cmd.Parameters.AddWithValue("@Beschrijving", this.Beschrijving);
                    cmd.Parameters.AddWithValue("@Bouwjaar", this.Bouwjaar);
                    cmd.Parameters.AddWithValue("@TransmissieType", this.TransmissieType);
                    cmd.Parameters.AddWithValue("@BrandstofType", this.BrandstofType);
                    cmd.Parameters.AddWithValue("@ID", this.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateGetrokken()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Voertuigen SET Naam = @Naam, Beschrijving = @Beschrijving, Bouwjaar = @Bouwjaar, Merk = @Merk, Model = @Model, Gewicht = @Gewicht, MaxBelasting = @MaxBelasting, Afmetingen = @Afmetingen, Geremd = @Geremd WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Naam", this.Naam);
                    cmd.Parameters.AddWithValue("@Beschrijving", this.Beschrijving);
                    cmd.Parameters.AddWithValue("@Bouwjaar", this.Bouwjaar);
                    cmd.Parameters.AddWithValue("@Merk", this.Merk);
                    cmd.Parameters.AddWithValue("@Model", this.Model);
                    cmd.Parameters.AddWithValue("@Gewicht", this.Gewicht);
                    cmd.Parameters.AddWithValue("@MaxBelasting", this.MaxBelasting);
                    cmd.Parameters.AddWithValue("@Afmetingen", this.Afmetingen);
                    cmd.Parameters.AddWithValue("@Geremd", this.Geremd);
                    cmd.Parameters.AddWithValue("@ID", this.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
