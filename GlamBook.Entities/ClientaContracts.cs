namespace GlamBook.Entities;

public record CrearClientaRequest(string Nombre, string Apellido, string Telefono, string Correo);

public record ClientaDto(int ClientaID, string Nombre, string Apellido, string Telefono, string Correo, DateTime FechaRegistro);
