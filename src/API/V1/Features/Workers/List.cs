using API.V1.Features.Orders.Response;
using API.V1.Features.Workers.Request;
using API.V1.Features.Workers.Response;
using APIEndpoints;
using AutoMapper;
using Domain;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.V1.Features.Workers
{
    public class List : BaseAsyncEndpoint.WithRequest<WorkerListPageRequest>.WithResponse<List<WorkerListResult>>
    {
        private readonly IAsyncRepository<Worker> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public List(IAsyncRepository<Worker> repository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<List>();
        }

        [HttpGet("api/v1/workers", Name = "GetWorkerList")]
        [SwaggerOperation(Summary = "Gets a list of workers", Description = "List of workers", OperationId = "Worker.List", Tags = new[] {"Workers"})]
        [ProducesResponseType(typeof(List<WorkerListResult>), (int) HttpStatusCode.OK)]
        public override async Task<ActionResult<List<WorkerListResult>>> HandleAsync(
            [FromQuery] WorkerListPageRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.WorkerGetListEvent, "Getting list of workers");

            var workerList = await _repository.ListAllAsync(request.PerPage, request.Page, cancellationToken: cancellationToken);

            return Ok(workerList.Select(x => _mapper.Map<WorkerListResult>(x)));
        }
    }
}