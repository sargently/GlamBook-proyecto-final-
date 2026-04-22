using GlamBook.DAL;
using GlamBook.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GlamBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientasController : ControllerBase
{
    private readonly ClientaDAL _clientaDal;
    private readonly ILogger<ClientasController> _logger;

    public ClientasController(ClientaDAL clientaDal, ILogger<ClientasController> logger)
    {
        _clientaDal = clientaDal;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ClientaDto>> Get()
    {
        try
        {
            var clientas = _clientaDal
                .ObtenerTodas()
                .Select(c => new ClientaDto(c.ClientaID, c.Nombre, c.Apellido, c.Telefono, c.Correo, c.FechaRegistro))
                .ToList();

            return Ok(clientas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientas.");
            return Problem(title: "No se pudo obtener la lista de clientas.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public ActionResult<ClientaDto> Post([FromBody] CrearClientaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre) ||
            string.IsNullOrWhiteSpace(request.Apellido) ||
            string.IsNullOrWhiteSpace(request.Telefono) ||
            string.IsNullOrWhiteSpace(request.Correo))
        {
            return BadRequest("Nombre, Apellido, Telefono y Correo son obligatorios.");
        }

        try
        {
            var clienta = new Clienta(request.Nombre, request.Apellido, request.Telefono, request.Correo);
            var id = _clientaDal.Guardar(clienta);

            var created = new ClientaDto(
                id,
                clienta.Nombre,
                clienta.Apellido,
                clienta.Telefono,
                clienta.Correo,
                clienta.FechaRegistro);

            return Created($"/api/clientas/{id}", created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear clienta.");
            return Problem(title: "No se pudo crear la clienta.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        try
        {
            var deleted = _clientaDal.Eliminar(id);

            if (!deleted)
            {
                return NotFound($"No existe una clienta con ID {id}.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar clienta con ID {ClientaId}.", id);
            return Problem(title: "No se pudo eliminar la clienta.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
