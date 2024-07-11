using ChatGS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatGS.Repositorios.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<UsuarioModel>
    {
        Task<UsuarioModel> GetByNomeAsync(string nome);
        Task<IEnumerable<UsuarioModel>> GetUsuariosByPartialNomeAsync(string partialNome);
    }
}
