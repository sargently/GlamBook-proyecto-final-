namespace GlamBook.Entities
{
    // Los estados posibles de una cita
    public enum EstadoCita { Pendiente, Confirmada, Cancelada, Completada }

    // La cita conecta una clienta con un servicio en una fecha y hora específica.
    // Usa ServicioBase en vez de un tipo específico — eso es polimorfismo:
    // puede ser ServicioMaquillaje o ServicioNovias sin importar cuál.
    public class Cita
    {
        // ID que viene de la base de datos
        public int CitaID { get; set; }

        // La clienta que agendó la cita
        public Clienta Clienta { get; set; }

        // El servicio solicitado (puede ser cualquier tipo que herede de ServicioBase)
        public ServicioBase Servicio { get; set; }

        // Fecha y hora de la cita
        public DateTime FechaHora { get; set; }

        // Estado actual de la cita
        public EstadoCita Estado { get; set; }

        // Notas adicionales (opcional)
        public string Notas { get; set; }

        // Constructor para agendar una cita nueva
        // La clienta y el servicio son obligatorios, las notas son opcionales
        public Cita(Clienta clienta, ServicioBase servicio, DateTime fechaHora, string notas = "")
        {
            // Si no se recibe clienta o servicio, lanza un error de inmediato
            Clienta = clienta ?? throw new ArgumentNullException(nameof(clienta));
            Servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            FechaHora = fechaHora;
            Estado = EstadoCita.Pendiente; // toda cita empieza como pendiente
            Notas = notas;
        }

        // Constructor para cuando traigo una cita de la base de datos
        // Reutiliza el constructor de arriba con ": this(...)"
        public Cita(int id, Clienta clienta, ServicioBase servicio,
                    DateTime fechaHora, EstadoCita estado, string notas)
            : this(clienta, servicio, fechaHora, notas)
        {
            CitaID = id;
            Estado = estado;
        }

        // Cancela la cita — no se puede cancelar si ya está completada
        public void Cancelar()
        {
            if (Estado == EstadoCita.Completada)
                throw new InvalidOperationException("No se puede cancelar una cita ya completada.");
            Estado = EstadoCita.Cancelada;
        }

        // Reprograma la cita a una nueva fecha
        // No se puede reprogramar si ya fue cancelada o completada
        public void Reprogramar(DateTime nuevaFecha)
        {
            if (Estado == EstadoCita.Cancelada || Estado == EstadoCita.Completada)
                throw new InvalidOperationException("No se puede reprogramar esta cita.");
            FechaHora = nuevaFecha;
            Estado = EstadoCita.Pendiente;
        }

        // Muestra el resumen completo de la cita
        public override string ToString()
        {
            return $"[Cita #{CitaID}] {Clienta.NombreCompleto} | {Servicio.Nombre} | " +
                   $"{FechaHora:dd/MM/yyyy HH:mm} | Estado: {Estado} | RD$ {Servicio.Calcular():N2}";
        }
    }
}