namespace GlamBook.Entities
{
    // Servicio de maquillaje estándar (social, artístico, fantasía, etc.)
    // Hereda de ServicioBase e implementa Calcular() de forma simple.
    public class ServicioMaquillaje : ServicioBase
    {
        // Tipo de maquillaje que se va a realizar
        public string TipoMaquillaje { get; set; }

        // Lista de materiales que se usan en este servicio
        public List<string> Materiales { get; set; }

        // Constructor: recibe todos los datos del servicio
        public ServicioMaquillaje(string nombre, string descripcion,
                                  decimal precio, int duracionMinutos,
                                  string tipoMaquillaje)
            : base(nombre, descripcion, precio, duracionMinutos)
        {
            TipoMaquillaje = tipoMaquillaje;
            Materiales = new List<string>();
        }

        // Método para agregar materiales a la lista
        public void AgregarMaterial(string material)
        {
            if (!string.IsNullOrWhiteSpace(material))
                Materiales.Add(material.Trim());
        }

        // Para maquillaje estándar el precio no cambia, se devuelve tal cual
        public override decimal Calcular() => Precio;
    }
}
