using GlamBook.DAL;
using GlamBook.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GlamBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientasController : ControllerBase
{
    private readonly ClientaDAL _clientaDal;

    public ClientasController(ClientaDAL clientaDal)
    {
        _clientaDal = clientaDal;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Clienta>> Get()
    {
        var clientas = _clientaDal.ObtenerTodas();
        return Ok(clientas);
    }

    [HttpPost]
    public ActionResult<Clienta> Post([FromBody] CrearClientaRequest request)
    {
        var clienta = new Clienta(request.Nombre, request.Apellido, request.Telefono, request.Correo);
        var id = _clientaDal.Guardar(clienta);

        var created = new Clienta(
            id,
            clienta.Nombre,
            clienta.Apellido,
            clienta.Telefono,
            clienta.Correo,
            clienta.FechaRegistro);

        return Created($"/api/clientas/{id}", created);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _clientaDal.Eliminar(id);
        return NoContent();
    }
}

public record CrearClientaRequest(string Nombre, string Apellido, string Telefono, string Correo);
