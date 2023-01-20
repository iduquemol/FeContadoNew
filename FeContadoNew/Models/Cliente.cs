namespace FeContadoNew.Models
{
    public class Cliente
    {
        public string IdTipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public byte TipoPersona { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string IdCiudad { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
    }
}
