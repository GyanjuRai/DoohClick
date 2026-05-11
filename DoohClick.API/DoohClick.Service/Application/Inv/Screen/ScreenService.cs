
using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Application.Inv.Screen;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Application.crm.advertiser;
using DoohClick.Model.Application.Inv.Screen;
using DoohClick.Model.Shared.AppClaim;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using DoohClick.Service.Shared.Base;
using Newtonsoft.Json;

namespace DoohClick.Service.Application.Inv.Screen
{
    public class ScreenService : BaseService, IScreenService
    {
        public ScreenService(
                IDataAccessService dataAccessService,
                IJsonSerializer jsonSerializer
            ) : base(dataAccessService, jsonSerializer)
        { }

        public async Task<MvGridResponse<MvScreen>?> GetGrid(MvGridParamOption<MvScreenFilterOptions> param)
        {
            string result = await _dataAccessService.RetrievalProcedure("inv.sp_screen_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvGridResponse<MvScreen>>(result);
        }

        public async Task<MvScreen?> Save(MvScreen param)
        {
            string result = await _dataAccessService.ActionProcedure("inv.sp_screen_tsk", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvScreen>(result);
        }

        public async Task<MvScreen?> Remove(MvScreenDelParam param)
        {
            string result = await _dataAccessService.ActionProcedure("inv.sp_screen_del", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvScreen>(result);
        }

        public async Task<List<MvScreenDdl>?> GetDdl(MvTenantIdParam param)
        {
            string result = await _dataAccessService.RetrievalProcedure("inv.sp_screen_ddl", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<List<MvScreenDdl>>(result);
        }
    }
}
