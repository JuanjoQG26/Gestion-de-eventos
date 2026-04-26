using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Inscripcion
    {
        public int Id_Inscripcion { get; set; }
        public int Id_Usuario { get; set; }
        public int Id_Evento { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; } //= null!;
        public Evento? Evento { get; set; } //= null!;
        //UNO A UNO INSCRIPCION - PAGO
        public Pago? Pago { get; set; }
        public List<Asistencia> Asistencias { get; set; } = new();
        public List<Inscripcion_Actividad> Inscripciones_Actividades { get; set; } = new();
        public Certificado? Certificado { get; set; }
    }
}
