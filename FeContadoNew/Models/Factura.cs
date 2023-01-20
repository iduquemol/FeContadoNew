using System;

namespace FeContadoNew.Models
{
    public class Factura
    {
        public long NumeroOrdenFe { get; set; }
        public DateTime FechaVenta { get; set; }
        public string IdCiudadVenta { get; set; }
        public int IdFormaPago { get; set; }
        public string NombreFormaPago { get; set; }
        public Cliente Cliente { get; set; }
        public Item[] Items { get; set; }
    }
}
