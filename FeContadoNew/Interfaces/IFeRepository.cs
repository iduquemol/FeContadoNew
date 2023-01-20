using FeContadoNew.Models;
using System.Threading.Tasks;

namespace FeContadoNew.Interfaces
{
    public interface IFeRepository
    {
        Task<FacturaResponse> insertaEncabezado(Factura facturaRequest);
    }
}
