using GlamBook.Entities;
using Microsoft.Data.SqlClient;

namespace GlamBook.DAL
{
    // Capa de acceso a datos para citas.
    // Maneja todas las operaciones con la tabla de citas en SQL Server.
    //
    // Stored procedures requeridos en la BD:
    //   sp_ObtenerCitas         → SELECT con JOIN a Clientas y Servicios
    //   sp_InsertarCita         → INSERT + devuelve SCOPE_IDENTITY()
    //   sp_ActualizarEstadoCita → UPDATE EstadoCita WHERE CitaID = @CitaID
    //   sp_EliminarCita         → DELETE WHERE CitaID = @CitaID
    public class CitaDAL
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor: recibe la cadena de conexión desde afuera
        public CitaDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Guarda una cita nueva en la base de datos
        // Devuelve el ID que generó SQL Server automáticamente
        public int Guardar(Cita cita)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_InsertarCita", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ClientaID", cita.Clienta.ClientaID);
            cmd.Parameters.AddWithValue("@ServicioID", cita.Servicio.ServicioID);
            cmd.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
            cmd.Parameters.AddWithValue("@Estado", cita.Estado.ToString());
            cmd.Parameters.AddWithValue("@Notas", cita.Notas ?? string.Empty);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Actualiza el estado de una cita (ej: Pendiente → Confirmada, Cancelada...)
        public void ActualizarEstado(int citaID, EstadoCita nuevoEstado)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_ActualizarEstadoCita", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CitaID", citaID);
            cmd.Parameters.AddWithValue("@Estado", nuevoEstado.ToString());
            cmd.ExecuteNonQuery();
        }

        // Actualiza la fecha/hora de una cita reprogramada
        public void ActualizarFecha(int citaID, DateTime nuevaFecha)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_ReprogramarCita", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CitaID", citaID);
            cmd.Parameters.AddWithValue("@FechaHora", nuevaFecha);
            cmd.ExecuteNonQuery();
        }

        // Elimina una cita de la base de datos por su ID
        public void Eliminar(int citaID)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            using var cmd = new SqlCommand("sp_EliminarCita", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CitaID", citaID);
            cmd.ExecuteNonQuery();
        }
    }
}
