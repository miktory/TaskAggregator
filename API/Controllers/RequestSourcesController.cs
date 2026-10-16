using Asp.Versioning;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using TaskAggregator.API.Dtos.Params;
using TaskAggregator.API.Dtos.Requests;
using TaskAggregator.API.Dtos.Responses;
using TaskAggregator.API.Extensions;
using TaskAggregator.Application.CQRS.Commands.RequestSources.Create;
using TaskAggregator.Application.CQRS.Dtos.RequestSources.Output;
using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Application.CQRS.Queries.RequestSources.GetById;
using TaskAggregator.Application.CQRS.Queries.RequestSources.GetFiltered;

namespace TaskAggregator.API.Controllers
{

    [ApiVersion("1.0")]
    //[ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/request-sources")]
    public class RequestSourcesController : BaseController
    {
        private readonly IMediator _mediator;
        public RequestSourcesController(IMediator mediator) : base()
            => _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestSourceResponse>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            var query = new GetRequestSourceByIdQuery(id);
            var result = await _mediator.Send<RequestSourceDto>(query, cancellationToken);
            var responseDto = result.Adapt<RequestSourceResponse>();
            return Ok(responseDto);
        }

        [HttpPost()]
        public async Task<ActionResult> Create([FromBody] CreateRequestSourceRequest cmdRequest, CancellationToken cancellationToken = default)
        {
            var cmd = new CreateRequestSourceCommand(cmdRequest.ExternalId);
            var result = await _mediator.Send<long>(command: cmd, cancellationToken: cancellationToken);
            return CreatedAtAction(actionName: nameof(GetById), value: null, routeValues: new { id = result });
        }

        [HttpGet()]
        public async Task<ActionResult<GenericOffsetPagedResponse<RequestSourceResponse>>> GetFiltered([FromQuery] OffsetPagingRequestParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = new GetRequestSourceFilteredQuery(new OffsetPagingParams(queryParams.PageSize, queryParams.PageNumber));
            var result = await _mediator.Send(query: query, cancellationToken: cancellationToken);
            var pagedResponse = result.MapToOffsetPagedResponse<RequestSourceDto, RequestSourceResponse>();
            return Ok(pagedResponse);
        }


    }

}
