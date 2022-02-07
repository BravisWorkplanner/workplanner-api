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
using API.V1.Features.Products.Request;

namespace API.V1.Features.Products
{
    public class Update : BaseAsyncEndpoint.WithRequest<ProductUpdateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;
        private readonly ILogger _logger;

        public Update(AppDbContext db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger<Update>();
        }

        [HttpPut("api/v1/products", Name = "UpdateProduct")]
        [SwaggerOperation(
            Summary = "Update a product",
            Description = "Update a product",
            OperationId = "Product.Update",
            Tags = new[] { "Products" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] ProductUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            var product = await _db.FindAsync<Product>(new object[] { request.ProductId }, cancellationToken);
            if (product == null)
            {
                _logger.LogWarning(
                    LogEvents.ProductNotFoundEvent,
                    "Product with id {ProductId} was not found",
                    request.ProductId);

                return NotFound(request.ProductId);
            }

            product.Type = request.Type;
            product.Description = request.Description;

            _db.Entry(product).State = EntityState.Modified;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(request.ProductId);
        }
    }
}