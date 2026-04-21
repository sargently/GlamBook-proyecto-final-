using GlamBook.DAL;
using GlamBook.Entities;
using GlamBook.BLL;

// Cadena de conexión a mi base de datos local
string connectionString = "Data Source=Gaby;Initial Catalog=GlamBookDB;Integrated Security=True;TrustServerCertificate=True;";

// Creo las instancias de las capas que voy a usar
var bll = new CitaBLL();
var clientaDAL = new ClientaDAL(connectionString);

// ----------------------------
// DATOS DE PRUEBA
// ----------------------------

// Creo una clienta nueva
var clienta = new Clienta("Ana", "Perez", "8091234567", "ana@email.com")
{
    ClientaID = 1
};

// Creo un servicio de maquillaje estándar
var servicioMaquillaje = new ServicioMaquillaje(
    "Maquillaje Social",
    "Maquillaje sencillo para eventos",
    2000m,
    60,
    "Natural"
);

// Creo un servicio de novia con prueba incluida
var servicioNovia = new ServicioNovias(
    "Paquete Novia Premium",
    "Maquillaje completo con sesión de prueba",
    8500m,
    180,
    "Premium",
    incluyePrueba: true
);

// ----------------------------
// DEMO: AGENDAR CITA
// ----------------------------
Console.WriteLine("=== CITA AGENDADA ===");
var cita = bll.AgendarCita(clienta, servicioMaquillaje, DateTime.Now.AddHours(2));
Console.WriteLine(cita);

// ----------------------------
// DEMO: LISTAR CITAS
// ----------------------------
Console.WriteLine("\n=== LISTA DE CITAS ===");
foreach (var c in bll.ObtenerTodas())
{
    Console.WriteLine(c);
}

// ----------------------------
// DEMO: SOBRECARGA DE Calcular()
// ----------------------------
Console.WriteLine("\n=== SOBRECARGA: Calcular() ===");
Console.WriteLine($"Servicio  : {servicioNovia.Nombre}");
Console.WriteLine($"Sin descuento : RD$ {servicioNovia.Calcular():N2}");
Console.WriteLine($"Con 10% desc. : RD$ {servicioNovia.Calcular(10):N2}");
Console.WriteLine($"Con 25% desc. : RD$ {servicioNovia.Calcular(25):N2}");

// ----------------------------
// DEMO: POLIMORFISMO
// ----------------------------
Console.WriteLine("\n=== POLIMORFISMO ===");

// Esta lista puede guardar cualquier tipo de servicio
// porque todos heredan de ServicioBase
List<ServicioBase> servicios = new()
{
    servicioMaquillaje,
    servicioNovia,
    new ServicioMaquillaje("Maquillaje Artístico", "Fantasía y teatro", 3500m, 120, "Artístico")
};

foreach (ServicioBase s in servicios)
{
    // Calcular() se comporta diferente según el tipo real del objeto
    Console.WriteLine($"[{s.GetType().Name}] {s.Nombre} → RD$ {s.Calcular():N2}");
}