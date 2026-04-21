namespace GlamBook.Entities
{
    // Clase abstracta para todos los servicios del salón.
    // No se puede usar directamente — sirve de base para
    // ServicioMaquillaje y ServicioNovias.
    public abstract class ServicioBase
    {
        // Variables privadas para proteger precio y duración
        private decimal _precio;
        private int _duracionMinutos;

        // ID que viene de la base de datos
        public int ServicioID { get; set; }

        // Nombre del servicio (ej: "Maquillaje Social")
        public string Nombre { get; set; }

        // Descripción breve de qué incluye el servicio
        public string Descripcion { get; set; }

        // Precio: no puede ser negativo
        public decimal Precio
        {
            get => _precio;
            set
            {
                if (value < 0)
                    throw new ArgumentException("El precio no puede ser negativo.");
                _precio = value;
            }
        }

        // Duración en minutos: tiene que ser mayor a cero
        public int DuracionMinutos
        {
            get => _duracionMinutos;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La duración debe ser mayor a cero.");
                _duracionMinutos = value;
            }
        }

        // Constructor: datos básicos que todo servicio necesita
        protected ServicioBase(string nombre, string descripcion, decimal precio, int duracionMinutos)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            DuracionMinutos = duracionMinutos;
        }

        // Método abstracto: cada tipo de servicio calcula su precio a su manera
        public abstract decimal Calcular();

        // Al imprimir un servicio muestra su nombre, precio y duración
        public override string ToString()
        {
            return $"{Nombre} | RD$ {Calcular():N2} | {DuracionMinutos} min";
        }
    }
}