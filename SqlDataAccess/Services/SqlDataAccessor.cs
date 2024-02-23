using SqlDataAccess.Contract.Configurations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Reflection;

namespace SqlDataAccess.Services
{
    public class SqlDataAccessor(MsSqlDataSettings msSqlDataSettings)
    {
        private readonly SqlConnection _connection = new(DbConnectionStringGet(msSqlDataSettings));

        public async Task<List<T>> GetEntityListAsync<T>(string sqlCommand, SqlParameter[] parameters) where T : class, new()
        {
            try
            {
                await using var command = new SqlCommand(sqlCommand, _connection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await _connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                var results = new List<T>();

                while (await reader.ReadAsync())
                {
                    var dataModel = new T();

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var property = dataModel.GetType().GetProperty(reader.GetName(i));
                        property?.SetValue(dataModel, reader.GetValue(i));
                    }

                    results.Add(dataModel);
                }

                await reader.CloseAsync();
                await _connection.CloseAsync();

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetEntityListAsync. Couldn't get Data from SQL-Server", ex);
            }
        }

        public async Task<T> GetEntityAsync<T>(string sqlCommand, SqlParameter[] parameters) where T : class, new()
        {
            try
            {
                await using var command = new SqlCommand(sqlCommand, _connection);

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                await _connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();

                var dataModel = new T();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var property = dataModel.GetType().GetProperty(reader.GetName(i));
                    property?.SetValue(dataModel, reader.GetValue(i));
                }

                await reader.CloseAsync();
                await _connection.CloseAsync();

                return dataModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetEntityAsync. Couldn't get Data from SQL-Server", ex);
            }
        }

        public async Task SetEntityAsync<T>(T entity) where T : class
        {
            try
            {
                var primaryKeyProperty = typeof(T).GetProperties()
                    .FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute))) 
                                         ?? throw new Exception("Type needs to be an Entity with a primary Key");

                var tableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name
                                ?? throw new Exception("Type needs to be an Entity with a Table Attribute");

                var columnName = primaryKeyProperty.GetCustomAttribute<ColumnAttribute>()?.Name
                                 ?? throw new Exception("Type needs to be an Entity with a Column Attribute");

                var primaryKeyValue = primaryKeyProperty.GetValue(entity);

                var isPrimaryKeyNull = primaryKeyValue is null or 0;

                if (isPrimaryKeyNull)
                {
                    await InsertEntityAsync(entity, tableName);
                    return;
                }

                await _connection.OpenAsync();
                var sqlCommand = $"select * from {tableName} where {columnName} = '{primaryKeyValue}'";
                await using var command = new SqlCommand(sqlCommand, _connection);
                var reader = await command.ExecuteReaderAsync();

                var hasData = await reader.ReadAsync();

                if (hasData)
                {
                    await UpdateEntityAsync(entity, tableName, columnName, primaryKeyValue.ToString());
                }
                else
                {
                    await InsertEntityAsync(entity, tableName);
                }

                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in EntitySetAsync. Couldn't insert/update Data in SQL-Server", ex);
            }
        }

        private async Task InsertEntityAsync<T>(T entity, string tableName) where T : class
        {
            try
            {
                var properties = entity.GetType().GetProperties();

                var columnNames = string.Join(", ", properties.Select(p => p.Name));
                var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

                var insertCommand = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";

                await using var command = new SqlCommand(insertCommand, _connection);

                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue("@" + property.Name, property.GetValue(entity));
                }

                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in InsertEntityAsync. Couldn't insert Data in SQL-Server", ex);
            }
        }

        private async Task UpdateEntityAsync<T>(T entity, string tableName, string columnName, string primaryKeyValue) where T : class
        {
            try
            {
                var properties = entity.GetType().GetProperties();

                var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

                var updateCommand = $"UPDATE {tableName} SET {setClause} WHERE {columnName} = {primaryKeyValue}";

                await using var command = new SqlCommand(updateCommand, _connection);

                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue("@" + property.Name, property.GetValue(entity));
                }

                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in UpdateEntityAsync. Couldn't update Data in SQL-Server", ex);
            }
        }

        private static string DbConnectionStringGet(MsSqlDataSettings msSqlDataSettings)
        {
            return $"Server={msSqlDataSettings.Server}; Database={msSqlDataSettings.Database}; User Id={msSqlDataSettings.UserId}; Password={msSqlDataSettings.Password};";
        }
    }
}
