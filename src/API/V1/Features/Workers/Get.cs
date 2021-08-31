using API.V1.Features.Workers.Response;
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
    public class Get : BaseAsyncEndpoint.WithRequest<int>.WithResponse<WorkerGetResult>
    {
        private readonly IAsyncRepository<Worker> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public Get(IAsyncRepository<Worker> repository, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<Get>();
        }

        [HttpGet("api/v1/workers/{id}", Name = "GetWorker")]
        [SwaggerOperation(Summary = "Gets a worker by id", Description = "Gets a worker by id", OperationId = "Worker.Get", Tags = new[] {"Workers"})]
        [ProducesResponseType(typeof(WorkerGetResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.NotFound)]
        public override async Task<ActionResult<WorkerGetResult>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.WorkerGetEvent, "Getting worker with id {WorkerId}", id);

            var worker = await _repository.GetByIdAsync(id, cancellationToken);
            if (worker == null)
            {
                _logger.LogWarning(LogEvents.WorkerNotFoundEvent, "Worker with id {WorkerId} was not found", id);

                return NotFound(id);
            }

            return Ok(_mapper.Map<WorkerGetResult>(worker));
        }
    }
}