using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Certificado
    {
        public int Id_Certificado {  get; set; }
        public int Id_Inscripcion { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string? UrlPDF { get; set; }
        public string CodigoValidacion { get; set; } = string.Empty;
        public Inscripcion Inscripcion { get; set; } = null!;
    }
}
