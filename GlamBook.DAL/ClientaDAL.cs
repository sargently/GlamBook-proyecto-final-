using GlamBook.Entities;
using Microsoft.Data.SqlClient;

namespace GlamBook.DAL
{
    public class ClientaDAL
    {
        private readonly string _connectionString;

        public ClientaDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Clienta> ObtenerTodas()
        {
            var lista = new List<Clienta>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_ObtenerClientas", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Clienta(
                    id: (int)reader["ClientaID"],
                    nombre: reader["Nombre"].ToString(),
                    apellido: reader["Apellido"].ToString(),
                    telefono: reader["Telefono"].ToString(),
                    correo: reader["Correo"].ToString(),
                    fechaRegistro: (DateTime)reader["FechaRegistro"]
                ));
            }
            return lista;
        }

        public int Guardar(Clienta clienta)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_InsertarClienta", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Nombre", clienta.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", clienta.Apellido);
            cmd.Parameters.AddWithValue("@Telefono", clienta.Telefono);
            cmd.Parameters.AddWithValue("@Correo", clienta.Correo);

            return (int)(decimal)cmd.ExecuteScalar();
        }

        public void Eliminar(int clientaID)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_EliminarClienta", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientaID", clientaID);
            cmd.ExecuteNonQuery();
        }
    }
}   