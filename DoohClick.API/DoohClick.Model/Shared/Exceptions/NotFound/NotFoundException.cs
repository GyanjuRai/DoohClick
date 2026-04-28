
using DoohClick.Model.Shared.Enum.HttpStatusCode;
using DoohClick.Model.Shared.Enum.Response;

namespace DoohClick.Model.Shared.Exceptions.NotFound
{
    public class NotFoundException: AppException
    {
        public NotFoundException(
            string message
            ) : base(message, (int)HttpStatusCodeEnum.NotFound, ResponseStatusEnum.NotFound.ToString())
        { }
    }
}
