using FeContadoNew.Models;
using System.Threading.Tasks;

namespace FeContadoNew.InterfacesBusiness
{
    public interface IFeBusiness
    {
        Task<FacturaResponse> insertaEncabezado(Factura facturaRequest);
    }
}
