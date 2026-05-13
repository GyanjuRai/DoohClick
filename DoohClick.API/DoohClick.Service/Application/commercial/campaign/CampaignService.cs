
using DoohClick.DataAccess.Dapper;
using DoohClick.DataAccess.Data;
using DoohClick.DataAccess.Entity;
using DoohClick.Interface.Application.commercial.campaign;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Application.commercial.campaign;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using DoohClick.Service.Shared.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DoohClick.Service.Application.commercial.campaign
{
    public class CampaignService : BaseService, ICampaignService
    {
        private readonly AppDbContext _dbContext;
        public CampaignService(
            IDataAccessService dataAccessService,
            IJsonSerializer jsonSerializer,
            AppDbContext dbContext
            ) : base(dataAccessService, jsonSerializer)
        {
            _dbContext = dbContext;
        }

        public async Task<MvGridResponse<MvCampaign>?> GetGrid(MvGridParamOption<MvCampaignFilterOptionParam> param)
        {
            string result = await _dataAccessService.RetrievalProcedure("dbo.sp_campaign_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvGridResponse<MvCampaign>>(result);
        }

        public async Task<List<MvCampaignScreenSchedule>?> GetSchedules(MvCampaignIdParam param)
        {
            string result = await _dataAccessService.RetrievalProcedure("dbo.sp_campaign_screen_schedule_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<List<MvCampaignScreenSchedule>>(result);
        }

        public async Task<MvCampaign?> Save(MvCampaign param)
        {
            string result = await _dataAccessService.ActionProcedure("dbo.sp_campaign_tsk", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvCampaign>(result);
        }

        public async Task<MvCampaignScreenSchedule?> SaveSchedule(MvCampaignScreenScheduleParam param)
        {
            string result = await _dataAccessService.ActionProcedure("dbo.sp_campaign_screen_schedule_tsk", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvCampaignScreenSchedule>(result);
        }
        
        public async Task<MvCampaignIdParam?> Remove(MvCampaignIdParam param)
        {
            string result = await _dataAccessService.ActionProcedure("dbo.sp_campaign_del", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvCampaignIdParam>(result);
        }

        public async Task<MvCampaignIdParam?> Approve(MvCampaignIdParam param)
        {
            Campaign? result = await _dbContext.Campaigns
            .FirstOrDefaultAsync(c => c.Id == param.Id);

            if (result == null) return null;

            result.Status = "ACTIVE";
            result.ModifiedBy = param.UpdatedBy;
            result.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new MvCampaignIdParam { Id = result.Id };
        }
    }
}
