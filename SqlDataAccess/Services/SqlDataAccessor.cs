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
                        var columnName = reader.GetName(i);
                        var property = typeof(T).GetProperties()
                            .FirstOrDefault(p => Attribute.IsDefined(p, typeof(ColumnAttribute)) &&
                                                 ((ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute)))?.Name == columnName);

                        if (property == null)
                        {
                            continue;
                        }

                        if (property.PropertyType == typeof(DateOnly))
                        {
                            var dateValue = Convert.ToDateTime(reader.GetValue(i));
                            property.SetValue(dataModel, new DateOnly(dateValue.Year, dateValue.Month, dateValue.Day));
                        }
                        else
                        {
                            property.SetValue(dataModel, reader.GetValue(i));
                        }
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
            finally
            {
                await _connection.CloseAsync();
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

                if (!reader.HasRows)
                {
                    return dataModel;
                }

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);

                    var property = typeof(T).GetProperties()
                        .FirstOrDefault(p => Attribute.IsDefined(p, typeof(ColumnAttribute)) &&
                                              ((ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute)))?.Name == columnName);

                    if (property == null || reader.IsDBNull(i))
                    {
                        continue;
                    }

                    var value = reader.GetValue(i);

                    if (property.PropertyType == typeof(DateOnly) && value is DateTime dateTimeValue)
                    {
                        value = new DateOnly(dateTimeValue.Year, dateTimeValue.Month, dateTimeValue.Day);
                    }

                    if (property.PropertyType == typeof(DateOnly?) && value is DateTime nullableDateTimeValue)
                    {
                        value = nullableDateTimeValue == DateTime.MinValue
                            ? (DateOnly?)null
                            : new DateOnly(nullableDateTimeValue.Year, nullableDateTimeValue.Month, nullableDateTimeValue.Day);
                    }

                    property.SetValue(dataModel, value);
                }

                await reader.CloseAsync();
                await _connection.CloseAsync();

                return dataModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in GetEntityAsync. Couldn't get Data from SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
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
                await reader.CloseAsync();
                await _connection.CloseAsync();

                if (hasData)
                {
                    await UpdateEntityAsync(entity, tableName, columnName, primaryKeyValue.ToString());
                }
                else
                {
                    await InsertEntityAsync(entity, tableName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in EntitySetAsync. Couldn't insert/update Data in SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private async Task InsertEntityAsync<T>(T entity, string tableName) where T : class
        {
            try
            {
                var properties = entity.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute)));

                var columnNames = string.Join(", ", properties.Select(p => ((ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute))).Name));

                var paramNames = string.Join(", ", properties.Select(p => "@" + p.Name));

                var insertCommand = $"INSERT INTO {tableName} ({columnNames}) VALUES ({paramNames})";

                await using var command = new SqlCommand(insertCommand, _connection);

                foreach (var property in properties)
                {
                    command.Parameters.AddWithValue("@" + property.Name, property.GetValue(entity));
                }

                await _connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in InsertEntityAsync. Couldn't insert Data in SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
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
            }
            catch (Exception ex)
            {
                throw new Exception("Exception in UpdateEntityAsync. Couldn't update Data in SQL-Server", ex);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        private static string DbConnectionStringGet(MsSqlDataSettings msSqlDataSettings)
        {
            return $"Server={msSqlDataSettings.Server}; Database={msSqlDataSettings.Database}; User Id={msSqlDataSettings.UserId}; Password={msSqlDataSettings.Password};";
        }
    }
}
