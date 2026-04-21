using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Asistencia
    {
        public int Id_Asistencia {  get; set; }
        public int Id_Inscripcion { get; set; }
        public int Id_Actividad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string MetodoRegistro { get; set; } = string.Empty;
        public Inscripcion? Inscripcion { get; set; } //= null!;
        public Actividad? Actividad { get; set; } //= null!;
    }
}
