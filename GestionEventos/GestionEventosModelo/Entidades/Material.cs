using System;
using System.Collections.Generic;
using System.Text;

namespace GestionEventosModelo.Entidades
{
    public class Material
    {
        public int Id_Material {  get; set; }
        public int Id_Evento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Url {  get; set; } = string.Empty;
        public DateTime FechaSubida { get; set; }
        public Evento? Evento { get; set; } //= null!;
    }
}
