using ChatGS.ResponseModel;
using System.Data;

namespace ChatGS.Interfaces
{
    public interface IRepositoryGeneric<T> where T : class
    {
        Task<T> Insert<T>(T entity, string table) where T : class;
        Task<IDataReader> GetById(string query, object parameters);
        Task<IEnumerable<dynamic>> ListAll(string query);
        Task<bool> Delete(string query, object parameters);
        Task<T> Update(T entity);

    }
}
