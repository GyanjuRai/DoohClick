
using DoohClick.Model.Shared.Enum.HttpStatusCode;
using DoohClick.Model.Shared.Enum.Response;

namespace DoohClick.Model.Shared.Exceptions.Conflict
{
    public class ConflictException: AppException
    {
        public ConflictException(
            string message
            ) : base(message, (int)HttpStatusCodeEnum.Conflict, ResponseStatusEnum.Conflict.ToString())
        { }
    }
}
