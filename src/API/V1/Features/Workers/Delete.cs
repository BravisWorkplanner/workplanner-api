using APIEndpoints;
using Domain;
using Domain.Entities;
using Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.V1.Features.Workers
{
    public class Delete : BaseAsyncEndpoint.WithRequest<int>.WithResponse<int>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Delete(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<Delete>();
        }

        [HttpDelete("api/v1/workers/{id}", Name = "DeleteWorker")]
        [SwaggerOperation(
            Summary = "Delete a new worker",
            Description = "Delete a new worker",
            OperationId = "Worker.Delete",
            Tags = new[] { "Workers" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var worker = await _db.FindAsync<Worker>(new object[] { id }, cancellationToken: cancellationToken);
            if (worker == null)
            {
                _logger.LogWarning(LogEvents.WorkerNotFoundEvent, "Worker with id {WorkerId} was not found", id);

                return NotFound(id);
            }

            _db.Entry(worker).State = EntityState.Deleted;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(id);
        }
    }
}