
using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Application.cms.media;
using DoohClick.Interface.Shared.File;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Model.Application.cms.media;
using DoohClick.Model.Application.crm.advertiser;
using DoohClick.Model.Application.Inv.Screen;
using DoohClick.Model.Shared.File;
using DoohClick.Model.Shared.Param;
using DoohClick.Model.Shared.Response;
using DoohClick.Service.Shared.Base;
using Newtonsoft.Json;

namespace DoohClick.Service.Application.cms.media
{
    public class MediaService: BaseService, IMediaService
    {
        private readonly IFileService _fileService;
        public MediaService(
                IDataAccessService dataAccessService,
                IJsonSerializer jsonSerializer,
                IFileService fileService
            ) : base(dataAccessService, jsonSerializer) 
        { 
            _fileService = fileService;
        }

        public async Task<MvGridResponse<MvMedia>?> GetGrid(MvGridParamOption<MvMediaFilterOptions> param)
        {
            string result = await _dataAccessService.RetrievalProcedure("dbo.sp_media_sel", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvGridResponse<MvMedia>>(result);
        }

        public async Task<MvMedia?> Add(MvMedia param)
        {
            MvMedia media = new MvMedia
            {
                TenantId = param.TenantId,
                DisplayName = param.DisplayName,
                FileName = param.FileName,
                FileUrl = param.FileUrl,
                FileSizeBytes = param.FileSizeBytes,
                Resolution = param.Resolution,
                DurationSec = param.DurationSec,
                IsVideo = param.IsVideo,
                UploadedBy = param.UploadedBy,
                CreatedBy = param.CreatedBy
            };

            string result = await _dataAccessService.ActionProcedure("dbo.sp_media_ins", JsonConvert.SerializeObject(media));
            return _jsonSerializer.DeserializeObject<MvMedia>(result);
        }

        public async Task<MvMedia?> Remove(MvMediaDel param)
        {
            string result = await _dataAccessService.ActionProcedure("dbo.sp_media_del", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvMedia>(result);
        }

        public async Task<List<MvMediaDdl>?> GetDdl(MvTenantIdParam param)
        {
            string result = await _dataAccessService.RetrievalProcedure("dbo.sp_media_ddl", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<List<MvMediaDdl>>(result);
        }
    }
}
