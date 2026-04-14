using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionEventosBack.Data;
using GestionEventosModelo.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly DataContext _context;

        public EventosController(DataContext context)
        {
            _context = context;
        }

        // ---- OBTENER TODOS LOS EVENTOS ----
        // Ruta: GET api/eventos
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            // Incluye el organizador de cada evento
            /*var eventos = await _context.Eventos
                .Include(e => e.Organizador)
                .ToListAsync();*/

            var eventos = await _context.Eventos.ToListAsync();

            return Ok(eventos);
        }

        // ---- OBTENER EVENTOS PUBLICADOS ----
        // Ruta: GET api/eventos/publicados
        // Solo devuelve eventos con estado "Publicado"
        [HttpGet("publicados")]
        public async Task<IActionResult> ObtenerPublicados()
        {
            var eventos = await _context.Eventos
                .Where(e => e.Estado == "Publicado").ToListAsync();
            /*.Include(e => e.Organizador)
            .ToListAsync();*/

            return Ok(eventos);
        }

        // ---- OBTENER UN EVENTO POR ID ----
        // Ruta: GET api/eventos/1
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            // Incluye actividades, ponentes y materiales del evento
            var evento = await _context.Eventos
                //.Include(e => e.Organizador)
                .Include(e => e.Actividades)
                    .ThenInclude(a => a.Ponente)
                .Include(e => e.Materiales)
                .FirstOrDefaultAsync(e => e.Id_Evento == id);

            if (evento == null)
            {
                return NotFound("Evento no encontrado");
            }
            return Ok(evento);
        }

        // ---- CREAR EVENTO ----
        // Ruta: POST api/eventos
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Evento evento)
        {
            // Valida que la fecha de inicio no sea anterior a hoy (RN-009)
            if (evento.FechaInicio < DateTime.Now)
            {
                return BadRequest("La fecha de inicio no puede ser anterior a la fecha de inicio");
            }

            // Valida que el precio sea mayor o igual a 0 (RN-008)
            if (evento.Precio < 0)
            {
                return BadRequest("El precio no puede ser negativo");
            }

            // Por defecto el evento se crea en borrador
            evento.Estado = "Borrador";

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return Created("", evento);
        }

        // ---- PUBLICAR EVENTO ----
        // Ruta: PUT api/eventos/1/publicar
        // Cambia el estado del evento a "Publicado"
        [HttpPut("{id}/publicar")]
        public async Task<IActionResult> Publicar(int id)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id_Evento == id);
                /*.Include(e => e.Actividades)
                .FirstOrDefaultAsync(e => e.Id_Evento == id);*/

            if (evento == null)
            {
                return NotFound("Evento no encontrado");
            }
            
            // Valida que tenga al menos una actividad (RN-006)
            /*if (!evento.Actividades.Any())
            {
                return BadRequest("Complete toda la informacion antes de publicar");
            }*/

            evento.Estado = "Publicado";
            await _context.SaveChangesAsync();

            return Ok(evento);
        }

        // ---- ACTUALIZAR EVENTO ----
        // Ruta: PUT api/eventos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Evento eventoActualizado)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound("Evento no encontrado");
            }

            // Actualiza solo los campos permitidos
            evento.Nombre = eventoActualizado.Nombre;
            evento.Descripcion = eventoActualizado.Descripcion;
            evento.FechaInicio = eventoActualizado.FechaInicio;
            evento.FechaFin = eventoActualizado.FechaFin;
            evento.Lugar = eventoActualizado.Lugar;
            evento.Precio = eventoActualizado.Precio;
            evento.CupoTotal = eventoActualizado.CupoTotal;

            await _context.SaveChangesAsync();

            return Ok(evento);
        }

        // ---- ELIMINAR EVENTO ----
        // Ruta: DELETE api/eventos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var evento = await _context.Eventos
                .Include(e => e.Inscripciones)
                    .ThenInclude(i => i.Pago)
                .FirstOrDefaultAsync(e => e.Id_Evento == id);
            if (evento == null)
            {
                return NotFound("Evento no encontrado");
            }

            bool tieneInscritosConfirmados = evento.Inscripciones.Any(i => i.Estado == "Confirmada");

            if (tieneInscritosConfirmados)
            {
                return BadRequest("No se puede eliminar un evento que ya tiene inscritos confirmados");
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return Ok("Evento eliminado correctamente");
        }
    }
}
