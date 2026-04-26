using GestionEventosBack.Data;
using GestionEventosModelo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionActividadesController : ControllerBase
    {
        private readonly DataContext _context;

        public InscripcionActividadesController(DataContext context)
        {
            _context = context;
        }

        // ---- OBTENER TODAS ----
        // Ruta: GET api/inscripcionactividades
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var lista = await _context.Inscripciones_Actividades.ToListAsync();
            return Ok(lista);
        }

        // ---- OBTENER POR ID ----
        // Ruta: GET api/inscripcionactividades/1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var item = await _context.Inscripciones_Actividades
                .FirstOrDefaultAsync(ia => ia.Id == id);

            if (item == null)
                return NotFound("Registro no encontrado.");

            return Ok(item);
        }

        // ---- OBTENER ACTIVIDADES DE UNA INSCRIPCIÓN ----
        // Ruta: GET api/inscripcionactividades/inscripcion/1
        // Devuelve todas las actividades que seleccionó una inscripción específica
        [HttpGet("inscripcion/{idInscripcion}")]
        public async Task<IActionResult> ObtenerPorInscripcion(int idInscripcion)
        {
            var lista = await _context.Inscripciones_Actividades
                .Where(ia => ia.Id_Inscripcion == idInscripcion)
                .ToListAsync();

            return Ok(lista);
        }

        // ---- OBTENER INSCRITOS EN UNA ACTIVIDAD ----
        // Ruta: GET api/inscripcionactividades/actividad/1
        // Devuelve todas las inscripciones que seleccionaron una actividad específica
        [HttpGet("actividad/{idActividad}")]
        public async Task<IActionResult> ObtenerPorActividad(int idActividad)
        {
            var lista = await _context.Inscripciones_Actividades
                .Where(ia => ia.Id_Actividad == idActividad)
                .ToListAsync();

            return Ok(lista);
        }

        // ---- CREAR ----
        // Ruta: POST api/inscripcionactividades
        // Asigna una actividad a una inscripción
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Inscripcion_Actividad inscripcionActividad)
        {
            // Verifica que la inscripción exista
            var inscripcionExiste = await _context.Inscripciones
                .AnyAsync(i => i.Id_Inscripcion == inscripcionActividad.Id_Inscripcion);

            if (!inscripcionExiste)
                return BadRequest("La inscripción no existe.");

            // Verifica que la actividad exista
            var actividadExiste = await _context.Actividades
                .AnyAsync(a => a.Id_Actividad == inscripcionActividad.Id_Actividad);

            if (!actividadExiste)
                return BadRequest("La actividad no existe.");

            // Verifica que no esté ya registrada esa combinación
            var yaExiste = await _context.Inscripciones_Actividades
                .AnyAsync(ia => ia.Id_Inscripcion == inscripcionActividad.Id_Inscripcion
                             && ia.Id_Actividad == inscripcionActividad.Id_Actividad);

            if (yaExiste)
                return BadRequest("Esta actividad ya está asignada a esta inscripción.");

            _context.Inscripciones_Actividades.Add(inscripcionActividad);
            await _context.SaveChangesAsync();
            return Created("", inscripcionActividad);
        }

        // ---- ELIMINAR ----
        // Ruta: DELETE api/inscripcionactividades/1
        // Elimina la asignación de una actividad a una inscripción
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var item = await _context.Inscripciones_Actividades
                .FirstOrDefaultAsync(ia => ia.Id == id);

            if (item == null)
                return NotFound("Registro no encontrado.");

            _context.Inscripciones_Actividades.Remove(item);
            await _context.SaveChangesAsync();
            return Ok("Registro eliminado correctamente.");
        }
    }
}