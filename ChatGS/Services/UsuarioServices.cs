using ChatGS.Models;
using ChatGS.Repositorios.Interfaces;
using ChatGS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatGS.Servicos
{
    public class UsuarioService : BaseLista
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<UsuarioModel> SaveEntitiesAsync(UsuarioModel usuario)
        {



           
            AddListaSalvar(usuario);
            AddListaSalvar(usuario.Pessoa);

            // Obtém a lista de entidades a serem salvas
            List<object> listaSalvar = GetListaSalvar();

        ;
            await _usuarioRepository.SaveTransaction(listaSalvar);


            
            return usuario;
        }
    }
}
