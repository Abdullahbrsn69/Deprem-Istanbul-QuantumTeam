using Qu.Data;
using Npgsql;
using System.Data;

public class DbBase
{
    private static string DefaultConnectionString = dbDsnStore.Dev;

    #region Get

    public static Task<V> Get<V>(string functionName, params object[] parameters)
        => Get<V>(DefaultConnectionString, functionName, parameters);

    public static async Task<V> Get<V>(string connectionString, string functionName, params object[] parameters)
    {
        try
        {
            var list = await List<V>(connectionString, functionName, parameters);
            return list.FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region List

    public static Task<List<V>> List<V>(string functionName, params object[] parameters)
        => List<V>(DefaultConnectionString, functionName, parameters);

    public static async Task<List<V>> List<V>(string connectionString, string functionName, params object[] parameters)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = CreateCommandForFunction(connection, functionName, parameters);
            return await ReadData<V>(command);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region ExecuteNonQuery

    public static Task<int> ExecuteNonQuery(string procedureName, params object[] parameters)
        => ExecuteNonQuery(DefaultConnectionString, procedureName, parameters);

    public static async Task<int> ExecuteNonQuery(string connectionString, string procedureName, params object[] parameters)
    {
        try
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = CreateCommandForFunction(connection, procedureName, parameters);
            var result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                return default;
            }

            return (int)result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion

    #region Private Helper Methods

    private static NpgsqlCommand CreateCommandForFunction(NpgsqlConnection connection, string functionName, params object[] parameters)
    {
        try
        {
            var parameterNames = Enumerable.Range(0, parameters.Length).Select(i => $"@p{i}").ToArray();
            var sql = $"SELECT * FROM {functionName}({string.Join(", ", parameterNames)})";

            var command = new NpgsqlCommand(sql, connection);
            for (int i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue($"p{i}", parameters[i]);
            }
            return command;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private static async Task<List<V>> ReadData<V>(NpgsqlCommand command)
    {
        try
        {
            var result = new List<V>();
            using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
                return result; // Veri yoksa boş liste döndür

            var properties = typeof(V).GetProperties();
            var columnNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < reader.FieldCount; i++)
                columnNames.Add(reader.GetName(i));

            while (await reader.ReadAsync())
            {
                var obj = Activator.CreateInstance<V>();

                foreach (var property in properties)
                {
                    if (columnNames.Contains(property.Name))
                    {
                        var value = reader[property.Name];
                        if (value != DBNull.Value)
                            property.SetValue(obj, value);
                    }
                }

                result.Add(obj);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw; // Hata yönetimini daha spesifik hale getirin
        }
    }

    #endregion
}