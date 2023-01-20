using FeContadoNew.InterfacesBusiness;
using FeContadoNew.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeContadoNew.Controllers
{
    

    [ApiController]
    
    public class FeController : ControllerBase
    {
        private readonly IFeBusiness _feBusiness;

        public FeController(IFeBusiness feBusiness)
        {
            //_mapper = mapper;
            _feBusiness = feBusiness;
        }
        [HttpGet]
        [Route("api/fe/test")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("api/fe/recibeorden")]
        [ProducesResponseType(typeof(FacturaResponse), 200)]
        public async Task<IActionResult> Post([FromBody] Factura facturaRequest)
        {
            FacturaResponse responseModel = await _feBusiness.insertaEncabezado(facturaRequest);
            return Ok(responseModel);
        }
    }
}
