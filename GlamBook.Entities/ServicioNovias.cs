namespace GlamBook.Entities
{
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

        // SOBREESCRITURA (override) — precio sin descuento
        // Si tiene prueba suma el costo extra, si no devuelve el precio normal
        public override decimal Calcular()
        {
            return IncluyePrueba ? Precio + CostoPrueba : Precio;
        }

        // SOBRECARGA — mismo nombre de método, diferente parámetro.
        // C# los distingue automáticamente por la firma.
        // Ejemplo: Calcular(10) aplica 10% de descuento al total
        public decimal Calcular(decimal porcentajeDescuento)
        {
            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El descuento debe estar entre 0 y 100.");

            decimal total = Calcular(); // reutilizo el override
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
