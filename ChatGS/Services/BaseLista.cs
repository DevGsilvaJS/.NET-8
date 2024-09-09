using Dapper;
using System.Reflection;
using System.Text;

namespace ChatGS.Services
{
    public class BaseLista
    {

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
    }

}
