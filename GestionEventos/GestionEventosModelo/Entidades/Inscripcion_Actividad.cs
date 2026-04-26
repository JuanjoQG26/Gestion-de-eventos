using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Inscripcion_Actividad
    {
        public int Id { get; set; }
        public int Id_Inscripcion { get; set; }
        public int Id_Actividad { get; set; }
        public Inscripcion? Inscripcion { get; set; } //= null!;
        public Actividad? Actividad { get; set; } //= null!;
    }
}
