using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Ponente
    {
        public int Id_Ponente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Bio {  get; set; }

        public string? Foto { get; set; }
        public string? Email {  get; set; }
        public string? Especialidad { get; set; }
        public List<Actividad> Actividades { get; set; } = new();
    }
}
