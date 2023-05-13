using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class Foto
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int Voertuig_Id { get; set; }

        public static Foto GetFotoForVoertuig(int voertuigId)
        {
            string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BuurlenenDB;Integrated Security=True";
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
    }
}
