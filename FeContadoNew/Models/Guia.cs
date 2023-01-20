using System;

namespace FeContadoNew.Models
{
    public class Guia
    {
        public long NumeroGuia { get; set; }
        public DateTime FechaEdicion { get; set; }
        public Int16 SerIdServicio { get; set; }
        public string SerNombreServicio { get; set; }
        public string SerIdUnidadNegocio { get; set; }
        public Int16 SerPeso { get; set; }
        public decimal SerValorTransporte { get; set; }
        public decimal SerValorSeguro { get; set; }
        public decimal SerValorAdicionales { get; set; }
        public decimal SerValorTotal { get; set; }
        public string OriIdSucursal { get; set; }
        public string OriNombreSucursal { get; set; }
        public string OriIdCentroCostos { get; set; }
        public string OriNombreCentroCostos { get; set; }
        public decimal OriValor { get; set; }
        public string DesIdSucursal { get; set; }
        public string DesNombreSucursal { get; set; }
        public string DesIdCentroCostos { get; set; }
        public string DesNombreCentroCostos { get; set; }
        public decimal DesValor { get; set; }
    }
}
