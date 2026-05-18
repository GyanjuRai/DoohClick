

using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Application.crm.advertiser;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Application.crm.advertiser;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using DoohClick.Service.Shared.Base;
using Newtonsoft.Json;

namespace DoohClick.Service.Application.crm.advertiser
{
    public class AdvertiserService : BaseService, IAdvertiserService
    {
        public AdvertiserService(
                IDataAccessService dataAccessService,
                IJsonSerializer jsonSerializer
            ): base( dataAccessService, jsonSerializer ) 
        { }

        public async Task<MvGridResponse<MvAdvertiser>?> GetAll(MvGridParamOption<MvAdvertiserFilterOptions> param)
        {
            string result = await _dataAccessService.RetrievalProcedure("crm.sp_advertiser_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvGridResponse<MvAdvertiser>>(result);
        }

        public async Task<List<MvAdvertiserDdl>?> GetDdl(MvTenantIdParam param)
        {
            string result = await _dataAccessService.RetrievalProcedure("crm.sp_advertiser_ddl", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<List<MvAdvertiserDdl>>(result);
        }
    }
}
