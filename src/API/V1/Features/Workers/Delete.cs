using APIEndpoints;
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
    public class Delete : BaseAsyncEndpoint.WithRequest<int>.WithResponse<int>
    {
        private readonly IAsyncRepository<Worker> _repository;
        private readonly ILogger _logger;

        public Delete(IAsyncRepository<Worker> repository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Delete>();
            _repository = repository;
        }

        [HttpDelete("api/v1/workers/{id}", Name = "DeleteWorker")]
        [SwaggerOperation(
            Summary = "Delete a new worker",
            Description = "Delete a new worker",
            OperationId = "Worker.Delete",
            Tags = new[] {"Workers"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        public override async Task<ActionResult<int>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.WorkerGetEvent, "Getting worker with id {WorkerId}", id);
            var worker = await _repository.GetByIdAsync(id, cancellationToken);
            if (worker == null)
            {
                _logger.LogWarning(LogEvents.WorkerNotFoundEvent, "Order with id {OrderId} was not found", id);

                return NotFound(id);
            }

            _logger.LogInformation(LogEvents.WorkerDeleteEvent, "Deleting worker with id {OrderId}", id);

            await _repository.DeleteAsync(worker, cancellationToken);

            return Ok(id);
        }
    }
}