
using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Shared.JsonSerializer;

namespace DoohClick.Service.Shared.Base
{
    public class BaseService
    {
        protected readonly IDataAccessService _dataAccessService;
        protected readonly IJsonSerializer _jsonSerializer;

        public BaseService(
            IDataAccessService dataAccessService, 
            IJsonSerializer jsonSerializer
            )
        {
            _dataAccessService = dataAccessService;
            _jsonSerializer = jsonSerializer;
        }
    }
}
