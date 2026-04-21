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
    public class PagosController : ControllerBase
    {
        private readonly DataContext _context;

        public PagosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var pagos = await _context.Pagos.ToListAsync();



            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id_Pago == id);

            if (pago == null)
            {
                return NotFound("Pago no encontrado");
            }

            return Ok(pago);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Pago pago)
        {
            pago.FechaPago = DateTime.Now;

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();
            return Created("", pago);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Pago actualizado)
        {
            var pago = await _context.Pagos.FindAsync(id);

            if (pago == null)
            {
                return NotFound("Pago no encontrado");
            }

            pago.Monto = actualizado.Monto;
            pago.MetodoPago = actualizado.MetodoPago;
            pago.Estado = actualizado.Estado;
            pago.TransaccionId = actualizado.TransaccionId;

            await _context.SaveChangesAsync();
            return Ok(pago);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var pago = await _context.Pagos.FirstOrDefaultAsync(p => p.Id_Pago == id);

            if (pago == null)
            {
                return NotFound("Pago no encontrado");
            }

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();
            return Ok("Pago eliminado con exito");
        }
    }
}
