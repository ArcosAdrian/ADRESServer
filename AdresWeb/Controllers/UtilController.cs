using AdresLib;
using AdresLib.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace AdresWeb.Controllers
{
    [ApiController]
    [Route("Adres/Util/")]
    public class UtilController : ControllerBase
    {
        private readonly IAdresService AdresService;
        public UtilController(IAdresService adresService)
        {
            this.AdresService = adresService;
        }

        [HttpGet("Ping")]
        public String Ping()
        {
            return AdresService.Ping();
        }

        [HttpGet("Servicios")]
        public List<IdNombre> ServiciosListar()
        {
            return AdresService.ServiciosListar();
        }

        [HttpGet("Unidades")]
        public List<IdNombre> UnidadesListar()
        {
            return AdresService.UnidadesListar();
        }

        [HttpGet("Proveedores")]
        public List<IdNombre> ProveedoresListar()
        {
            return AdresService.ProveedoresListar();
        }

    }
}
