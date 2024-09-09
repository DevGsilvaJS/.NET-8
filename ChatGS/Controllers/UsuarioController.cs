using ChatGS.DTO;
using ChatGS.Servicos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatGS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var usuarios = await _usuarioService.ListaUsuarios();
        //    return Ok(usuarios);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var usuario = await _usuarioService.GetByIdAsync(id);
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(usuario);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioDTO usuario)
        {

            var createdUsuario = await _usuarioService.GravarUsuario(usuario);
            return Ok(createdUsuario);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            bool excluirUsuario = await _usuarioService.DeletarUsuario(id);
            return Ok(excluirUsuario);
        }

        [HttpGet("ListarUsuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ListarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use a logging framework)
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
