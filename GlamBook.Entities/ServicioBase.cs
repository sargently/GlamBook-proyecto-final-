namespace GlamBook.Entities
{
    public abstract class ServicioBase
    {
        private decimal _precio;
        private int _duracionMinutos;

        public int ServicioID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

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

        protected ServicioBase(string nombre, string descripcion, decimal precio, int duracionMinutos)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            DuracionMinutos = duracionMinutos;
        }

        public abstract decimal Calcular();

        public override string ToString()
        {
            return $"{Nombre} | RD$ {Calcular():N2} | {DuracionMinutos} min";
        }
    }
}