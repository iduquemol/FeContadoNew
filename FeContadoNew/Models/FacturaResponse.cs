using System;

namespace FeContadoNew.Models
{
    public class FacturaResponse
    {
        public string cod_error { get; set; }
        public string det_error { get; set; }
        public string estado { get; set; }
        public DateTime fecha_proceso { get; set; }
        public string numeroFactura { get; set; }
        public string NumeroOrdenFe { get; set; }
    }
}
