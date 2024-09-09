using ChatGS.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatGS.Data
{
    public class ChatGSDbContext : DbContext
    {

        public ChatGSDbContext(DbContextOptions<ChatGSDbContext> options)
            : base(options)
        {

        }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<PessoaModel> Pessoas { get; set; }
        public DbSet<GrupoUsuarioModel> GrupoUsuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



        }


    }
}
