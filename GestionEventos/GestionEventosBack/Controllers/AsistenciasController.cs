using GestionEventosBack.Data;
using GestionEventosModelo.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly DataContext _context;

        public AsistenciasController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var asistencias = await _context.Asistencias.ToListAsync();
            return Ok(asistencias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var asistencia = await _context.Asistencias.FirstOrDefaultAsync(a => a.Id_Asistencia == id);

            if (asistencia == null)
            {
                return NotFound("Asistencia no encontrada");
            }

            return Ok(asistencia);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Asistencia asistencia)
        {
            asistencia.FechaRegistro = DateTime.Now;

            _context.Asistencias.Add(asistencia);
            await _context.SaveChangesAsync();
            return Created("", asistencia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Asistencia actualizado)
        {
            var asistencia = await _context.Asistencias.FindAsync(id);

            if (asistencia == null)
            {
                return NotFound("Asistencia no encontrada");
            }

            asistencia.Id_Inscripcion = actualizado.Id_Inscripcion;
            asistencia.Id_Actividad = actualizado.Id_Actividad;
            asistencia.MetodoRegistro = actualizado.MetodoRegistro;

            await _context.SaveChangesAsync();
            return Ok(asistencia);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var asistencia = await _context.Asistencias.FirstOrDefaultAsync(a => a.Id_Asistencia == id);

            if (asistencia == null)
            {
                return NotFound("Asistencia no encontrada");
            }

            _context.Asistencias.Remove(asistencia);
            await _context.SaveChangesAsync();
            return Ok("Asistencia eliminada correctamente");
        }
    }
}
