

using Asp.Versioning;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using TaskAggregator.Application.CQRS.Commands.Requests.Integration.ImportFromExcel;

namespace TaskAggregator.API.Controllers
{

    [ApiVersion("1.0")]
    //[ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/integration")]
    public class IntegrationController : BaseController
    {
        private readonly IMediator _mediator;
        public IntegrationController(IMediator mediator) : base()
            => _mediator = mediator;


        [HttpPost("import/requests/from-excel")]
        public async Task<ActionResult> Create(IFormFile file, CancellationToken cancellationToken = default)
        {
            await using var stream = file.OpenReadStream();
            stream.Position = 0;

            var cmd = new ImportRequestsFromExcelCommand(stream);
            var result = await _mediator.Send(command: cmd, cancellationToken: cancellationToken);
            return Ok();
        }
    }

}
