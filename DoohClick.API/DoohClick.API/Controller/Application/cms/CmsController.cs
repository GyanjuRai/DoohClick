using Microsoft.AspNetCore.Mvc;

namespace DoohClick.API.Controller.Application.cms
{
    [Produces("application/json")]
    [ApiController]
    [Route("/cms/[controller]")]
    public class CmsController: ControllerBase
    {
    }
}
