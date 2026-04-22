namespace GlamBook.Entities
{
    // Esta es una clase abstracta, o sea que no se puede usar directamente.
    // Sirve como "molde" para Clienta y Empleada.
    // Todo lo que tienen en común (nombre, apellido, teléfono) va aquí.
    public abstract class Persona
    {
        // Guardo los valores en variables privadas para protegerlos
        private string _nombre = null!;
        private string _apellido = null!;
        private string _telefono = null!;

        // Nombre: no puede estar vacío
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

        // Apellido: igual, no puede estar vacío
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

        // Teléfono: tampoco puede estar vacío
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

        // Propiedad de solo lectura que junta nombre y apellido
        public string NombreCompleto => $"{Nombre} {Apellido}";

        // Constructor: cuando se crea una persona, estos 3 datos son obligatorios
        protected Persona(string nombre, string apellido, string telefono)
        {
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
        }

        // Método abstracto: cada subclase TIENE que implementar esto a su manera
        public abstract string ObtenerInfo();

        // Cuando imprimo un objeto Persona, llama a ObtenerInfo() automáticamente
        public override string ToString() => ObtenerInfo();
    }
}