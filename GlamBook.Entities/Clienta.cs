namespace GlamBook.Entities
{
    public class Clienta : Persona
    {
        public int ClientaID { get; set; }
        public string Correo { get; set; }
        public DateTime FechaRegistro { get; private set; }

        public Clienta(string nombre, string apellido, string telefono, string correo)
            : base(nombre, apellido, telefono)
        {
            Correo = correo;
            FechaRegistro = DateTime.Now;
        }

        public Clienta(int id, string nombre, string apellido, string telefono,
                       string correo, DateTime fechaRegistro)
            : base(nombre, apellido, telefono)
        {
            ClientaID = id;
            Correo = correo;
            FechaRegistro = fechaRegistro;
        }

        public override string ObtenerInfo()
        {
            return $"[Clienta #{ClientaID}] {NombreCompleto} | Tel: {Telefono} | Correo: {Correo}";
        }
    }
}