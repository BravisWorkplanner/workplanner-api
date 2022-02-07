using API.V1.Features.Workers.Request;
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
    public class Update : BaseAsyncEndpoint.WithRequest<WorkerUpdateRequest>.WithoutResponse
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Update(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger<Update>();
        }

        [HttpPut("api/v1/workers", Name = "WorkerUpdate")]
        [SwaggerOperation(
            Summary = "Update an Worker",
            Description = "Update an Worker",
            OperationId = "Worker.Update",
            Tags = new[] { "Workers" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult> HandleAsync(
            [FromBody] WorkerUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var worker = await _db.FindAsync<Worker>(
                new object[] { request.WorkerId },
                cancellationToken: cancellationToken);
            if (worker == null)
            {
                _logger.LogWarning(
                    LogEvents.WorkerNotFoundEvent,
                    "Worker with id {WorkerId} was not found",
                    request.WorkerId);

                return NotFound(request.WorkerId);
            }

            worker.Company = request.Company;
            worker.PhoneNumber = request.PhoneNumber;
            worker.Name = request.Name;

            _db.Entry(worker).State = EntityState.Modified;
            await _db.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}