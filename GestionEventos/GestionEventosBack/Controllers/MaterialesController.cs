using GestionEventosBack.Data;
using GestionEventosModelo.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialesController : ControllerBase
    {
        private readonly DataContext _context;

        public MaterialesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var materiales = await _context.Materiales.ToListAsync();
            return Ok(materiales);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var material = await _context.Materiales.FirstOrDefaultAsync(m => m.Id_Material == id);

            if (material == null)
            {
                return NotFound("Material no encontrado");
            }

            return Ok(material);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Material material)
        {
            material.FechaSubida = DateTime.Now;

            _context.Materiales.Add(material);
            await _context.SaveChangesAsync();
            return Created("", material);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Material actualizado)
        {
            var material = await _context.Materiales.FindAsync(id);

            if (material == null)
            {
                return NotFound("Material no encontrado");
            }

            material.Nombre = actualizado.Nombre;
            material.Tipo = actualizado.Tipo;
            material.Url = actualizado.Url;

            await _context.SaveChangesAsync();
            return Ok(material);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var material = await _context.Materiales.FirstOrDefaultAsync(m => m.Id_Material == id);

            if (material == null)
            {
                return NotFound("Material no encontrado");
            }

            _context.Materiales.Remove(material);
            await _context.SaveChangesAsync();
            return Ok("Material eliminado correctamente");
        }
    }
}
