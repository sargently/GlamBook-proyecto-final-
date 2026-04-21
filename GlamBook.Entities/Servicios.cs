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


    // Servicio especial para novias — tiene paquete y puede tener descuento.
    // Aquí es donde aplicamos la SOBRECARGA del método Calcular().
    public class ServicioNovias : ServicioBase
    {
        // Nombre del paquete (básico, premium, deluxe...)
        public string NombrePaquete { get; set; }

        // Indica si el paquete incluye una sesión de prueba antes del evento
        public bool IncluyePrueba { get; set; }

        // Costo adicional de la prueba (30% del precio base)
        public decimal CostoPrueba { get; set; }

        // Constructor: además de los datos básicos recibe el paquete
        // y si incluye prueba o no (por defecto no incluye)
        public ServicioNovias(string nombre, string descripcion,
                              decimal precio, int duracionMinutos,
                              string nombrePaquete, bool incluyePrueba = false)
            : base(nombre, descripcion, precio, duracionMinutos)
        {
            NombrePaquete = nombrePaquete;
            IncluyePrueba = incluyePrueba;

            // Si incluye prueba, el costo extra es el 30% del precio
            CostoPrueba = incluyePrueba ? precio * 0.30m : 0m;
        }

        // SOBRECARGA 1 — precio sin descuento
        // Si tiene prueba suma el costo extra, si no devuelve el precio normal
        public override decimal Calcular()
        {
            return IncluyePrueba ? Precio + CostoPrueba : Precio;
        }

        // SOBRECARGA 2 — precio con descuento
        // Mismo método, diferente parámetro. C# los distingue automáticamente.
        // Ejemplo: Calcular(10) aplica 10% de descuento al total
        public decimal Calcular(decimal porcentajeDescuento)
        {
            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El descuento debe estar entre 0 y 100.");

            decimal total = Calcular(); // reutilizo la sobrecarga 1
            decimal descuento = total * (porcentajeDescuento / 100m);
            return total - descuento;
        }

        // Método para agregar la prueba después de crear el servicio
        public void AgregarPrueba()
        {
            IncluyePrueba = true;
            CostoPrueba = Precio * 0.30m;
        }
    }
}