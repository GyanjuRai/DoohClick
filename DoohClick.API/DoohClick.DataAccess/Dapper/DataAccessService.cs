

using Dapper;
using DoohClick.Model.Shared.Enum.HttpStatusCode;
using DoohClick.Model.Shared.Enum.Response;
using DoohClick.Model.Shared.Exceptions;
using DoohClick.Model.Shared.Exceptions.Conflict;
using DoohClick.Model.Shared.Exceptions.Forbidden;
using DoohClick.Model.Shared.Exceptions.NotFound;
using DoohClick.Model.Shared.Exceptions.Validation;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace DoohClick.DataAccess.Dapper
{
    public class DataAccessService : IDataAccessService
    {
        private readonly string _ConnectionString = null!;

        public DataAccessService(IConfiguration configuration)
        {
            var ACTIVE_DB = configuration["ACTIVE_DB"] ?? "Local";
            _ConnectionString = configuration.GetConnectionString(ACTIVE_DB) ?? "";
        }

        public async Task<string> ActionProcedure(string sp, string json)
        {
            try
            {
                await using SqlConnection con = new(_ConnectionString);
                await con.OpenAsync();
                DynamicParameters param = new();
                param.Add("@Json", json, DbType.String, direction: ParameterDirection.InputOutput);
                await con.ExecuteAsync(sp, param, commandType: CommandType.StoredProcedure);
                return param.Get<string>("@Json") ?? "{}";
            }
            catch (SqlException ex)
            {
                throw MapSqlException(ex);
            }
        }

        public async Task<string> RetrievalProcedure(string sp, string json)
        {
            try
            {
                await using SqlConnection con = new(_ConnectionString);
                await con.OpenAsync();
                DynamicParameters param = new();
                param.Add("@Json", json, DbType.String);
                string result = await con.QueryFirstOrDefaultAsync<string>(sp, param, commandType: CommandType.StoredProcedure) ?? "{}";
                return result;
            }
            catch (SqlException ex)
            {
                throw MapSqlException(ex);
            }
        }

        public async Task<string> RetrievalProcedure(string sp)
        {
            try
            {
                await using SqlConnection con = new(_ConnectionString);
                await con.OpenAsync();
                string result = await con.QueryFirstOrDefaultAsync<string>(sp, commandType: CommandType.StoredProcedure) ?? "{}";
                return result;
            }
            catch (SqlException ex)
            {
                throw MapSqlException(ex);
            }
        }

        private static AppException MapSqlException(SqlException ex) => ex.Number switch
        {
            >= 50100 and <= 50199 => new NotFoundException(ex.Message),
            >= 50200 and <= 50299 => new ValidationException(ex.Message),
            >= 50300 and <= 50399 => new ForbiddenException(ex.Message),
            >= 50400 and <= 50499 => new ConflictException(ex.Message),
            _ => new AppException(ex.Message, (int)HttpStatusCodeEnum.InternalServerError, ResponseStatusEnum.ServerError.ToString())
        };
    }
}
