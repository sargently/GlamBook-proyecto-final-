using System.Net.Http.Json;
using GlamBook.Entities;

var apiBaseUrl = args.FirstOrDefault()
    ?? Environment.GetEnvironmentVariable("GLAMBOOK_API_BASEURL")
    ?? "http://localhost:5137";

using var httpClient = new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
};

Console.WriteLine($"Consumiento API en: {httpClient.BaseAddress}");

try
{
    Console.WriteLine("\n=== CLIENTAS ACTUALES (GET /api/clientas) ===");
    await ListarClientasAsync(httpClient);

    var nuevaClienta = new CrearClientaRequest(
        "Ana",
        $"Perez-{DateTime.Now:HHmmss}",
        "8091234567",
        "ana@email.com");

    Console.WriteLine("\n=== CREAR CLIENTA (POST /api/clientas) ===");
    var postResponse = await httpClient.PostAsJsonAsync("/api/clientas", nuevaClienta);
    postResponse.EnsureSuccessStatusCode();

    var clientaCreada = await postResponse.Content.ReadFromJsonAsync<ClientaDto>();
    Console.WriteLine($"Clienta creada con ID: {clientaCreada?.ClientaID}");

    Console.WriteLine("\n=== CLIENTAS DESPUÉS DE CREAR ===");
    await ListarClientasAsync(httpClient);

    if (clientaCreada is not null)
    {
        Console.WriteLine($"\n=== ELIMINAR CLIENTA (DELETE /api/clientas/{clientaCreada.ClientaID}) ===");
        var deleteResponse = await httpClient.DeleteAsync($"/api/clientas/{clientaCreada.ClientaID}");
        deleteResponse.EnsureSuccessStatusCode();
        Console.WriteLine("Clienta eliminada correctamente.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error consumiendo API: {ex.Message}");
}

Console.WriteLine("\n=== POLIMORFISMO (DEMO POO) ===");
List<ServicioBase> servicios =
[
    new ServicioMaquillaje("Maquillaje Social", "Evento", 2000m, 60, "Natural"),
    new ServicioNovias("Paquete Novia Premium", "Con prueba", 8500m, 180, "Premium", incluyePrueba: true)
];

foreach (var servicio in servicios)
{
    Console.WriteLine($"[{servicio.GetType().Name}] {servicio.Nombre} -> RD$ {servicio.Calcular():N2}");
}

static async Task ListarClientasAsync(HttpClient httpClient)
{
    var clientas = await httpClient.GetFromJsonAsync<List<ClientaDto>>("/api/clientas") ?? [];

    if (clientas.Count == 0)
    {
        Console.WriteLine("No hay clientas registradas.");
        return;
    }

    foreach (var c in clientas)
    {
        Console.WriteLine($"#{c.ClientaID}: {c.Nombre} {c.Apellido} | {c.Telefono} | {c.Correo}");
    }
}

public record CrearClientaRequest(string Nombre, string Apellido, string Telefono, string Correo);

public class ClientaDto
{
    public int ClientaID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; }
}
