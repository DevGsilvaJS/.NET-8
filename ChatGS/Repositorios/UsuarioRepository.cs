using ChatGS.Data;
using ChatGS.Models;
using ChatGS.Repositorios;
using ChatGS.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UsuarioRepository : GenericRepository<UsuarioModel>, IUsuarioRepository
{



    private readonly ChatGSDbContext _context;

    public UsuarioRepository(ChatGSDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UsuarioModel> GetByNomeAsync(string nome)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.NomeUsuario == nome);
    }

    public async Task<IEnumerable<UsuarioModel>> GetUsuariosByPartialNomeAsync(string partialNome)
    {
        return await _dbSet.Where(u => u.NomeUsuario.Contains(partialNome)).ToListAsync();
    }
}
