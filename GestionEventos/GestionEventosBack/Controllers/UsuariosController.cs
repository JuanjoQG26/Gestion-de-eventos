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
    public class UsuariosController : ControllerBase
    {
        // Variable que nos permite acceder a la base de datos
        private readonly DataContext _context;

        // Constructor: recibe el DataContext por inyección de dependencias
        public UsuariosController(DataContext context)
        {
            _context = context;
        }

        // ---- LOGIN ----
        // Ruta: POST api/usuarios/login
        // Recibe email y contraseña y verifica si el usuario existe
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            // Busca el usuario en la BD por email y contraseña
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email
                    && u.Contrasena == login.Contrasena);
            if (usuario == null)
            {
                return Unauthorized("Email o contraseña incorrectos");
            }
            return Ok(usuario);
        }


        // ---- REGISTRO ----
        // Ruta: POST api/usuarios/registro
        // Recibe los datos del nuevo usuario y lo guarda en la BD
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] Usuario usuario)
        {
            // Verifica si el email ya está registrado
            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email == usuario.Email);

            // Si ya existe retorna error 400 (solicitud incorrecta)
            if (existe)
            {
                return BadRequest("El email ya esta registrado.");
            }
            //Asigna la fecha de registro actual
            usuario.FechaRegistro = DateTime.Now;

            // Agrega el usuario a la BD
            _context.Add(usuario);

            await _context.SaveChangesAsync();

            return Created("", usuario);
        }

        // ---- OBTENER TODOS LOS USUARIOS ----
        // Ruta: GET api/usuarios
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        // DTO (Data Transfer Object): clase simple para recibir el login
        // Solo tiene email y contraseña, no toda la entidad Usuario
        public class LoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Contrasena {  get; set; } = string.Empty;
        }
    }
}
