

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace DoohClick.DataAccess.Dapper
{
    public class DataAccessService: IDataAccessService
    {
        private readonly string _ConnectionString = null!;

        public DataAccessService(IConfiguration configuration)
        {
            var ACTIVE_DB = configuration["ACTIVE_DB"] ?? "Local";
            _ConnectionString = configuration.GetConnectionString(ACTIVE_DB) ?? "";
        }

        public async Task<string> ActionProcedure(string sp, string json)
        {
            await using SqlConnection con = new(_ConnectionString);
            await con.OpenAsync();
            DynamicParameters param = new ();
            param.Add("@Json", json, DbType.String, direction: ParameterDirection.InputOutput);
            await con.ExecuteAsync(sp, param, commandType: CommandType.StoredProcedure);
            return param.Get<String>("@Json") ?? "{}";
        } 

        public async Task<string> RetrievalProcedure(string sp, string json)
        {
            await using SqlConnection con = new(_ConnectionString);
            await con.OpenAsync();
            DynamicParameters param = new ();
            param.Add("@Json", json, DbType.String);
            string result = await con.QueryFirstOrDefaultAsync<string>(sp, param, commandType: CommandType.StoredProcedure) ?? "{}";
            return result;
        }

        public async Task<string> RetrievalProcedure(string sp)
        {
            await using SqlConnection con = new(_ConnectionString);
            await con.OpenAsync();
            string result = await con.QueryFirstOrDefaultAsync<string>(sp, commandType: CommandType.StoredProcedure) ?? "{}";
            return result;
        }
    }
}
