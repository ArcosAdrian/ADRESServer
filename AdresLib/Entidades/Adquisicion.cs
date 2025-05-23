using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdresLib.Entidades
{
    public class Adquisicion
    {
        public long? Id { get; set; }
        public long ProveedorId { get; set; }
        public long ServicioId { get; set; }
        public long UnidadId { get; set; }
        public double Presupuesto { get; set; }
        public long Cantidad { get; set; }
        public double ValorUnitario { get; set; }
        public string FechaAdquisicion { get; set; }
        public string Comentario { get; set; }
    }
    public class AdquisicionExtra : Adquisicion
    {
        public string ProveedorNombre { get; set; }
        public string ServicioNombre { get; set; }
        public string UnidadNombre { get; set; }
    }
    public class AdquisicionHistoria
    {
        public long? Id { get; set; }
        
        public long? AdquisicionId { get; set; }
        public long? ProveedorId { get; set; }
        public long? ServicioId { get; set; }
        public long? UnidadId { get; set; }
        public double? Presupuesto { get; set; }
        public long? Cantidad { get; set; }
        public double? ValorUnitario { get; set; }
        public string FechaAdquisicion { get; set; }
        public string Comentario { get; set; }
        public string FechaCambio { get; set; }
        public string UsuarioCambio { get; set; }
        public string TipoCambio { get; set; }
        public string Cambios { get; set; }
    }

}
