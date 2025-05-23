using AdresLib;
using AdresLib.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace AdresWeb.Controllers
{
    [ApiController]
    [Route("Adres/Adquisicion/")]
    public class AdquisicionController : ControllerBase
    {
        private readonly IAdresService AdresService;
        public AdquisicionController(IAdresService adresService)
        {
            this.AdresService = adresService;
        }

        [HttpGet("Ping")]
        public String Ping()
        {
            return AdresService.Ping();
        }

        [HttpGet]
        public List<AdquisicionExtra> AdquisicionesListar()
        {
            return AdresService.AdquisicionesListar();
        }
        [HttpGet]
        [Route("{id:int}")]
        public List<AdquisicionExtra> AdquisicionesRecuperar(int id)
        {
            return AdresService.AdquisicionesListar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adquisicion"></param>
        [HttpPost]
        
        public void AdquisicionAdicionar([FromBody] Adquisicion adquisicion)
        {
            AdresService.AdquisicionAdicionar(adquisicion);
        }
        
        [HttpPut]
        [Route("{id:int}")]
        public void AdquisicionModificar(int id, [FromBody] Adquisicion adquisicion)
        {
            AdresService.AdquisicionModificar(adquisicion);
        }
        [HttpDelete]
        [Route("{id:long}")]
        public void AdquisicionesHistoriaListarPorId(long id)
        {
            AdresService.AdquisicionEliminar(id);
        }


        [HttpGet]
        [Route("Historia/{id:int}")]
        public List<AdquisicionHistoria> AdquisicionesHistoriaListarPorId(int id)
        {
            return AdresService.AdquisicionesHistoriaListarPorId(id);
        }

        

    }
}
