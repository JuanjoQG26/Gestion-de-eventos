using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionEventosBack.Data;
using GestionEventosBack.Pagos;
using GestionEventosModelo.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using PayPalCheckoutSdk.Orders;

namespace GestionEventosBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly PayPalServicio _paypal;

        public PayPalController(DataContext context, PayPalServicio paypal)
        {
            _context = context;
            _paypal = paypal;
        }

        public class CrearOrdenDto
        {
            public int IdInscripcion { get; set; }
        }

        // ---- CREAR ORDEN DE PAGO ----
        // Ruta: POST api/paypal/crear-orden
        // Recibe el ID de inscripción y crea una orden en PayPal
        [HttpPost("crear-orden")]
        public async Task<IActionResult> CrearOrden([FromBody] CrearOrdenDto dto)
        {
            var inscripcion = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.Id_Inscripcion == dto.IdInscripcion);
            if (inscripcion == null)
            {
                return NotFound("Inscripion no encontrada");
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(e => e.Id_Evento == inscripcion.Id_Evento);
            if (evento == null)
            {
                return NotFound("Evento no encontrado");
            }

            var (orderId, approvalUrl) = await _paypal.CrearOrden(
                evento.Precio,
                $"http://localhost:5259/api/paypal/exito?idInscripcion={dto.IdInscripcion}",
                "http://localhost:5259/api/paypal/cancelado"
                );
            return Ok(new { orderId, approvalUrl});
        }

        // ---- CAPTURAR PAGO EXITOSO ----
        // Ruta: GET api/paypal/exito
        // PayPal redirige aquí después de que el usuario aprueba el pago
        [HttpGet("exito")]
        public async Task<IActionResult> Exito([FromQuery] string token, [FromQuery] string PayerID, [FromQuery] int idInscripcion)
        {
            // Captura el pago en PayPal
            var exitoso = await _paypal.CapturarPago(token);

            if (!exitoso)
            {
                return BadRequest("No se pudo capturar el pago");
            }

            // Busca la inscripción y la confirma
            var inscripcion = await _context.Inscripciones
                .FirstOrDefaultAsync(i => i.Id_Inscripcion == idInscripcion);

            if (inscripcion != null)
            {
                inscripcion.Estado = "Confirmada";
                await _context.SaveChangesAsync();

                // Crea el registro de pago
                var pago = new Pago
                {
                    Id_Inscripcion = idInscripcion,
                    Monto = 0,
                    FechaPago = DateTime.Now,
                    MetodoPago = "PayPal",
                    Estado = "Confirmado",
                    TransaccionId = token
                };
                _context.Pagos.Add(pago);
                await _context.SaveChangesAsync();
            }
            return Redirect("https://localhost:7009/pago-exitoso");
        }

        // ---- PAGO CANCELADO ----
        // Ruta: GET api/paypal/cancelado
        // PayPal redirige aquí si el usuario cancela
        [HttpGet("cancelado")]
        public IActionResult Cancelado()
        {
            return Redirect("https://localhost_7009/pago-cancelado");
        }
    }
}
