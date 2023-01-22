using FeContadoNew.Interfaces;
using FeContadoNew.Shared;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FeContadoNew.Models
{
    public class FeRepository : IFeRepository
    {
        private DataMapper mapperSP = DataMapper.Instancia;
        private readonly IConfiguration _configuration;

        public FeRepository(IConfiguration configuration)
        {
            //_context = context;
            _configuration = configuration;
        }
        public async Task<FacturaResponse> insertaEncabezado(Factura facturaRequest)
        {
            FacturaResponse itemResponse = new FacturaResponse();
            SqlParameterCollection collection = new SqlCommand().Parameters;
            collection.Add(new SqlParameter("@NumeroOrdenFe", facturaRequest.NumeroOrdenFe));
            collection.Add(new SqlParameter("@FechaVenta", facturaRequest.FechaVenta));
            collection.Add(new SqlParameter("@IdCiudadVenta", facturaRequest.IdCiudadVenta));
            collection.Add(new SqlParameter("@IdFormaPago", facturaRequest.IdFormaPago));
            collection.Add(new SqlParameter("@NombreFormaPago", facturaRequest.NombreFormaPago));
            collection.Add(new SqlParameter("@CliTipIdentificacion", facturaRequest.Cliente.IdTipoIdentificacion));
            collection.Add(new SqlParameter("@CliNumIdentificacion", facturaRequest.Cliente.NumeroIdentificacion));
            collection.Add(new SqlParameter("@CliTipoPersona", facturaRequest.Cliente.TipoPersona));
            collection.Add(new SqlParameter("@CliPrimerNombre", facturaRequest.Cliente.PrimerNombre));
            collection.Add(new SqlParameter("@CliSegundoNombre", facturaRequest.Cliente.SegundoNombre));
            collection.Add(new SqlParameter("@CliPrimerApellido", facturaRequest.Cliente.PrimerApellido));
            collection.Add(new SqlParameter("@CliSegundoApellido", facturaRequest.Cliente.SegundoApellido));
            collection.Add(new SqlParameter("@CliRazonSocial", facturaRequest.Cliente.RazonSocial));
            collection.Add(new SqlParameter("@CliDireccion", facturaRequest.Cliente.Direccion));
            collection.Add(new SqlParameter("@CliIdCiudad", facturaRequest.Cliente.IdCiudad));
            collection.Add(new SqlParameter("@CliTelefono", facturaRequest.Cliente.Telefono));
            collection.Add(new SqlParameter("@CliCorreoElectronico", facturaRequest.Cliente.CorreoElectronico));

            String connectionSt = mapperSP.getConnectionString(_configuration);
            String connectionNova = mapperSP.getConnectionStringNOVA(_configuration);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionNova))
                {
                    connection.Open();
                    var readerasync = mapperSP.ExecuteProcedure("usr_sp_itq_insert_CabFEContado", ExecuteType.ExecuteReader, connection, collection).ConfigureAwait(false);

                    SqlDataReader reader = (SqlDataReader)readerasync.GetAwaiter().GetResult();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            itemResponse.estado = reader["estado"].ToString();
                            //itemResponse.tipoOrdenCXP = reader["tipoOrdenCXP"].ToString();
                            //itemResponse.numeroOrdenCXP = reader["numeroOrdenCXP"].ToString();                            
                        }
                    }
                    if (itemResponse.estado == "VO")
                    {
                        foreach (Item item in facturaRequest.Items)
                        {
                            SqlParameterCollection collectionDet = new SqlCommand().Parameters;
                            collectionDet.Add(new SqlParameter("@NumeroOrdenFe", facturaRequest.NumeroOrdenFe));
                            collectionDet.Add(new SqlParameter("@RegOrdenFE", item.regOrdenFE));
                            collectionDet.Add(new SqlParameter("@IdItem", item.IdItem));
                            collectionDet.Add(new SqlParameter("@DescripcionItem", item.DescripcionItem));
                            collectionDet.Add(new SqlParameter("@IdSucursal", item.IdSucursal));
                            collectionDet.Add(new SqlParameter("@NombreSucursal", item.NombreSucursal));
                            collectionDet.Add(new SqlParameter("@IdCentroCostos", item.IdCentroCostos));
                            collectionDet.Add(new SqlParameter("@NombreCentroCostos", item.NombreCentroCostos));
                            collectionDet.Add(new SqlParameter("@IdProcedimiento", item.IdProcedimiento));
                            collectionDet.Add(new SqlParameter("@NombreProcedimiento", item.NombreProcedimiento));
                            collectionDet.Add(new SqlParameter("@IdLineaServicio", item.IdLineaServicio));
                            collectionDet.Add(new SqlParameter("@NombreLineaServicio", item.NombreLineaServicio));
                            collectionDet.Add(new SqlParameter("@IdUnidadNegocio", item.IdUnidadNegocio));
                            collectionDet.Add(new SqlParameter("@NombreUnidadNegocio", item.NombreUnidadNegocio));
                            collectionDet.Add(new SqlParameter("@Cantidad", item.Cantidad));
                            collectionDet.Add(new SqlParameter("@ValorUnitario", item.ValorUnitario));
                            collectionDet.Add(new SqlParameter("@ValorDescuento", item.ValorDescuento));
                            collectionDet.Add(new SqlParameter("@BaseIva", item.BaseIva));
                            collectionDet.Add(new SqlParameter("@PorcentajeIva", item.PorcentajeIva));
                            collectionDet.Add(new SqlParameter("@ValorIva", item.ValorIva));
                            collectionDet.Add(new SqlParameter("@ValorTotal", item.ValorTotal));
                            collectionDet.Add(new SqlParameter("@NumeroGuia", item.Guia.NumeroGuia));
                            collectionDet.Add(new SqlParameter("@FechaEdicion", item.Guia.FechaEdicion));
                            collectionDet.Add(new SqlParameter("@SerIdServicio", item.Guia.SerIdServicio));
                            collectionDet.Add(new SqlParameter("@SerNombreServicio", item.Guia.SerNombreServicio));
                            collectionDet.Add(new SqlParameter("@SerIdUnidadNegocio", item.Guia.SerIdUnidadNegocio));
                            collectionDet.Add(new SqlParameter("@SerPeso ", item.Guia.SerPeso));
                            collectionDet.Add(new SqlParameter("@SerValorTransporte", item.Guia.SerValorTransporte));
                            collectionDet.Add(new SqlParameter("@SerValorSeguro", item.Guia.SerValorSeguro));
                            collectionDet.Add(new SqlParameter("@SerValorAdicionales", item.Guia.SerValorAdicionales));
                            collectionDet.Add(new SqlParameter("@SerValorTotal", item.Guia.SerValorTotal));
                            collectionDet.Add(new SqlParameter("@OriIdSucursal", item.Guia.OriIdSucursal));
                            collectionDet.Add(new SqlParameter("@OriNombreSucursal", item.Guia.OriNombreSucursal));
                            collectionDet.Add(new SqlParameter("@OriIdCentroCostos", item.Guia.OriIdCentroCostos));
                            collectionDet.Add(new SqlParameter("@OriNombreCentroCostos", item.Guia.OriNombreCentroCostos));
                            collectionDet.Add(new SqlParameter("@OriValor", item.Guia.OriValor));
                            collectionDet.Add(new SqlParameter("@DesIdSucursal ", item.Guia.DesIdSucursal));
                            collectionDet.Add(new SqlParameter("@DesNombreSucursal", item.Guia.DesNombreSucursal));
                            collectionDet.Add(new SqlParameter("@DesIdCentroCostos", item.Guia.DesIdCentroCostos));
                            collectionDet.Add(new SqlParameter("@DesNombreCentroCostos", item.Guia.DesNombreCentroCostos));
                            collectionDet.Add(new SqlParameter("@DesValor", item.Guia.DesNombreCentroCostos));

                            var readerDetasync = mapperSP.ExecuteProcedure("usr_sp_itq_insert_DetFEContado", ExecuteType.ExecuteReader, connection, collectionDet).ConfigureAwait(false);

                            SqlDataReader readerDet = (SqlDataReader)readerDetasync.GetAwaiter().GetResult();

                            if (readerDet.HasRows)
                            {
                                while (readerDet.Read())
                                {
                                    itemResponse.estado = reader["estado"].ToString(); 
                                    //itemResponse.tipoOrdenCXP = readerDet["tipoOrdenCXP"].ToString();
                                    //itemResponse.numeroOrdenCXP = readerDet["numeroOrdenCXP"].ToString();
                                }
                            }
                        }
                    }
                }
                return itemResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
