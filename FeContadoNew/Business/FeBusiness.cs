using FeContadoNew.Interfaces;
using FeContadoNew.InterfacesBusiness;
using FeContadoNew.Models;
using System.Threading.Tasks;

namespace FeContadoNew.Business
{
    public class FeBusiness : IFeBusiness
    {
        private readonly IFeRepository _FeRepository;

        public FeBusiness(IFeRepository feRepository)
        {
            _FeRepository = feRepository;
        }
        public async Task<FacturaResponse> insertaEncabezado(Factura facturaRequest)
        {
            return await _FeRepository.insertaEncabezado(facturaRequest).ConfigureAwait(false);
        }
    }
}
