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
using API.V1.Features.Expenses.Requests;
using API.V1.Features.Products.Request;
using API.V1.Features.Products.Response;

namespace API.V1.Features.Products
{
    public class List : BaseAsyncEndpoint.WithRequest<ProductListPageRequest>.WithResponse<List<ProductListResult>>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public List(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<List>();
        }

        [HttpGet("api/v1/products", Name = "GetProductList")]
        [SwaggerOperation(
            Summary = "Gets a list of product",
            Description = "List of product",
            OperationId = "Product.List",
            Tags = new[] { "Products" })]
        [ProducesResponseType(typeof(List<ProductListResult>), (int)HttpStatusCode.OK)]
        public override async Task<ActionResult<List<ProductListResult>>> HandleAsync(
            [FromQuery] ProductListPageRequest request,
            CancellationToken cancellationToken = default)
        {
            var orderList = await _db.Products.OrderByDescending(x => x.Id).
                                      Skip(request.PerPage * (request.Page - 1)).
                                      Take(request.PerPage).
                                      Select(
                                          x => new ProductListResult
                                          {
                                              ProductId = x.Id,
                                              Type = x.Type,
                                              Description = x.Description,
                                          }).
                                      ToListAsync(cancellationToken);

            return Ok(orderList);
        }
    }
}