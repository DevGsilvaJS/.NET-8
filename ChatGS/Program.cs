using ChatGS.Data;
using ChatGS.Repositorios;
using ChatGS.Repositorios.Interfaces;
using ChatGS.Servicos;
using Microsoft.EntityFrameworkCore;

namespace ChatGS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração do DbContext
            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<ChatGSDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

            // Registro dos repositórios e do repositório genérico
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Registro da UnitOfWork
            
            // Registro do serviço de usuário
            builder.Services.AddTransient<UsuarioService>();

            // Outros serviços
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure o pipeline de requisição HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
