namespace GlamBook.Entities
{
    public class ServicioMaquillaje : ServicioBase
    {
        public string TipoMaquillaje { get; set; }
        public List<string> Materiales { get; set; }

        public ServicioMaquillaje(string nombre, string descripcion,
                                  decimal precio, int duracionMinutos,
                                  string tipoMaquillaje)
            : base(nombre, descripcion, precio, duracionMinutos)
        {
            TipoMaquillaje = tipoMaquillaje;
            Materiales = new List<string>();
        }

        public void AgregarMaterial(string material)
        {
            if (!string.IsNullOrWhiteSpace(material))
                Materiales.Add(material.Trim());
        }

        public override decimal Calcular() => Precio;
    }


    public class ServicioNovias : ServicioBase
    {
        public string NombrePaquete { get; set; }
        public bool IncluyePrueba { get; set; }
        public decimal CostoPrueba { get; set; }

        public ServicioNovias(string nombre, string descripcion,
                              decimal precio, int duracionMinutos,
                              string nombrePaquete, bool incluyePrueba = false)
            : base(nombre, descripcion, precio, duracionMinutos)
        {
            NombrePaquete = nombrePaquete;
            IncluyePrueba = incluyePrueba;
            CostoPrueba = incluyePrueba ? precio * 0.30m : 0m;
        }

        // Sobrecarga 1 — sin descuento
        public override decimal Calcular()
        {
            return IncluyePrueba ? Precio + CostoPrueba : Precio;
        }

        // Sobrecarga 2 — con descuento
        public decimal Calcular(decimal porcentajeDescuento)
        {
            if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                throw new ArgumentException("El descuento debe estar entre 0 y 100.");

            decimal total = Calcular();
            decimal descuento = total * (porcentajeDescuento / 100m);
            return total - descuento;
        }

        public void AgregarPrueba()
        {
            IncluyePrueba = true;
            CostoPrueba = Precio * 0.30m;
        }
    }
}