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
        public int Voertuig_Id { get; set; }
        public string VoertuigNaam { get; set; }
        public int Aanvrager_Id { get; set; }
        public Status OntleningStatus { get; set; }
        public Gebruiker Aanvrager { get; set; }

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

                using (SqlCommand command = new SqlCommand("SELECT Voertuig.naam, Ontlening.Id, Ontlening.Vanaf, Ontlening.Tot, Ontlening.Status FROM Ontlening INNER JOIN Voertuig ON Ontlening.Voertuig_Id = Voertuig.Id WHERE Ontlening.Aanvrager_Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", gebruikerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ontlening ontl = new Ontlening
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
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

        public static void VerwijderOntlening(int ontleningId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM Ontlening WHERE Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", ontleningId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void VoegOntleningToe(Ontlening nieuweOntlening)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Ontlening (voertuig_id, vanaf, tot, bericht, status, aanvrager_id) " +
               "VALUES (@VoertuigId, @Van, @Tot, @Bericht, @Status, @AanvragerID)";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@VoertuigId", nieuweOntlening.Id);
                    command.Parameters.AddWithValue("@Van", nieuweOntlening.Vanaf);
                    command.Parameters.AddWithValue("@Tot", nieuweOntlening.Tot);
                    command.Parameters.AddWithValue("@Bericht", nieuweOntlening.Bericht);
                    command.Parameters.AddWithValue("@Status", nieuweOntlening.OntleningStatus);
                    command.Parameters.AddWithValue("@AanvragerId", nieuweOntlening.Aanvrager.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Ontlening> GetAanvraagdeOntleningen(int aanvragingId)
        {
            List<Ontlening> aanvraagOntleningen = new List<Ontlening>();
            List<Ontlening> ontleningen = GetOntleningen(aanvragingId);

            foreach (Ontlening ontlening in ontleningen)
            {
                if (ontlening.OntleningStatus == Status.InAanvraag)
                {
                    aanvraagOntleningen.Add(ontlening);
                }
            }

            return aanvraagOntleningen;
        }

        public static void WijzigStatus(int ontleningId, Status nieuweStatus)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand("UPDATE Ontlening SET Status = @Status WHERE Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", ontleningId);
                    command.Parameters.AddWithValue("@Status", (byte)nieuweStatus);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateOntleningStatus(int ontleningId, Status nieuweStatus)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("UPDATE Ontlening SET Status = @nieuweStatus WHERE Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", ontleningId);
                    command.Parameters.AddWithValue("@nieuweStatus", nieuweStatus);

                    command.ExecuteNonQuery();
                }
            }
        }


        public override string ToString()
        {
            return $"{VoertuigNaam} - {Vanaf.ToString("dd/MM/yyyy HH:mm")} tot {Tot.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
