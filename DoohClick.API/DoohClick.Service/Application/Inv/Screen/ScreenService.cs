
using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Application.Inv.Screen;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Application.Inv.Screen;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using DoohClick.Service.Shared.Base;
using Newtonsoft.Json;

namespace DoohClick.Service.Application.Inv.Screen
{
    public class ScreenService: BaseService, IScreenService
    {
        public ScreenService(
                IDataAccessService dataAccessService,
                IJsonSerializer jsonSerializer
            ): base(dataAccessService, jsonSerializer)
        {}

        public async Task<MvGridResponse<MvScreen>?> GetGrid(MvGridParamOption<MvScreenFilterOptions> param)
        {
            string result = await _dataAccessService.RetrievalProcedure("inv.sp_screen_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvGridResponse<MvScreen>>(result);
        }
    }
}
