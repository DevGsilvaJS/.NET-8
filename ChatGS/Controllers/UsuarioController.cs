using ChatGS.DTO;
using ChatGS.Services.Users;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuarioService.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario(UsuarioDTO usuario)
        {

            var createdUsuario = await _usuarioService.GravarUsuario(usuario);
            return Ok(createdUsuario);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            bool excluirUsuario = await _usuarioService.DeleteUsuario(id);
            return Ok(excluirUsuario);
        }

        [HttpGet("ListarUsuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ListAll();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use a logging framework)
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> BuscaUsuarioPorID(int id)
        //{
        //    var excluirUsuario = await _usuarioService.BuscaUsuarioPorID(id);
        //    return Ok(excluirUsuario);
        //}

        //[HttpPost("autenticacao")]
        //public async Task<IActionResult> Autenticacao(string email, string senha)
        //{
        //    // Verifica se o usuário e senha são válidos
        //    bool autenticado = await _usuarioService.Autenticacao(email, senha);

        //    if (autenticado)
        //    {
        //        // Retorna OK se autenticado
        //        return Ok(new { mensagem = "Usuário autenticado com sucesso." });
        //    }
        //    else
        //    {
        //        // Retorna Unauthorized se não autenticado
        //        return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });
        //    }
        //}
    }
}

