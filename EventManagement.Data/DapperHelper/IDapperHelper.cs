using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Dapper
{
    public interface IDapperHelper
    {
        Task<T> ExcuteReturnScalar<T>(string query, DynamicParameters parameters = null);
        Task ExecuteNotReturn(string query, DynamicParameters parameters = null, IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteSqlReturnList<T>(string query, DynamicParameters parameters = null);
        Task<IEnumerable<T>> ExecuteStoreProcedureReturnList<T>(string query, DynamicParameters parameters = null);
    }
}
