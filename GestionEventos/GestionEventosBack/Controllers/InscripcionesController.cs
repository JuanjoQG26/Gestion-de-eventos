using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionEventosBack.Data;
using GestionEventosModelo.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionesController : ControllerBase
    {
        private readonly DataContext _context;

        public InscripcionesController(DataContext context)
        {
            _context = context;
        }

        //OBTENER TODAS LAS INSCRIPCIONES
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var inscripciones = await _context.Inscripciones.ToListAsync();
            return Ok(inscripciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var inscripcion = await _context.Inscripciones.FirstOrDefaultAsync(i => i.Id_Inscripcion == id);

            if (inscripcion == null)
            {
                return NotFound("Inscripcion no encontrada");
            }

            return Ok(inscripcion);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Inscripcion inscripcion)
        {
            var yainscrito = await _context.Inscripciones
                .AnyAsync(i => i.Id_Usuario == inscripcion.Id_Usuario
                && i.Id_Evento == inscripcion.Id_Evento
                && i.Estado != "Cancelada");

            if (yainscrito)
            {
                return BadRequest("Ya estas inscrito en este evento");
            }
            
            inscripcion.FechaInscripcion = DateTime.Now;
            
            //Console.WriteLine(inscripcion.FechaInscripcion);
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();
            return Created("", inscripcion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Inscripcion actualizado)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);

            if (inscripcion == null)
            {
                return NotFound("Inscripcion no encontrada");
            }

            

            inscripcion.Id_Usuario = actualizado.Id_Usuario;
            inscripcion.Id_Evento = actualizado.Id_Evento;
            inscripcion.FechaInscripcion = actualizado.FechaInscripcion;
            inscripcion.Estado = actualizado.Estado;

            await _context.SaveChangesAsync();
            return Ok(inscripcion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var inscripcion = await _context.Inscripciones.FirstOrDefaultAsync(i => i.Id_Inscripcion == id);

            if (inscripcion == null)
            {
                return NotFound("Inscripcion no encontrada");
            }

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();
            return Ok("Inscripcion eliminada correctamente");
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> ObtenerPorUsuario(int idUsuario)
        {
            var inscripciones = await _context.Inscripciones
                .Where(i => i.Id_Usuario == idUsuario)
                .ToListAsync();
            return Ok(inscripciones);
        }
    }
}
