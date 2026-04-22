using GlamBook.DAL;
using GlamBook.Entities;
using GlamBook.BLL;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

string connectionString =
    "Data Source=Gaby;Initial Catalog=GlamBookDB;Integrated Security=True;TrustServerCertificate=True;";

// BLL (citas en memoria para demo de POO/validación de horarios)
var bll = new CitaBLL();

// DAL (clientas desde BD)
var clientaDAL = new ClientaDAL(connectionString);

// Servicios en memoria 
var servicios = new List<ServicioBase>
{
    new ServicioMaquillaje("Maquillaje Social", "Maquillaje sencillo para eventos", 2000m, 60, "Natural")
    { ServicioID = 1 },

    new ServicioNovias("Paquete Novia Premium", "Maquillaje completo con sesión de prueba", 8500m, 180, "Premium", incluyePrueba: true)
    { ServicioID = 2 },

    new ServicioMaquillaje("Maquillaje Artístico", "Fantasía y teatro", 3500m, 120, "Artístico")
    { ServicioID = 3 }
};

while (true)
{
    Console.Clear();
    Console.WriteLine("==========================================");
    Console.WriteLine("         GLAMBOOK - CONSOLA (C#)          ");
    Console.WriteLine(" Agenda de Maquillaje y Citas (con BD)    ");
    Console.WriteLine("==========================================");
    Console.WriteLine("1) Registrar clienta (BD)");
    Console.WriteLine("2) Listar clientas (BD)");
    Console.WriteLine("3) Eliminar clienta (BD)");
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("4) Agendar cita (memoria)");
    Console.WriteLine("5) Listar citas (memoria)");
    Console.WriteLine("6) Cancelar cita (memoria)");
    Console.WriteLine("7) Reprogramar cita (memoria)");
    Console.WriteLine("------------------------------------------");
    Console.WriteLine("8) Sobrecarga: Calcular() con descuento");
    Console.WriteLine("9) Polimorfismo: lista de servicios");
    Console.WriteLine("0) Salir");
    Console.Write("\nElige una opción: ");

    var op = Console.ReadLine()?.Trim();

    try
    {
        switch (op)
        {
            case "1":
                RegistrarClientaBD();
                break;
            case "2":
                ListarClientasBD();
                break;
            case "3":
                EliminarClientaBD();
                break;
            case "4":
                AgendarCitaMemoria();
                break;
            case "5":
                ListarCitasMemoria();
                break;
            case "6":
                CancelarCitaMemoria();
                break;
            case "7":
                ReprogramarCitaMemoria();
                break;
            case "8":
                DemoSobrecarga();
                break;
            case "9":
                DemoPolimorfismo();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("\nOpción inválida.");
                Pause();
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nERROR: {ex.Message}");
        Pause();
    }
}

// ===========================
// BD: CLIENTAS (DAL)
// ===========================
void RegistrarClientaBD()
{
    Console.Clear();
    Console.WriteLine("=== REGISTRAR CLIENTA (BD) ===\n");

    Console.Write("Nombre -> ");
    var nombre = (Console.ReadLine() ?? "").Trim();

    Console.Write("Apellido -> ");
    var apellido = (Console.ReadLine() ?? "").Trim();

    Console.Write("Teléfono -> ");
    var tel = (Console.ReadLine() ?? "").Trim();

    Console.Write("Correo -> ");
    var correo = (Console.ReadLine() ?? "").Trim();

    // dispara validaciones de Persona

    var nueva = new Clienta(nombre, apellido, tel, correo);

    int id = clientaDAL.Guardar(nueva);
    nueva.ClientaID = id;

    Console.WriteLine("\nClienta guardada en BD:");
    Console.WriteLine(nueva);

    Pause();
}

void ListarClientasBD()
{
    Console.Clear();
    Console.WriteLine("=== LISTA DE CLIENTAS (BD) ===\n");

    var lista = clientaDAL.ObtenerTodas();
    if (lista.Count == 0)
    {
        Console.WriteLine("No hay clientas registradas en la base de datos.");
        Pause();
        return;
    }

    foreach (var c in lista)
        Console.WriteLine(c);

    Pause();
}

void EliminarClientaBD()
{
    Console.Clear();
    Console.WriteLine("=== ELIMINAR CLIENTA (BD) ===\n");

    var lista = clientaDAL.ObtenerTodas();
    if (lista.Count == 0)
        throw new InvalidOperationException("No hay clientas para eliminar.");

    foreach (var c in lista)
        Console.WriteLine(c);

    Console.Write("\nClientaID a eliminar -> ");
    if (!int.TryParse(Console.ReadLine(), out var id))
        throw new ArgumentException("ID inválido.");

    clientaDAL.Eliminar(id);
    Console.WriteLine("\nClienta eliminada.");
    Pause();
}

// ===========================
// MEMORIA: CITAS (BLL)
// ===========================
void AgendarCitaMemoria()
{
    Console.Clear();
    Console.WriteLine("=== AGENDAR CITA (MEMORIA) ===");
    Console.WriteLine("(Usa clientas desde BD y servicios en memoria)\n");

    var clienta = SeleccionarClientaDesdeBD();
    var servicio = SeleccionarServicio();

    Console.Write("\nFecha y hora (yyyy-MM-dd HH:mm) -> ");
    var input = Console.ReadLine();

    if (!DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var fechaHora))
        throw new ArgumentException("Formato inválido. Usa: yyyy-MM-dd HH:mm (ej: 2026-04-22 14:00)");

    Console.Write("Notas (opcional) -> ");
    var notas = Console.ReadLine() ?? "";

    var cita = bll.AgendarCita(clienta, servicio, fechaHora, notas);

    Console.WriteLine("\nCita agendada:");
    Console.WriteLine(cita);

    Pause();
}

void ListarCitasMemoria()
{
    Console.Clear();
    Console.WriteLine("=== LISTA DE CITAS (MEMORIA) ===\n");

    var lista = bll.ObtenerTodas();
    if (lista.Count == 0)
    {
        Console.WriteLine("No hay citas registradas.");
        Pause();
        return;
    }

    foreach (var c in lista)
        Console.WriteLine(c);

    Pause();
}

void CancelarCitaMemoria()
{
    Console.Clear();
    Console.WriteLine("=== CANCELAR CITA (MEMORIA) ===\n");

    ListarCitasSimple();

    Console.Write("\nID de la cita -> ");
    if (!int.TryParse(Console.ReadLine(), out var id))
        throw new ArgumentException("ID inválido.");

    bll.CancelarCita(id);

    Console.WriteLine("\nCita cancelada.");
    Pause();
}

void ReprogramarCitaMemoria()
{
    Console.Clear();
    Console.WriteLine("=== REPROGRAMAR CITA (MEMORIA) ===\n");

    ListarCitasSimple();

    Console.Write("\nID de la cita -> ");
    if (!int.TryParse(Console.ReadLine(), out var id))
        throw new ArgumentException("ID inválido.");

    Console.Write("Nueva fecha y hora (yyyy-MM-dd HH:mm) -> ");
    var input = Console.ReadLine();

    if (!DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var nuevaFecha))
        throw new ArgumentException("Formato inválido. Usa: yyyy-MM-dd HH:mm");

    bll.ReprogramarCita(id, nuevaFecha);

    Console.WriteLine("\nCita reprogramada.");
    Pause();
}

// ===========================
// POO: SOBRECARGA / POLIMORFISMO
// ===========================
void DemoSobrecarga()
{
    Console.Clear();
    Console.WriteLine("=== SOBRECARGA: Calcular() ===\n");

    var novia = servicios.OfType<ServicioNovias>().First();

    Console.WriteLine($"Servicio: {novia.Nombre}");
    Console.WriteLine($"Sin descuento : RD$ {novia.Calcular():N2}");

    Console.Write("\nPorcentaje de descuento (0-100) -> ");
    if (!decimal.TryParse(Console.ReadLine(), out var p))
        throw new ArgumentException("Porcentaje inválido.");

    Console.WriteLine($"Con {p}% desc. : RD$ {novia.Calcular(p):N2}");

    Pause();
}

void DemoPolimorfismo()
{
    Console.Clear();
    Console.WriteLine("=== POLIMORFISMO ===\n");

    foreach (var s in servicios)
        Console.WriteLine($"[{s.GetType().Name}] {s.Nombre} -> RD$ {s.Calcular():N2}");

    Pause();
}

// ===========================
// HELPERS
// ===========================
Clienta SeleccionarClientaDesdeBD()
{
    var lista = clientaDAL.ObtenerTodas();
    if (lista.Count == 0)
        throw new InvalidOperationException("No hay clientas en BD. Primero registra una (opción 1).");

    Console.WriteLine("Clientas (desde BD):");
    foreach (var c in lista)
        Console.WriteLine($"  {c.ClientaID}) {c.NombreCompleto} - {c.Telefono}");

    Console.Write("\nSelecciona ClientaID -> ");
    if (!int.TryParse(Console.ReadLine(), out var id))
        throw new ArgumentException("ClientaID inválido.");

    return lista.FirstOrDefault(x => x.ClientaID == id)
        ?? throw new KeyNotFoundException("No existe una clienta con ese ID.");
}

ServicioBase SeleccionarServicio()
{
    Console.WriteLine("\nServicios:");
    foreach (var s in servicios)
        Console.WriteLine($"  {s.ServicioID}) {s.Nombre} - RD$ {s.Calcular():N2} - {s.DuracionMinutos} min");

    Console.Write("\nSelecciona ServicioID -> ");
    if (!int.TryParse(Console.ReadLine(), out var id))
        throw new ArgumentException("ServicioID inválido.");

    return servicios.FirstOrDefault(x => x.ServicioID == id)
        ?? throw new KeyNotFoundException("No existe un servicio con ese ID.");
}

void ListarCitasSimple()
{
    var lista = bll.ObtenerTodas();
    if (lista.Count == 0)
        throw new InvalidOperationException("No hay citas para mostrar.");

    foreach (var c in lista)
        Console.WriteLine(c);
}

void Pause()
{
    Console.WriteLine("\nPresiona una tecla para continuar...");
    Console.ReadKey();
}