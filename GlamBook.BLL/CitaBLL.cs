using GlamBook.Entities;

namespace GlamBook.BLL
{
    public class CitaBLL
    {
        private readonly List<Cita> _citas = new();
        private int _nextId = 1;

        public Cita AgendarCita(Clienta clienta, ServicioBase servicio, DateTime fechaHora, string notas = "")
        {
            ValidarDisponibilidad(fechaHora, servicio.DuracionMinutos);

            var cita = new Cita(clienta, servicio, fechaHora, notas)
            {
                CitaID = _nextId++
            };

            _citas.Add(cita);
            return cita;
        }

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
                    $"Ya existe una cita en ese horario. Elige otra hora.");
        }

        public void CancelarCita(int citaID)
        {
            var cita = ObtenerPorID(citaID);
            cita.Cancelar();
        }

        public void ReprogramarCita(int citaID, DateTime nuevaFecha)
        {
            var cita = ObtenerPorID(citaID);
            ValidarDisponibilidad(nuevaFecha, cita.Servicio.DuracionMinutos);
            cita.Reprogramar(nuevaFecha);
        }

        public List<Cita> ObtenerTodas() => _citas.ToList();

        public List<Cita> ObtenerHistorialClienta(int clientaID)
        {
            return _citas
                .Where(c => c.Clienta.ClientaID == clientaID)
                .OrderByDescending(c => c.FechaHora)
                .ToList();
        }

        private Cita ObtenerPorID(int citaID)
        {
            return _citas.FirstOrDefault(c => c.CitaID == citaID)
                ?? throw new KeyNotFoundException($"No se encontró la cita con ID {citaID}.");
        }
    }
}