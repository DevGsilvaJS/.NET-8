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

            // Configura��o do DbContext
            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<ChatGSDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));

            // Registro dos reposit�rios e do reposit�rio gen�rico
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            // Registro da UnitOfWork
            
            // Registro do servi�o de usu�rio
            builder.Services.AddTransient<UsuarioService>();

            // Outros servi�os
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure o pipeline de requisi��o HTTP
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
