using Asp.Versioning;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using TaskAggregator.API.Dtos.Params;
using TaskAggregator.API.Dtos.Responses;
using TaskAggregator.API.Extensions;
using TaskAggregator.Application.CQRS.Dtos.Requests.Output;
using TaskAggregator.Application.CQRS.Params;
using TaskAggregator.Application.CQRS.Queries.Requests.GetById;
using TaskAggregator.Application.CQRS.Queries.Requests.GetFiltered;


namespace TaskAggregator.API.Controllers
{

    [ApiVersion("1.0")]
    //[ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/requests")]
    public class RequestsController : BaseController
    {
        private readonly IMediator _mediator;
        public RequestsController(IMediator mediator) : base()
            => _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestResponse>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            var query = new GetRequestByIdQuery(id);
            var result = await _mediator.Send<RequestDto>(query, cancellationToken);
            var responseDto = result.Adapt<RequestResponse>();
            return Ok(responseDto);
        }

        [HttpGet()]
        public async Task<ActionResult<GenericOffsetPagedResponse<RequestResponse>>> GetFiltered([FromQuery] OffsetPagingRequestParams queryParams, CancellationToken cancellationToken = default)
        {
            var query = new GetRequestFilteredQuery(new OffsetPagingParams(queryParams.PageSize, queryParams.PageNumber));
            var result = await _mediator.Send(query: query, cancellationToken: cancellationToken);
            var pagedResponse = result.MapToOffsetPagedResponse<RequestDto, RequestResponse>();
            return Ok(pagedResponse);
        }
    }

}
