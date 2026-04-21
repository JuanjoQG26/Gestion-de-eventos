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
    public class ActividadesController : ControllerBase
    {
        private readonly DataContext _context;

        public ActividadesController(DataContext context)
        {
            _context = context;
        }

        // ---- OBTENER TODOS LAS ACTIVIDADES ----
        // Ruta: GET api/actividades
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var actividades = await _context.Actividades.ToListAsync();
            return Ok(actividades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var actividad = await _context.Actividades.FirstOrDefaultAsync(a => a.Id_Actividad == id);

            if (actividad == null)
            {
                return NotFound("Actividad no encontrada");
            }

            return Ok(actividad);
        }

        // ---- CREAR ACTIVIDAD ----
        // Ruta: POST api/actividad
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Actividad actividad)
        {
            /*if (actividad.HoraInicio < actividad.HoraFin)
            {
                return BadRequest("La hora inicio tiene que ser menor a la hora de fin");
            }*/

            if (actividad.Cupo < 0)
            {
                return BadRequest("El cupo tiene que ser mayor a 0");
            }

            _context.Actividades.Add(actividad);
            await _context.SaveChangesAsync();
            return Created("", actividad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Actividad actualizado)
        {
            var actividad = await _context.Actividades.FindAsync(id);

            if (actividad == null)
            {
                return NotFound("Actividad no encontrada");
            }

            actividad.Id_Evento = actualizado.Id_Evento;
            actividad.Id_Ponente = actualizado.Id_Ponente;
            actividad.Titulo = actualizado.Titulo;
            actividad.Descripcion = actualizado.Descripcion;
            actividad.HoraInicio = actualizado.HoraInicio;
            actividad.HoraFin = actualizado.HoraFin;
            actividad.Cupo = actualizado.Cupo;
            actividad.Tipo = actualizado.Tipo;

            await _context.SaveChangesAsync();
            return Ok(actividad);
        }

        // ---- ELIMINAR ACTIVIDAD ----
        // Ruta: DELETE api/actividades/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var actividad = await _context.Actividades.FirstOrDefaultAsync(a => a.Id_Actividad == id);

            if (actividad == null)
            {
                return NotFound("Actividad no encontrada");
            }

            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();
            return Ok("Actividad eliminada correctamente");
        }
    }
}
