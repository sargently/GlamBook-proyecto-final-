namespace GlamBook.Entities
{
    // Empleada también hereda de Persona.
    // La diferencia con Clienta es que tiene una especialidad
    // (por ejemplo: maquillaje social, artístico, novias, etc.)
    public class Empleada : Persona
    {
        // ID que viene de la base de datos
        public int EmpleadaID { get; set; }

        // Especialidad de la maquillista
        public string Especialidad { get; set; }

        // Constructor para registrar una empleada nueva
        public Empleada(string nombre, string apellido, string telefono, string especialidad)
            : base(nombre, apellido, telefono)
        {
            Especialidad = especialidad;
        }

        // Constructor para cuando traigo una empleada de la base de datos
        public Empleada(int id, string nombre, string apellido, string telefono, string especialidad)
            : base(nombre, apellido, telefono)
        {
            EmpleadaID = id;
            Especialidad = especialidad;
        }

        // Implementación del método abstracto de Persona
        public override string ObtenerInfo()
        {
            return $"[Empleada #{EmpleadaID}] {NombreCompleto} | Especialidad: {Especialidad} | Tel: {Telefono}";
        }
    }
}