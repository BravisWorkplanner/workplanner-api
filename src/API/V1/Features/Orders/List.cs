using API.V1.Features.Orders.Request;
using API.V1.Features.Orders.Response;
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

namespace API.V1.Features.Orders
{
    public class List : BaseAsyncEndpoint.WithRequest<OrderListPageRequest>.WithResponse<List<OrderListResult>>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public List(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<List>();
        }

        [HttpGet("api/v1/orders", Name = "GetOrderList")]
        [SwaggerOperation(
            Summary = "Gets a list of orders",
            Description = "List of orders",
            OperationId = "Order.List",
            Tags = new[] { "Orders" })]
        [ProducesResponseType(typeof(List<OrderListResult>), (int)HttpStatusCode.OK)]
        public override async Task<ActionResult<List<OrderListResult>>> HandleAsync(
            [FromQuery] OrderListPageRequest request,
            CancellationToken cancellationToken = default)
        {
            var orderList = await _db.Orders.OrderByDescending(x => x.Id).
                                      Skip(request.PerPage * (request.Page - 1)).
                                      Take(request.PerPage).
                                      Select(
                                          x => new OrderListResult
                                          {
                                              OrderId = x.Id,
                                              ObjectNumber = x.ObjectNumber,
                                              OrderStatus = x.OrderStatus,
                                              Address = x.Address,
                                              Description = x.Description,
                                              StartDate = x.StartDate,
                                              EndDate = x.EndDate,
                                              InvoiceDate = x.InvoiceDate,
                                          }).
                                      ToListAsync(cancellationToken);

            return Ok(orderList);
        }
    }
}