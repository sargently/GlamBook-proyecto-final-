using GlamBook.Entities;
using Microsoft.Data.SqlClient;

namespace GlamBook.DAL
{
    // Capa de acceso a datos para empleadas.
    // Maneja todas las operaciones con la tabla de empleadas en SQL Server.
    //
    // Stored procedures requeridos en la BD:
    //   sp_ObtenerEmpleadas  → SELECT * FROM Empleadas
    //   sp_InsertarEmpleada  → INSERT + devuelve SCOPE_IDENTITY()
    //   sp_EliminarEmpleada  → DELETE WHERE EmpleadaID = @EmpleadaID
    //   sp_ActualizarEmpleada → UPDATE WHERE EmpleadaID = @EmpleadaID
    public class EmpleadaDAL
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor: recibe la cadena de conexión desde afuera
        public EmpleadaDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Trae todas las empleadas de la base de datos
        public List<Empleada> ObtenerTodas()
        {
            var lista = new List<Empleada>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_ObtenerEmpleadas", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Empleada(
                    id: (int)reader["EmpleadaID"],
                    nombre: reader["Nombre"]?.ToString() ?? string.Empty,
                    apellido: reader["Apellido"]?.ToString() ?? string.Empty,
                    telefono: reader["Telefono"]?.ToString() ?? string.Empty,
                    especialidad: reader["Especialidad"]?.ToString() ?? string.Empty
                ));
            }
            return lista;
        }

        // Guarda una empleada nueva en la base de datos
        // Devuelve el ID que generó SQL Server automáticamente
        public int Guardar(Empleada empleada)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_InsertarEmpleada", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Nombre", empleada.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", empleada.Apellido);
            cmd.Parameters.AddWithValue("@Telefono", empleada.Telefono);
            cmd.Parameters.AddWithValue("@Especialidad", empleada.Especialidad);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Actualiza los datos de una empleada existente
        public void Actualizar(Empleada empleada)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_ActualizarEmpleada", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmpleadaID", empleada.EmpleadaID);
            cmd.Parameters.AddWithValue("@Nombre", empleada.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", empleada.Apellido);
            cmd.Parameters.AddWithValue("@Telefono", empleada.Telefono);
            cmd.Parameters.AddWithValue("@Especialidad", empleada.Especialidad);
            cmd.ExecuteNonQuery();
        }

        // Elimina una empleada de la base de datos por su ID
        public void Eliminar(int empleadaID)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_EliminarEmpleada", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpleadaID", empleadaID);
            cmd.ExecuteNonQuery();
        }
    }
}
