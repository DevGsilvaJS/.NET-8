using ChatGS.Models;
using ChatGS.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatGS.Repositorios
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveEntitiesAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task SaveTransaction(List<object> lista)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.AddRange(lista); // Adiciona todos os objetos na lista ao contexto

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        // Outros métodos omitidos para brevidade...
    }
}
