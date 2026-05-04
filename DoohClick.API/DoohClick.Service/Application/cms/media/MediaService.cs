
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

        public async Task<MvMedia?> Add(MvFileUploadParam param, int tenantId, int userId)
        {
            MvFileUploadResult fileUploadResult = await _fileService.UploadAsync(param);

            MvMedia media = new MvMedia
            {
                TenantId = tenantId,
                DisplayName = param.DisplayName,
                FileName = fileUploadResult.FileName,
                FileUrl = fileUploadResult.FileUrl,
                FileSizeBytes = fileUploadResult.FileSizeBytes,
                Resolution = fileUploadResult.Resolution,
                DurationSec = fileUploadResult.DurationSec,
                IsVideo = fileUploadResult.IsVideo,
                UploadedBy = userId,
                CreatedBy = userId
            };

            string result = await _dataAccessService.ActionProcedure("dbo.sp_media_ins", JsonConvert.SerializeObject(media));
            return _jsonSerializer.DeserializeObject<MvMedia>(result);
        }

        public async Task<MvMedia?> Remove(MvMediaDel param)
        {
            string result = await _dataAccessService.ActionProcedure("dbo.sp_media_del", JsonConvert.SerializeObject(param));
            var media = _jsonSerializer.DeserializeObject<MvMedia>(result);

            if (media != null && !string.IsNullOrEmpty(media.FileUrl))
            {
                _fileService.DeleteAsync(media.FileUrl);
            }

            return media;
        }

        public async Task<MvMediaDdl?> GetDdl(MvTenantIdParam param)
        {
            string result = await _dataAccessService.RetrievalProcedure("dbo.sp_media_ddl", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<MvMediaDdl>(result);
        }
    }
}
