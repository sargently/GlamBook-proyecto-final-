namespace GlamBook.Entities
{
    public enum EstadoCita { Pendiente, Confirmada, Cancelada, Completada }

    public class Cita
    {
        public int CitaID { get; set; }
        public Clienta Clienta { get; set; }
        public ServicioBase Servicio { get; set; }
        public DateTime FechaHora { get; set; }
        public EstadoCita Estado { get; set; }
        public string Notas { get; set; }

        public Cita(Clienta clienta, ServicioBase servicio, DateTime fechaHora, string notas = "")
        {
            Clienta = clienta ?? throw new ArgumentNullException(nameof(clienta));
            Servicio = servicio ?? throw new ArgumentNullException(nameof(servicio));
            FechaHora = fechaHora;
            Estado = EstadoCita.Pendiente;
            Notas = notas;
        }

        public Cita(int id, Clienta clienta, ServicioBase servicio,
                    DateTime fechaHora, EstadoCita estado, string notas)
            : this(clienta, servicio, fechaHora, notas)
        {
            CitaID = id;
            Estado = estado;
        }

        public void Cancelar()
        {
            if (Estado == EstadoCita.Completada)
                throw new InvalidOperationException("No se puede cancelar una cita ya completada.");
            Estado = EstadoCita.Cancelada;
        }

        public void Reprogramar(DateTime nuevaFecha)
        {
            if (Estado == EstadoCita.Cancelada || Estado == EstadoCita.Completada)
                throw new InvalidOperationException("No se puede reprogramar esta cita.");
            FechaHora = nuevaFecha;
            Estado = EstadoCita.Pendiente;
        }

        public override string ToString()
        {
            return $"[Cita #{CitaID}] {Clienta.NombreCompleto} | {Servicio.Nombre} | " +
                   $"{FechaHora:dd/MM/yyyy HH:mm} | Estado: {Estado} | RD$ {Servicio.Calcular():N2}";
        }
    }
}