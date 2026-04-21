using GlamBook.Entities;

namespace GlamBook.BLL
{
    // Esta es la capa de lógica de negocio.
    // Aquí van las reglas del sistema.
    // Su único trabajo es validar y coordinar las operaciones.
    public class CitaBLL
    {
        // Lista donde se guardan las citas en memoria
        private readonly List<Cita> _citas = new();

        // Contador para asignar IDs a las citas nuevas
        private int _nextId = 1;

        // Agenda una cita nueva después de validar que el horario esté libre
        public Cita AgendarCita(Clienta clienta, ServicioBase servicio,
                                DateTime fechaHora, string notas = "")
        {
            // Primero verifico que no haya conflicto de horario
            ValidarDisponibilidad(fechaHora, servicio.DuracionMinutos);

            // Creo la cita y le asigno un ID
            var cita = new Cita(clienta, servicio, fechaHora, notas)
            {
                CitaID = _nextId++
            };

            // La agrego a la lista y la devuelvo
            _citas.Add(cita);
            return cita;
        }

        // Verifica que no haya dos citas al mismo tiempo
        // Si hay conflicto lanza un error con mensaje claro
        public void ValidarDisponibilidad(DateTime fechaHora, int duracionMinutos)
        {
            DateTime fin = fechaHora.AddMinutes(duracionMinutos);

            bool hayConflicto = _citas.Any(c =>
                c.Estado != EstadoCita.Cancelada &&
                fechaHora < c.FechaHora.AddMinutes(c.Servicio.DuracionMinutos) &&
                fin > c.FechaHora
            );

            if (hayConflicto)
                throw new InvalidOperationException(
                    "Ya existe una cita en ese horario. Por favor elige otra hora.");
        }

        // Cancela una cita buscándola por su ID
        public void CancelarCita(int citaID)
        {
            var cita = ObtenerPorID(citaID);
            cita.Cancelar();
        }

        // Reprograma una cita a una nueva fecha
        // Valida primero que el nuevo horario esté disponible
        public void ReprogramarCita(int citaID, DateTime nuevaFecha)
        {
            var cita = ObtenerPorID(citaID);
            ValidarDisponibilidad(nuevaFecha, cita.Servicio.DuracionMinutos);
            cita.Reprogramar(nuevaFecha);
        }

        // Devuelve todas las citas registradas
        public List<Cita> ObtenerTodas() => _citas.ToList();

        // Devuelve el historial de citas de una clienta específica
        // ordenado de la más reciente a la más antigua
        public List<Cita> ObtenerHistorialClienta(int clientaID)
        {
            return _citas
                .Where(c => c.Clienta.ClientaID == clientaID)
                .OrderByDescending(c => c.FechaHora)
                .ToList();
        }

        // Busca una cita por ID — si no existe lanza un error
        private Cita ObtenerPorID(int citaID)
        {
            return _citas.FirstOrDefault(c => c.CitaID == citaID)
                ?? throw new KeyNotFoundException($"No se encontró ninguna cita con ID {citaID}.");
        }
    }
}