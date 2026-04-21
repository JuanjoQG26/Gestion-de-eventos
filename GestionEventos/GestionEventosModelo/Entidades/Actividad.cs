using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Actividad
    {
        public int Id_Actividad { get; set; }
        public int Id_Evento { get; set; }
        public int? Id_Ponente { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin {  get; set; }
        public int Cupo { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public Evento? Evento { get; set; } //= null!;
        public Ponente? Ponente { get; set; }
        public List<Asistencia> Asistencias { get; set; } = new();
        public List<InscripcionActividad> InscripcionActividades { get; set; } = new();
    }
}
