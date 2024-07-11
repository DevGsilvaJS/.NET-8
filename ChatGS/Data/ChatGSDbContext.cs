﻿using ChatGS.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {




            modelBuilder.Entity<UsuarioModel>()
                .HasOne(u => u.Pessoa)
                .WithMany()
                .HasForeignKey(u => u.IdPessoa);


            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UsuarioModel>().ToTable("Usuario");
            modelBuilder.Entity<PessoaModel>().ToTable("Pessoa");





        }


    }
}
