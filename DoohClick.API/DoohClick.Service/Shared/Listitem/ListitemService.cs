

using DoohClick.DataAccess.Dapper;
using DoohClick.Interface.Shared.JsonSerializer;
using DoohClick.Interface.Shared.Listitem;
using DoohClick.Model.Shared.Listitem;
using DoohClick.Service.Shared.Base;
using Newtonsoft.Json;

namespace DoohClick.Service.Shared.Listitem
{
    public class ListitemService: BaseService, IListiemService
    {
        public ListitemService(
                IDataAccessService dataAccessService,
                IJsonSerializer jsonSerializer
            ): base( dataAccessService, jsonSerializer ) 
        { }

        public async Task<List<MvListitemDdlResponse>?> GetDdl(MvListitemDdlParam param)
        {
            string result = await _dataAccessService.RetrievalProcedure("shared.sp_listitem_ddl", JsonConvert.SerializeObject(param));
            return _jsonSerializer.DeserializeObject<List<MvListitemDdlResponse>?>(result);
        }
    }
}
