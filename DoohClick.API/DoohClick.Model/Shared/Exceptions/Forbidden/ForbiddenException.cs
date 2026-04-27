
using DoohClick.Model.Shared.Enum.HttpStatusCode;
using DoohClick.Model.Shared.Enum.Response;

namespace DoohClick.Model.Shared.Exceptions.Forbidden
{
    public class ForbiddenException: AppException
    {
        public ForbiddenException(
            string message
            ) : base(message, (int)HttpStatusCodeEnum.Forbidden, ResponseStatusEnum.Forbidden.ToString())
        {
        }
    }
}
