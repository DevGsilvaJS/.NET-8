using ChatGS.Models;
using ChatGS.Repositorios.Interfaces;
using ChatGS.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var usuarios = await _usuarioService.GetAllAsync();
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
        public async Task<IActionResult> Create(UsuarioModel usuario)
        {
            var createdUsuario = await _usuarioService.SaveEntitiesAsync(usuario);
            return Ok(createdUsuario);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, UsuarioModel usuario)
        //{
        //    if (id != usuario.Id)
        //    {
        //        return BadRequest();
        //    }

        //    await _usuarioRepository.UpdateAsync(usuario);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _usuarioRepository.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}
