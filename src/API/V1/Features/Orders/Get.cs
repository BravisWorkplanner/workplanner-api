using API.V1.Features.Orders.Response;
using APIEndpoints;
using Domain;
using Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.V1.Features.Orders
{
    public class Get : BaseAsyncEndpoint.WithRequest<int>.WithResponse<OrderGetResult>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Get(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<Get>();
        }

        [HttpGet("api/v1/orders/{id}", Name = "GetOrder")]
        [SwaggerOperation(
            Summary = "Get a specific Order",
            Description = "Get a specific Order",
            OperationId = "Order.Get",
            Tags = new[] { "Orders" })]
        [ProducesResponseType(typeof(OrderGetResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult<OrderGetResult>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var order = await _db.Orders.Include(x => x.Expenses).
                                  ThenInclude(x => x.Worker).
                                  Include(x => x.Expenses).
                                  ThenInclude(x => x.Product).
                                  Include(x => x.TimeRegistrations).
                                  ThenInclude(x => x.Worker).
                                  AsSplitQuery().
                                  FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (order == null)
            {
                _logger.LogWarning(LogEvents.OrderNotFoundEvent, "Order with id {OrderId} was not found", id);

                return NotFound(id);
            }

            var result = new OrderGetResult
            {
                OrderId = order.Id,
                ObjectNumber = order.ObjectNumber,
                Address = order.Address,
                Description = order.Description,
                CustomerName = order.CustomerName,
                CustomerPhoneNumber = order.CustomerPhoneNumber,
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                OrderStatus = order.OrderStatus,
                InvoiceDate = order.InvoiceDate,
                Expenses = order.Expenses.Select(
                                     x => new ExpenseResult
                                     {
                                         Id = x.Id,
                                         Description = x.Description,
                                         Price = x.Price,
                                         Product = x.Product.Type,
                                         Worker = x.Worker.Name,
                                     }).
                                 ToList(),
                TimeRegistrations = order.TimeRegistrations.Select(
                                              x => new TimeRegistrationResult
                                              {
                                                  Id = x.Id,
                                                  Day = x.Day,
                                                  CreatedAt = x.CreatedAt,
                                                  Week = x.Week,
                                                  Hours = x.Hours,
                                                  Worker = x.Worker.Name,
                                              }).
                                          ToList(),
            };

            return Ok(result);
        }
    }
}