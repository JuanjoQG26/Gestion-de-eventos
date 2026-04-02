using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Pago
    {
        public int Id_Pago {  get; set; }
        public int Id_Inscripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? TransaccionId { get; set; }
        //UNO A UNO CON INSCRIPCION
        public Inscripcion Inscripcion { get; set; } = null!;
    }
}
