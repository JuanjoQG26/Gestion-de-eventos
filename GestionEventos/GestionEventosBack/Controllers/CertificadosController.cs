using GestionEventosBack.Data;
using GestionEventosModelo.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificadosController : ControllerBase
    {
        private readonly DataContext _context;

        public CertificadosController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var certificados = await _context.Certificados.ToListAsync();
            return Ok(certificados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var certificado = await _context.Certificados.FirstOrDefaultAsync(c => c.Id_Certificado == id);

            if (certificado == null)
            {
                return NotFound("Certificado no encontrado");
            }
            return Ok(certificado);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] Certificado certificado)
        {
            certificado.FechaGeneracion = DateTime.Now;

            certificado.CodigoValidacion = Guid.NewGuid().ToString();

            _context.Certificados.Add(certificado);
            await _context.SaveChangesAsync();
            return Created("", certificado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Certificado actualizado)
        {
            var certificado = await _context.Certificados.FindAsync(id);
            if (certificado == null)
            {
                return NotFound("Certificado no encontrado");
            }

            certificado.Id_Inscripcion = actualizado.Id_Inscripcion;
            certificado.UrlPDF = actualizado.UrlPDF;
            certificado.CodigoValidacion = actualizado.CodigoValidacion;
            await _context.SaveChangesAsync();
            return Ok(certificado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var certificado = await _context.Certificados.FirstOrDefaultAsync(c => c.Id_Certificado == id);

            if (certificado == null)
            {
                return NotFound("Certificado no encontrado");
            }

            _context.Certificados.Remove(certificado);
            await _context.SaveChangesAsync();
            return Ok("Certificado eliminado correctamente");
        }
    }
}
