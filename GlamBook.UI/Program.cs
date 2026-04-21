using GlamBook.DAL;
using GlamBook.Entities;
using GlamBook.BLL;

var bll = new CitaBLL();

string connectionString = "Data Source=Gaby;Initial Catalog=GlamBookDB;Integrated Security=True;TrustServerCertificate=True;";

var clientaDAL = new ClientaDAL(connectionString);

// crear clienta
var clienta = new Clienta("Ana", "Perez", "8091234567", "ana@email.com")
{
    ClientaID = 1
};

// crear servicio
var servicio = new ServicioMaquillaje(
    "Maquillaje básico",
    "Maquillaje sencillo",
    2000,
    60,
    "Natural"
);

// agendar cita
var cita = bll.AgendarCita(clienta, servicio, DateTime.Now.AddHours(2));

// mostrar
Console.WriteLine("CITA CREADA:");
Console.WriteLine(cita);

// listar todas
Console.WriteLine("\nLISTA DE CITAS:");
foreach (var c in bll.ObtenerTodas())
{
    Console.WriteLine(c);
}