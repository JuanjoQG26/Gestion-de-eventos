using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Evento
    {
        public int Id_Evento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion {  get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin {  get; set; }
        public string Lugar { get; set; } = string.Empty;
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }
        public int CupoTotal { get; set; }
        public string Estado { get; set; } = string.Empty;

        //FK ID ORGANIZADOR
        public int Id_Organizador { get; set; }

        // Relación: el organizador que creó este evento
        public Usuario? Organizador { get; set; } /*= null!;*/

        // Relación: un evento tiene muchas actividades
        public List<Actividad> Actividades { get; set; } = new();

        // Relación: un evento tiene muchas inscripciones
        public List<Inscripcion> Inscripciones { get; set; } = new();

        // Relación: un evento tiene muchos materiales
        public List<Material> Materiales { get; set; } = new();
    }
}
