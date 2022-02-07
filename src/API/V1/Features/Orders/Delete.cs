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

namespace API.V1.Features.Orders
{
    public class Delete : BaseAsyncEndpoint.WithRequest<int>.WithResponse<int>
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _db;

        public Delete(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Delete>();
            _db = db;
        }

        [HttpDelete("api/v1/orders/{id}", Name = "DeleteOrder")]
        [SwaggerOperation(
            Summary = "Delete a new Order",
            Description = "Delete a new Order",
            OperationId = "Order.Delete",
            Tags = new[] { "Orders" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var order = await _db.FindAsync<Order>(new object[] { id }, cancellationToken: cancellationToken);
            if (order == null)
            {
                _logger.LogWarning(LogEvents.OrderNotFoundEvent, "Order with id {OrderId} was not found", id);

                return NotFound(id);
            }

            _db.Entry(order).State = EntityState.Deleted;
            await _db.SaveChangesAsync(cancellationToken);

            return Ok(id);
        }
    }
}