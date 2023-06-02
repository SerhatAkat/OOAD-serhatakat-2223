using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace MyClassLibrary
{
    public class Foto
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int Voertuig_Id { get; set; }

        // één foto voor HomePage
        public static Foto GetFotoForVoertuig(int voertuigId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Foto] WHERE Voertuig_Id = @voertuigId", conn))
                {
                    cmd.Parameters.AddWithValue("@voertuigId", voertuigId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Foto foto = new Foto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Image = (byte[])reader["data"],
                                Voertuig_Id = reader.GetInt32(reader.GetOrdinal("Voertuig_Id"))
                            };

                            return foto;
                        }
                    }
                }
            }

            return null;
        }

        // meerdere foto's voor InfoPage
        public static List<Foto> GetFotosForVoertuig(int voertuigId)
        {
            List<Foto> fotos = new List<Foto>();

            string connectionString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Foto] WHERE Voertuig_Id = @voertuigId", conn))
                {
                    cmd.Parameters.AddWithValue("@voertuigId", voertuigId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Foto foto = new Foto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Image = (byte[])reader["data"],
                                Voertuig_Id = reader.GetInt32(reader.GetOrdinal("Voertuig_Id"))
                            };
                            fotos.Add(foto);
                        }
                    }
                }
            }
            return fotos;
        }

        public static void AddFoto(byte[] dataImage, int voertuigId)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO [Foto] (data, voertuig_id) VALUES (@Data, @Voertuig_Id)", conn);
                cmd.Parameters.AddWithValue("@data", dataImage);
                cmd.Parameters.AddWithValue("@Voertuig_Id", voertuigId);
                cmd.ExecuteNonQuery();
            }
        }

        public static void VerwijderFotoByFotoId(int fotoId)
        {
            string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand("DELETE FROM Foto WHERE Id = @Id", conn);
                command.Parameters.AddWithValue("@Id", fotoId);

                command.ExecuteNonQuery();
            }
        }
        public static void EditFoto(int fotoId, byte[] dataImage)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connStr"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE [Foto] SET data = @Data WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Data", dataImage);
                cmd.Parameters.AddWithValue("@Id", fotoId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
