using Microsoft.AspNetCore.Mvc;

namespace TaskAggregator.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        public BaseController()
        {

        }
    }
}
