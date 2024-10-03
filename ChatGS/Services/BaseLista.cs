using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace ChatGS.Services
{
    public class BaseLista
    {


        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        public class TableNameAttribute : Attribute
        {
            public string Name { get; }

            public TableNameAttribute(string name)
            {
                Name = name;
            }
        }

        private readonly string _connectionString;

        public BaseLista(string connectionString)
        {
            _connectionString = connectionString;
        }



        List<object> listaSalvar = new List<object>();
        public void AddListaSalvar(object entity)
        {
            listaSalvar.Add(entity);
        }

        public List<object> GetListaSalvar()
        {
            return listaSalvar;
        }


        public class RetornaQueryGravacao
        {
            public (string, DynamicParameters) GenerateInsertCommand<T>(T entity, string tableName)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                PropertyInfo[] properties = typeof(T).GetProperties();


                var identityColumnName = "Id";

                StringBuilder columns = new StringBuilder();
                StringBuilder values = new StringBuilder();
                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    if (property.Name.Equals(identityColumnName, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var value = property.GetValue(entity);
                    if (value == null)
                        continue;

                    if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    {
                        // Se a propriedade é uma classe, gere parâmetros para as suas propriedades também
                        var nestedEntity = value;
                        var nestedGenerator = new RetornaQueryGravacao();
                        var (nestedSql, nestedParameters) = nestedGenerator.GenerateInsertCommand(nestedEntity, property.Name);
                        // Adicione os parâmetros aninhados ao conjunto de parâmetros
                        foreach (var param in nestedParameters.ParameterNames)
                        {
                            parameters.Add(param, nestedParameters.Get<dynamic>(param));
                        }
                        continue;
                    }

                    columns.Append(property.Name).Append(", ");
                    values.Append("@").Append(property.Name).Append(", ");

                    parameters.Add("@" + property.Name, value);
                }

                if (columns.Length > 0) columns.Length -= 2;
                if (values.Length > 0) values.Length -= 2;

                var sql = $@"INSERT INTO {tableName} ({columns}) VALUES ({values}); 
                     SELECT CAST(SCOPE_IDENTITY() AS int);";

                return (sql, parameters);
            }
        }

        public async Task<bool> AtualizarAsync<T>(object entity) where T : class
        {

            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var keyProperty = properties.FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
            if (keyProperty == null)
            {
                throw new InvalidOperationException("A propriedade 'Id' não foi encontrada no objeto.");
            }

            var idValue = keyProperty.GetValue(entity);
            if (idValue == null)
            {
                throw new InvalidOperationException("O ID do objeto não está definido.");
            }

            var setClause = string.Join(", ", properties
                .Where(p => p != keyProperty)
                .Select(p => $"{p.Name} = @{p.Name}"));

            // Construa o comando SQL
            var sql = $"UPDATE {tableName} SET {setClause} WHERE {keyProperty.Name} = @Id;";

            // Cria um dicionário para os parâmetros do comando SQL
            var parameters = properties.ToDictionary(p => p.Name, p => p.GetValue(entity));
            parameters.Add("Id", idValue); // Adiciona o valor da chave primária

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();


                var rowsAffected = await connection.ExecuteAsync(sql, parameters);

                return rowsAffected > 0;
            }
        }

        
         
    }
}