using ChatGS.Interfaces;
using ChatGS.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data; // N�o se esque�a de incluir esta linha
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using ChatGS.Models.Users;
using ChatGS.Services.Users;
using ChatGS.Models.Transactions;
using ChatGS.Services.Transactions; // Certifique-se de que isso est� aqui

namespace ChatGS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var key = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            var base64Key = Convert.ToBase64String(key);

            // Configura��o do IDbConnection
            builder.Services.AddScoped<IDbConnection>(db =>
                new SqlConnection(builder.Configuration.GetConnectionString("DataBase")));


            // Configura��o do CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173") // URL do frontend
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            // Registro do reposit�rio gen�rico
            builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
            builder.Services.AddScoped<IRepositoryGeneric<UsuarioModel>, RepositoryGeneric<UsuarioModel>>();
            builder.Services.AddScoped<IRepositoryGeneric<PessoaModel>, RepositoryGeneric<PessoaModel>>();
            builder.Services.AddScoped<IRepositoryGeneric<PlanoContasModel>, RepositoryGeneric<PlanoContasModel>>();
            builder.Services.AddScoped<UsuarioService>();
            builder.Services.AddScoped<PlanoContasServices>();


            // Configura��o da autentica��o JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

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

            app.UseCors("AllowSpecificOrigin");

            app.UseHttpsRedirection();
            app.UseAuthentication(); // Adicionando autentica��o ao pipeline
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
