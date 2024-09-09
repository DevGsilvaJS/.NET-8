using ChatGS.Data;
using ChatGS.Servicos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ChatGS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Gerando a chave JWT segura
            var key = new byte[32]; // Tamanho mínimo de 256 bits (32 bytes)
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            var base64Key = Convert.ToBase64String(key);

            // Configuração do DbContext
            builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<ChatGSDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));


            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:7000/",
                    ValidAudience = "ChatGSApp",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(base64Key)) // Corrigido para usar base64Key decodificado
                };
            });

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
            app.UseAuthentication(); // Adicione esta linha para usar autenticação
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
