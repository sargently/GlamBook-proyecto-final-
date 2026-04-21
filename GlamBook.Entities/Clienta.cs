using System.Text.RegularExpressions;

namespace GlamBook.Entities
{
    // Clienta hereda de Persona, o sea que ya tiene Nombre, Apellido y Telefono.
    // Aquí solo agregamos lo que es específico de una clienta del salón.
    public class Clienta : Persona
    {
        // ID que viene de la base de datos
        public int ClientaID { get; set; }

        // Variable privada para proteger el correo
        private string _correo = null!;

        // Correo electrónico: debe tener formato válido (algo@algo.algo)
        public string Correo
        {
            get => _correo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El correo no puede estar vacío.");
                if (!Regex.IsMatch(value.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new ArgumentException("El correo no tiene un formato válido.");
                _correo = value.Trim();
            }
        }

        // Fecha en que se registró en el sistema (se guarda automáticamente)
        public DateTime FechaRegistro { get; private set; }

        // Constructor para cuando registro una clienta nueva
        // La fecha de registro se pone sola con DateTime.Now
        public Clienta(string nombre, string apellido, string telefono, string correo)
            : base(nombre, apellido, telefono)
        {
            Correo = correo;
            FechaRegistro = DateTime.Now;
        }

        // Constructor para cuando traigo una clienta de la base de datos
        // Aquí sí recibo el ID y la fecha porque ya existen en la BD
        public Clienta(int id, string nombre, string apellido, string telefono,
                       string correo, DateTime fechaRegistro)
            : base(nombre, apellido, telefono)
        {
            ClientaID = id;
            Correo = correo;
            FechaRegistro = fechaRegistro;
        }

        // Implementación del método abstracto de Persona
        // Muestra toda la info de la clienta en una sola línea
        public override string ObtenerInfo()
        {
            return $"[Clienta #{ClientaID}] {NombreCompleto} | Tel: {Telefono} | Correo: {Correo}";
        }
    }
}