using ChatGS.Interfaces;
using ChatGS.Models;
using ChatGS.ResponseModel;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using static Dapper.SqlMapper;

namespace ChatGS.Repository
{
    public class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : class
    {
        private readonly IDbConnection _dbConnection;

        // Injeção de dependência para a conexão
        public RepositoryGeneric(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<bool> Delete(string query, object parameters)
        {
            int rowsAffected = await _dbConnection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<IDataReader> GetById(string query, object parameters)
        {
            try
            {
                var connection = _dbConnection; // Assumindo que _dbConnection é sua conexão com o banco
                return await connection.ExecuteReaderAsync(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar a query: " + ex.Message);
            }
        }
        public async Task<T> Insert<T>(T entity, string table) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var propertiesAndValues = GetPropertiesAndValues(entity);

            // Filtrar propriedades sem valor e propriedades de relacionamento
            propertiesAndValues = propertiesAndValues
                .Where(kvp => kvp.Value != null &&
                              !(kvp.Value is int intValue && intValue == 0) &&
                              !IsRelationshipProperty(kvp.Key)) // Chame o método que verifica se é uma propriedade de relacionamento
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (!propertiesAndValues.Any())
                throw new InvalidOperationException("Nenhuma propriedade válida para inserir.");

            var columns = string.Join(", ", propertiesAndValues.Keys);  // Nome das colunas
            var values = string.Join(", ", propertiesAndValues.Keys.Select(key => "@" + key));  // Nomes dos parâmetros

            // SQL para o INSERT seguido de captura do ID gerado
            var sql = $@"INSERT INTO {table} ({columns}) VALUES ({values});
                 SELECT CAST(SCOPE_IDENTITY() AS int);";  // Captura o último ID gerado

            var parameters = new DynamicParameters();
            foreach (var kvp in propertiesAndValues)
            {
                parameters.Add("@" + kvp.Key, kvp.Value);
            }

            try
            {
                // Executa o comando de inserção e captura o ID gerado
                var idGerado = await _dbConnection.QuerySingleAsync<int>(sql, parameters);

                // Detecta o prefixo da tabela e monta o nome do campo de ID
                string prefixo = table.Split('_')[1];  // Extrai o prefixo, por exemplo, "PES" de "TB_PES_PESSOA"
                string nomeId = $"{prefixo}ID";        // Monta o nome do ID, ex: "PESID"

                // Atribui o ID gerado à propriedade correspondente
                var propertyInfo = entity.GetType().GetProperty(nomeId);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(entity, idGerado);
                }

                // Retorna a entidade com o ID atualizado
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir entidade: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<dynamic>> ListAll(string query)
        {
            return await _dbConnection.QueryAsync(query);
        }

        public async Task<T> Update(T entity)
        {
            string query = $"UPDATE {typeof(T).Name} SET /*colunas a serem atualizadas*/ WHERE Id = @Id"; // Ajuste para as colunas corretas

            int rowsAffected = await _dbConnection.ExecuteAsync(query, entity);
            return rowsAffected > 0 ? entity : default;
        }
        public static Dictionary<string, object> GetPropertiesAndValues<T>(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var propertiesAndValues = new Dictionary<string, object>();

            // Obter o tipo da entidade
            Type type = entity.GetType();

            // Obter todas as propriedades públicas da entidade
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.CanRead)
                {
                    var value = property.GetValue(entity);
                    // Ignorar propriedades sem valor
                    if (value != null && !(value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        propertiesAndValues.Add(property.Name, value);
                    }
                }
            }

            return propertiesAndValues;
        }
        private bool IsRelationshipProperty(string propertyName)
        {
            // Aqui você pode implementar a lógica para identificar propriedades de relacionamento
            // Por exemplo, pode usar uma lista de nomes de propriedades para ignorar
            var relationshipProperties = new[] { "Pessoa", "GrupoUsuarios" }; // Exemplo de propriedades de relacionamento
            return relationshipProperties.Contains(propertyName);
        }


    }
}
