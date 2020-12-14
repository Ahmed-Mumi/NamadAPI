using Microsoft.AspNetCore.Mvc;
using NomadAPI.Helpers;

namespace NomadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class BaseApiController : ControllerBase
    {
    }
}
