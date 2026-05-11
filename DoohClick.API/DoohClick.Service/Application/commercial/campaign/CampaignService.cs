
using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Application.commercial.campaign;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Application.commercial.campaign;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using DoohClick.Service.Shared.Base;
using Newtonsoft.Json;

namespace DoohClick.Service.Application.commercial.campaign
{
    public class CampaignService: BaseService, ICampaignService
    {
        public CampaignService(
            IDataAccessService dataAccessService,
            IJsonSerializer jsonSerializer
            ): base(dataAccessService, jsonSerializer)
        { 
        }

        public async Task<MvGridResponse<MvCampaign>?> GetGrid(MvGridParamOption<MvCampaignFilterOptionParam> param)
        {
            string result = await _dataAccessService.RetrievalProcedure("dbo.sp_campaign_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvGridResponse<MvCampaign>>(result);
        }

        public async Task<MvCampaign?> Save(MvCampaign param)
        {
            string result = await _dataAccessService.ActionProcedure("dbo.sp_campaign_tsk", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvCampaign>(result);
        }

        public async Task<MvCampaignIdParam?> Remove(MvCampaignIdParam param)
        {
            string result = await _dataAccessService.ActionProcedure("dbo.sp_campaign_del", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvCampaignIdParam>(result);
        }
    }
}
