using API.V1.Features.Workers.Request;
using APIEndpoints;
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
    public class Create : BaseAsyncEndpoint.WithRequest<WorkerCreateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Create(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<Create>();
        }

        [HttpPost("api/v1/workers", Name = "CreateWorker")]
        [SwaggerOperation(
            Summary = "Create a new worker",
            Description = "Create a new worker",
            OperationId = "Worker.Create",
            Tags = new[] { "Workers" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] WorkerCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var worker = new Worker
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Company = request.Company,
            };

            _db.Entry(worker).State = EntityState.Added;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(worker.Id);
        }
    }
}