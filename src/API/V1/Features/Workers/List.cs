using API.V1.Features.Workers.Request;
using API.V1.Features.Workers.Response;
using APIEndpoints;
using Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public List(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger<List>();
        }

        [HttpGet("api/v1/workers", Name = "GetWorkerList")]
        [SwaggerOperation(
            Summary = "Gets a list of workers",
            Description = "List of workers",
            OperationId = "Worker.List",
            Tags = new[] { "Workers" })]
        [ProducesResponseType(typeof(List<WorkerListResult>), (int)HttpStatusCode.OK)]
        public override async Task<ActionResult<List<WorkerListResult>>> HandleAsync(
            [FromQuery] WorkerListPageRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await _db.Workers.OrderByDescending(x => x.Id).
                                   Skip(request.PerPage * (request.Page - 1)).
                                   Take(request.PerPage).
                                   Select(
                                       x => new WorkerListResult
                                       {
                                           WorkerId = x.Id,
                                           PhoneNumber = x.PhoneNumber,
                                           Company = x.Company,
                                           Name = x.Name,
                                       }).
                                   ToListAsync(cancellationToken);

            return Ok(result);
        }
    }
}