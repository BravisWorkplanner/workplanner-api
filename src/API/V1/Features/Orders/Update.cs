using API.V1.Features.Orders.Request;
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
    public class Update : BaseAsyncEndpoint.WithRequest<OrderUpdateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Update(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<Delete>();
        }

        [HttpPut("api/v1/orders", Name = "UpdateOrder")]
        [SwaggerOperation(
            Summary = "Update an Order",
            Description = "Update an Order",
            OperationId = "Order.Update",
            Tags = new[] { "Orders" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] OrderUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var order = await _db.FindAsync<Order>(new object[] { request.OrderId }, cancellationToken: cancellationToken);
            if (order == null)
            {
                _logger.LogWarning(LogEvents.OrderNotFoundEvent, "Order with id {OrderId} was not found", request.OrderId);

                return NotFound(request.OrderId);
            }

            order.Address = request.Address;
            order.Description = request.Description;
            order.StartDate = request.StartDate;
            order.EndDate = request.EndDate;
            order.OrderStatus = request.OrderStatus;
            order.InvoiceDate = request.InvoiceDate;

            _db.Entry(order).State = EntityState.Modified;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(request.OrderId);
        }
    }
}