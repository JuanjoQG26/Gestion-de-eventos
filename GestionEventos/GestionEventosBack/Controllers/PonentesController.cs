using GestionEventosBack.Data;
using GestionEventosModelo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PonentesController : ControllerBase
    {
        private readonly DataContext _context;

        public PonentesController(DataContext context)
        {
            _context = context;
        }

        // ---- OBTENER TODOS LOS PONENTES ----
        // Ruta: GET api/ponentes
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var ponentes = await _context.Ponentes.ToListAsync();
            return Ok(ponentes);
        }

        // ---- OBTENER UN PONENTE POR ID ----
        // Ruta: GET api/ponentes/1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var ponente = await _context.Ponentes.FirstOrDefaultAsync(p => p.Id_Ponente == id);

            if (ponente == null)
            {
                return NotFound("Ponente no encontrado");
            }

            return Ok(ponente);
        }

        // ---- CREAR PONENTE ----
        // Ruta: POST api/ponentes
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Ponente ponente)
        {
            // Valida que el nombre sea obligatorio
            if (string.IsNullOrEmpty(ponente.Nombre))
            {
                return BadRequest("El nombre del ponente es obligatorio");
            }

            _context.Ponentes.Add(ponente);
            await _context.SaveChangesAsync();

            return Created("", ponente);
        }

        // ---- ACTUALIZAR PONENTE ----
        // Ruta: PUT api/ponentes/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Ponente ponenteActualizado)
        {
            var ponente = await _context.Ponentes.FindAsync(id);

            if (ponente == null)
            {
                return BadRequest("Ponente no encontrado");
            }

            ponente.Nombre = ponenteActualizado.Nombre;
            ponente.Bio = ponenteActualizado.Bio;
            ponente.Foto = ponenteActualizado.Foto;
            ponente.Email = ponenteActualizado.Email;
            ponente.Especialidad = ponenteActualizado.Especialidad;

            await _context.SaveChangesAsync();

            return Ok(ponente);
        }

        // ---- ELIMINAR PONENTE ----
        // Ruta: DELETE api/ponentes/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var ponente = _context.Ponentes.Find(id);

            if (ponente == null)
            {
                return BadRequest("Ponente no encontrado");
            }

            _context.Ponentes.Remove(ponente);
            await _context.SaveChangesAsync();

            return Ok("Ponente eliminado correctamente");
        }
    }
}
