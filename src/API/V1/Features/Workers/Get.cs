using API.V1.Features.Workers.Response;
using APIEndpoints;
using Domain;
using Domain.Entities;
using Infrastructure.EF;
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
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Get(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger<Get>();
        }


        [HttpGet("api/v1/workers/{id}", Name = "GetWorker")]
        [SwaggerOperation(
            Summary = "Gets a worker by id",
            Description = "Gets a worker by id",
            OperationId = "Worker.Get",
            Tags = new[] { "Workers" })]
        [ProducesResponseType(typeof(WorkerGetResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult<WorkerGetResult>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var worker = await _db.FindAsync<Worker>(new object[] { id }, cancellationToken: cancellationToken);
            if (worker == null)
            {
                _logger.LogWarning(LogEvents.WorkerNotFoundEvent, "Order with id {OrderId} was not found", id);

                return NotFound(id);
            }

            var result = new WorkerGetResult
            {
                WorkerId = worker.Id,
                Company = worker.Company,
                Name = worker.Name,
                PhoneNumber = worker.PhoneNumber,
            };

            return Ok(result);
        }
    }
}