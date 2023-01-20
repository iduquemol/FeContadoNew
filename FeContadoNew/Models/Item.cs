using System;

namespace FeContadoNew.Models
{
    public class Item
    {
        public Int16 regOrdenFE { get; set; }
        public Int16 IdItem { get; set; }
        public string DescripcionItem { get; set; }
        public string IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
        public string IdCentroCostos { get; set; }
        public string NombreCentroCostos { get; set; }
        public string IdProcedimiento { get; set; }
        public string NombreProcedimiento { get; set; }
        public string IdLineaServicio { get; set; }
        public string NombreLineaServicio { get; set; }
        public string IdUnidadNegocio { get; set; }
        public string NombreUnidadNegocio { get; set; }
        public decimal Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ValorDescuento { get; set; }
        public decimal BaseIva { get; set; }
        public decimal PorcentajeIva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal ValorTotal { get; set; }
        public Guia Guia { get; set; }
    }
}
