using GlamBook.DAL;
using GlamBook.Entities;
using GlamBook.BLL;

// ---------------------------------------------------------------
// Cadena de conexión — cámbiala por la de tu entorno local
// ---------------------------------------------------------------
const string CONNECTION_STRING =
    "Data Source=Gaby;Initial Catalog=GlamBookDB;Integrated Security=True;TrustServerCertificate=True;";

// Instancias de las capas
var bll        = new CitaBLL();
var clientaDAL = new ClientaDAL(CONNECTION_STRING);
var empleadaDAL = new EmpleadaDAL(CONNECTION_STRING);

MostrarMenu();

// ---------------------------------------------------------------
// MENÚ PRINCIPAL
// ---------------------------------------------------------------
void MostrarMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║         G L A M B O O K      ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine("  1. Gestión de Clientas");
        Console.WriteLine("  2. Gestión de Empleadas");
        Console.WriteLine("  3. Gestión de Citas");
        Console.WriteLine("  4. Demo de Polimorfismo y Sobrecarga");
        Console.WriteLine("  0. Salir");
        Console.Write("\nElige una opción: ");

        switch (Console.ReadLine())
        {
            case "1": MenuClientas(); break;
            case "2": MenuEmpleadas(); break;
            case "3": MenuCitas();    break;
            case "4": DemoServicios(); break;
            case "0": Console.WriteLine("¡Hasta pronto!"); return;
            default:  Pausa("Opción no válida."); break;
        }
    }
}

// ---------------------------------------------------------------
// MENÚ CLIENTAS
// ---------------------------------------------------------------
void MenuClientas()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("── Clientas ──────────────────");
        Console.WriteLine("  1. Ver todas");
        Console.WriteLine("  2. Registrar nueva");
        Console.WriteLine("  3. Eliminar por ID");
        Console.WriteLine("  0. Volver");
        Console.Write("\nOpción: ");

        switch (Console.ReadLine())
        {
            case "1": ListarClientas();    break;
            case "2": RegistrarClienta();  break;
            case "3": EliminarClienta();   break;
            case "0": return;
            default:  Pausa("Opción no válida."); break;
        }
    }
}

void ListarClientas()
{
    try
    {
        var lista = clientaDAL.ObtenerTodas();
        Console.Clear();
        if (lista.Count == 0)
        {
            Pausa("No hay clientas registradas.");
            return;
        }
        foreach (var c in lista)
            Console.WriteLine(c);
        Pausa();
    }
    catch (Exception ex)
    {
        Pausa($"Error al conectar con la BD: {ex.Message}");
    }
}

void RegistrarClienta()
{
    Console.Clear();
    Console.WriteLine("── Nueva Clienta ─────────────");
    try
    {
        string nombre   = Leer("Nombre");
        string apellido = Leer("Apellido");
        string telefono = Leer("Teléfono");
        string correo   = Leer("Correo electrónico");

        var clienta = new Clienta(nombre, apellido, telefono, correo);
        int id = clientaDAL.Guardar(clienta);
        Pausa($"✓ Clienta registrada con ID {id}.");
    }
    catch (ArgumentException ex)
    {
        Pausa($"Dato inválido: {ex.Message}");
    }
    catch (Exception ex)
    {
        Pausa($"Error: {ex.Message}");
    }
}

void EliminarClienta()
{
    Console.Clear();
    Console.Write("ID de la clienta a eliminar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Pausa("ID inválido.");
        return;
    }
    try
    {
        clientaDAL.Eliminar(id);
        Pausa($"✓ Clienta #{id} eliminada.");
    }
    catch (Exception ex)
    {
        Pausa($"Error: {ex.Message}");
    }
}

// ---------------------------------------------------------------
// MENÚ EMPLEADAS
// ---------------------------------------------------------------
void MenuEmpleadas()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("── Empleadas ─────────────────");
        Console.WriteLine("  1. Ver todas");
        Console.WriteLine("  2. Registrar nueva");
        Console.WriteLine("  3. Eliminar por ID");
        Console.WriteLine("  0. Volver");
        Console.Write("\nOpción: ");

        switch (Console.ReadLine())
        {
            case "1": ListarEmpleadas();   break;
            case "2": RegistrarEmpleada(); break;
            case "3": EliminarEmpleada();  break;
            case "0": return;
            default:  Pausa("Opción no válida."); break;
        }
    }
}

void ListarEmpleadas()
{
    try
    {
        var lista = empleadaDAL.ObtenerTodas();
        Console.Clear();
        if (lista.Count == 0)
        {
            Pausa("No hay empleadas registradas.");
            return;
        }
        foreach (var e in lista)
            Console.WriteLine(e);
        Pausa();
    }
    catch (Exception ex)
    {
        Pausa($"Error al conectar con la BD: {ex.Message}");
    }
}

void RegistrarEmpleada()
{
    Console.Clear();
    Console.WriteLine("── Nueva Empleada ────────────");
    try
    {
        string nombre       = Leer("Nombre");
        string apellido     = Leer("Apellido");
        string telefono     = Leer("Teléfono");
        string especialidad = Leer("Especialidad");

        var empleada = new Empleada(nombre, apellido, telefono, especialidad);
        int id = empleadaDAL.Guardar(empleada);
        Pausa($"✓ Empleada registrada con ID {id}.");
    }
    catch (ArgumentException ex)
    {
        Pausa($"Dato inválido: {ex.Message}");
    }
    catch (Exception ex)
    {
        Pausa($"Error: {ex.Message}");
    }
}

void EliminarEmpleada()
{
    Console.Clear();
    Console.Write("ID de la empleada a eliminar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Pausa("ID inválido.");
        return;
    }
    try
    {
        empleadaDAL.Eliminar(id);
        Pausa($"✓ Empleada #{id} eliminada.");
    }
    catch (Exception ex)
    {
        Pausa($"Error: {ex.Message}");
    }
}

// ---------------------------------------------------------------
// MENÚ CITAS (en memoria vía CitaBLL)
// ---------------------------------------------------------------
void MenuCitas()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("── Citas ─────────────────────");
        Console.WriteLine("  1. Ver todas");
        Console.WriteLine("  2. Agendar nueva cita");
        Console.WriteLine("  3. Cancelar cita");
        Console.WriteLine("  4. Reprogramar cita");
        Console.WriteLine("  5. Historial de clienta");
        Console.WriteLine("  0. Volver");
        Console.Write("\nOpción: ");

        switch (Console.ReadLine())
        {
            case "1": ListarCitas();       break;
            case "2": AgendarCita();       break;
            case "3": CancelarCita();      break;
            case "4": ReprogramarCita();   break;
            case "5": HistorialClienta();  break;
            case "0": return;
            default:  Pausa("Opción no válida."); break;
        }
    }
}

void ListarCitas()
{
    Console.Clear();
    var citas = bll.ObtenerTodas();
    if (citas.Count == 0)
    {
        Pausa("No hay citas registradas en esta sesión.");
        return;
    }
    foreach (var c in citas)
        Console.WriteLine(c);
    Pausa();
}

void AgendarCita()
{
    Console.Clear();
    Console.WriteLine("── Nueva Cita ────────────────");
    try
    {
        // Datos de la clienta (simplificado para la demo)
        string nombre   = Leer("Nombre de la clienta");
        string apellido = Leer("Apellido");
        string telefono = Leer("Teléfono");
        string correo   = Leer("Correo");
        var clienta = new Clienta(nombre, apellido, telefono, correo) { ClientaID = 1 };

        // Servicio
        Console.WriteLine("Tipo de servicio: 1) Maquillaje Social  2) Paquete Novia");
        Console.Write("Elige (1/2): ");
        string opcion = Console.ReadLine() ?? "1";

        ServicioBase servicio = opcion == "2"
            ? new ServicioNovias("Paquete Novia Premium", "Completo con prueba",
                                 8500m, 180, "Premium", incluyePrueba: true)
            : new ServicioMaquillaje("Maquillaje Social", "Maquillaje de evento",
                                     2000m, 60, "Natural");

        // Fecha y hora
        Console.Write("Fecha y hora (dd/MM/yyyy HH:mm): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
            null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
        {
            Pausa("Formato de fecha inválido.");
            return;
        }

        string notas = Leer("Notas (opcional, Enter para omitir)");
        var cita = bll.AgendarCita(clienta, servicio, fecha, notas);
        Pausa($"✓ Cita #{cita.CitaID} agendada: {cita}");
    }
    catch (ArgumentException ex)
    {
        Pausa($"Dato inválido: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Pausa($"Conflicto de horario: {ex.Message}");
    }
    catch (Exception ex)
    {
        Pausa($"Error: {ex.Message}");
    }
}

void CancelarCita()
{
    Console.Clear();
    Console.Write("ID de la cita a cancelar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Pausa("ID inválido.");
        return;
    }
    try
    {
        bll.CancelarCita(id);
        Pausa($"✓ Cita #{id} cancelada.");
    }
    catch (KeyNotFoundException ex)
    {
        Pausa(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        Pausa(ex.Message);
    }
}

void ReprogramarCita()
{
    Console.Clear();
    Console.Write("ID de la cita a reprogramar: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Pausa("ID inválido.");
        return;
    }
    Console.Write("Nueva fecha y hora (dd/MM/yyyy HH:mm): ");
    if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm",
        null, System.Globalization.DateTimeStyles.None, out DateTime nuevaFecha))
    {
        Pausa("Formato de fecha inválido.");
        return;
    }
    try
    {
        bll.ReprogramarCita(id, nuevaFecha);
        Pausa($"✓ Cita #{id} reprogramada para {nuevaFecha:dd/MM/yyyy HH:mm}.");
    }
    catch (KeyNotFoundException ex)
    {
        Pausa(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        Pausa(ex.Message);
    }
}

void HistorialClienta()
{
    Console.Clear();
    Console.Write("ID de la clienta: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Pausa("ID inválido.");
        return;
    }
    var historial = bll.ObtenerHistorialClienta(id);
    if (historial.Count == 0)
    {
        Pausa("No se encontraron citas para esa clienta.");
        return;
    }
    foreach (var c in historial)
        Console.WriteLine(c);
    Pausa();
}

// ---------------------------------------------------------------
// DEMO POLIMORFISMO Y SOBRECARGA
// ---------------------------------------------------------------
void DemoServicios()
{
    Console.Clear();
    Console.WriteLine("=== POLIMORFISMO ===");
    Console.WriteLine("(Todos heredan de ServicioBase, Calcular() actúa diferente según el tipo)\n");

    // Esta lista puede guardar cualquier tipo de servicio
    List<ServicioBase> servicios = new()
    {
        new ServicioMaquillaje("Maquillaje Social",   "Eventos",  2000m, 60,  "Natural"),
        new ServicioMaquillaje("Maquillaje Artístico","Fantasía", 3500m, 120, "Artístico"),
        new ServicioNovias("Paquete Básico",  "Sin prueba", 6000m, 150, "Básico"),
        new ServicioNovias("Paquete Premium", "Con prueba", 8500m, 180, "Premium", incluyePrueba: true),
    };

    foreach (ServicioBase s in servicios)
        Console.WriteLine($"[{s.GetType().Name,-22}] {s.Nombre,-25} → RD$ {s.Calcular():N2}");

    Console.WriteLine("\n=== SOBRECARGA de Calcular() en ServicioNovias ===");
    var novia = (ServicioNovias)servicios[3]; // Paquete Premium con prueba
    Console.WriteLine($"Servicio       : {novia.Nombre}");
    Console.WriteLine($"Sin descuento  : RD$ {novia.Calcular():N2}");
    Console.WriteLine($"Con 10% desc.  : RD$ {novia.Calcular(10):N2}");
    Console.WriteLine($"Con 25% desc.  : RD$ {novia.Calcular(25):N2}");

    Pausa();
}

// ---------------------------------------------------------------
// UTILIDADES
// ---------------------------------------------------------------
static string Leer(string etiqueta)
{
    Console.Write($"{etiqueta}: ");
    return Console.ReadLine() ?? string.Empty;
}

static void Pausa(string mensaje = "")
{
    if (!string.IsNullOrEmpty(mensaje))
        Console.WriteLine(mensaje);
    Console.WriteLine("\nPresiona Enter para continuar...");
    Console.ReadLine();
}
