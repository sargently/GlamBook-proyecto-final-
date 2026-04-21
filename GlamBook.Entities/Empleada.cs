namespace GlamBook.Entities
{
    public class Empleada : Persona
    {
        public int EmpleadaID { get; set; }
        public string Especialidad { get; set; }

        public Empleada(string nombre, string apellido, string telefono, string especialidad)
            : base(nombre, apellido, telefono)
        {
            Especialidad = especialidad;
        }

        public Empleada(int id, string nombre, string apellido, string telefono, string especialidad)
            : base(nombre, apellido, telefono)
        {
            EmpleadaID = id;
            Especialidad = especialidad;
        }

        public override string ObtenerInfo()
        {
            return $"[Empleada #{EmpleadaID}] {NombreCompleto} | Especialidad: {Especialidad} | Tel: {Telefono}";
        }
    }
}