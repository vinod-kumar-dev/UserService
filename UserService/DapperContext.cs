using System.Data;
using System.Data.SqlClient;
using Dapper;
namespace UserService
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        private IDbConnection CreateConnection()
            => new SqlConnection(_connectionString); 


        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>(sql, parameters, commandType: commandType);
            }
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters, commandType: commandType);
            }
        }
        public async Task<List<List<T>>> QueryMultipleAsync<T>(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            List<List<T>> list = new List<List<T>>();
            using (var connection = CreateConnection())
            {
                using (var multiResult = await connection.QueryMultipleAsync(sql, parameters, commandType: commandType))
                {
                    while (!multiResult.IsConsumed)
                    {
                        var resultSet = await multiResult.ReadAsync<T>();

                        list.Add(resultSet.ToList());
                    }
                }
            }
            return list;
        }

        public async Task<int> ExecuteAsync(string sql, object parameters = null, CommandType commandType = CommandType.Text)
        {
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(sql, parameters, commandType: commandType);
            }
        }
    }
}
