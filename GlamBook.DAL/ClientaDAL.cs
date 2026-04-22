using GlamBook.Entities;
using Microsoft.Data.SqlClient;

namespace GlamBook.DAL
{
    // Esta es la capa de acceso a datos.
    // Su único trabajo es hablar con SQL Server.
    // solo consultas.
    public class ClientaDAL
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor: recibe la cadena de conexión desde afuera
        public ClientaDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Trae todas las clientas de la base de datos
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
                    nombre: reader["Nombre"].ToString() ?? "",
                    apellido: reader["Apellido"].ToString() ?? "",
                    telefono: reader["Telefono"].ToString() ?? "",
                    correo: reader["Correo"].ToString() ?? "",
                    fechaRegistro: (DateTime)reader["FechaRegistro"]
                ));
            }

            return lista;
        }

        // Guarda una clienta nueva en la base de datos
        // Devuelve el ID que generó SQL Server automáticamente
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

            object? result = cmd.ExecuteScalar();
            if (result is null || result == DBNull.Value)
                throw new InvalidOperationException("No se pudo obtener el ID generado al insertar la clienta.");

            return Convert.ToInt32(result);
        }

        // Elimina una clienta de la base de datos por su ID
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