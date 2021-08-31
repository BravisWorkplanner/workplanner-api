using API.V1.Features.Workers.Request;
using APIEndpoints;
using AutoMapper;
using Domain;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.V1.Features.Workers
{
    public class Create : BaseAsyncEndpoint.WithRequest<WorkerCreateRequest>.WithResponse<int>
    {
        private readonly IAsyncRepository<Worker> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public Create(IAsyncRepository<Worker> repository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<Create>();
        }

        [HttpPost("api/v1/workers", Name = "CreateWorker")]
        [SwaggerOperation(
            Summary = "Create a new worker",
            Description = "Create a new worker",
            OperationId = "Worker.Create",
            Tags = new[] {"Workers"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] WorkerCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.WorkerCreateEvent, "Creating new worker");

            var worker = _mapper.Map<Worker>(request);

            var result = await _repository.AddAsync(worker, cancellationToken);

            return CreatedAtRoute("GetWorker", new {id = result.Id}, result);
        }
    }
}