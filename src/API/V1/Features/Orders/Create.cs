using API.V1.Features.Orders.Request;
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

namespace API.V1.Features.Orders
{
    public class Create : BaseAsyncEndpoint.WithRequest<OrderCreateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Create(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<Create>();
        }

        [HttpPost("api/v1/orders", Name = "CreateOrder")]
        [SwaggerOperation(
            Summary = "Create a new Order",
            Description = "Create a new Order",
            OperationId = "Order.Create",
            Tags = new[] { "Orders" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] OrderCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var order = new Order
            {
                Address = request.Address,
                Description = request.Description,
                CustomerName = request.CustomerName,
                CustomerPhoneNumber = request.CustomerPhoneNumber,
            };

            _db.Entry(order).State = EntityState.Added;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(order.Id);
        }
    }
}