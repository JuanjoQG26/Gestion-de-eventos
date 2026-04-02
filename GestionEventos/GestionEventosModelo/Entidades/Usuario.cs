using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Usuario
    {
        public int Id_Usuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena {  get; set; } = string.Empty;
        public string Rol {  get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        // Relación: un usuario puede crear muchos eventos (si es organizador)
        public List<Evento> Eventos { get; set; } = new();

        // Relación: un usuario puede tener muchas inscripciones (si es asistente)
        public List<Inscripcion> Inscripciones { get; set; } = new();
    }
}
