using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EventManagement.Data.Dapper
{
    public class DapperHelper : IDapperHelper
    {
        private readonly string connectString = string.Empty;
        private IDbConnection _dbConnection;

        public DapperHelper(IConfiguration configuration)
        {
            connectString = configuration.GetConnectionString("DefaultSQLConnection");
            _dbConnection = new SqlConnection(connectString);
        }

        public async Task ExecuteNotReturn(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null)
        {
            using (var dbConnection = new SqlConnection(connectString)) //Co the goi dispose de huy ket noi
            {
                await dbConnection.ExecuteAsync(query, param: parameters, dbTransaction, commandType: CommandType.Text);
            }
        }

        public async Task<T> ExcuteReturnScalar<T>(string query, DynamicParameters parameters = null)
        {
            using (var dbConnection = new SqlConnection(connectString)) //Co the goi dispose de huy ket noi
            {
                return (T)Convert.ChangeType(await dbConnection.ExecuteScalarAsync<T>(query, param: parameters, commandType: CommandType.Text), typeof(T));
            }
        }

        public async Task<IEnumerable<T>> ExecuteSqlReturnList<T>(string query, DynamicParameters parameters = null)
        {
            using (var dbConnection = new SqlConnection(connectString)) //Co the goi dispose de huy ket noi
            {
                return await dbConnection.QueryAsync<T>(query, param: parameters, commandType: CommandType.Text);
            }
        }

        public async Task<IEnumerable<T>> ExecuteStoreProcedureReturnList<T>(string query, DynamicParameters parameters = null)
        {
            using (var dbConnection = new SqlConnection(connectString)) //Co the goi dispose de huy ket noi
            {
                return await dbConnection.QueryAsync<T>(query, param: parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
