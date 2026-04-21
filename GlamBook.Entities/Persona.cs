namespace GlamBook.Entities
{
    public abstract class Persona
    {
        private string _nombre;
        private string _apellido;
        private string _telefono;

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío.");
                _nombre = value.Trim();
            }
        }

        public string Apellido
        {
            get => _apellido;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El apellido no puede estar vacío.");
                _apellido = value.Trim();
            }
        }

        public string Telefono
        {
            get => _telefono;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El teléfono no puede estar vacío.");
                _telefono = value.Trim();
            }
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";

        protected Persona(string nombre, string apellido, string telefono)
        {
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
        }

        public abstract string ObtenerInfo();

        public override string ToString() => ObtenerInfo();
    }
}