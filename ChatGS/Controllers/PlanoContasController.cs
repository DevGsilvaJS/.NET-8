using ChatGS.DTO;
using ChatGS.Models.Transactions;
using ChatGS.Services.Transactions;
using ChatGS.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatGS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoContasController : ControllerBase
    {

        private readonly PlanoContasServices _planoContasServices;

        public PlanoContasController(PlanoContasServices planoContasServices)
        {
            _planoContasServices = planoContasServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario(PlanoContasModel planoContas)
        {

            var savePlanoContas = await _planoContasServices.GravarPlanoContas(planoContas);
            return Ok(savePlanoContas);
        }
    }
}
