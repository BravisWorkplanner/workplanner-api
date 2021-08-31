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
    public class Update : BaseAsyncEndpoint.WithRequest<WorkerUpdateRequest>.WithResponse<int>
    {
        private readonly IAsyncRepository<Worker> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public Update(IAsyncRepository<Worker> repository, ILoggerFactory loggerFactory, IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger<Update>();
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPut("api/v1/workers", Name = "WorkerUpdate")]
        [SwaggerOperation(Summary = "Update an Worker", Description = "Update an Worker", OperationId = "Worker.Update", Tags = new[] {"Workers"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] WorkerUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.WorkerGetEvent, "Getting worker with id {WorkerId}", request.Id);
            var worker = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (worker == null)
            {
                _logger.LogWarning(LogEvents.WorkerNotFoundEvent, "Worker with id {WorkerId} was not found", request.Id);

                return NotFound(request.Id);
            }

            var updatedOrder = _mapper.Map<Worker>(request);

            _logger.LogInformation(LogEvents.WorkerUpdateEvent, "Updating worker with id {WorkerId}", request.Id);

            await _repository.UpdateAsync(updatedOrder, cancellationToken);

            return Ok(request.Id);
        }
    }
}