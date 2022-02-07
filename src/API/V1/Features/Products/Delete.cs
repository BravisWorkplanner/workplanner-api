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

namespace API.V1.Features.Products
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

        [HttpDelete("api/v1/products/{id}", Name = "DeleteProduct")]
        [SwaggerOperation(
            Summary = "Delete a product",
            Description = "Delete a product",
            OperationId = "Product.Delete",
            Tags = new[] { "Products" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromRoute] int id,
            CancellationToken cancellationToken = default)
        {
            var order = await _db.FindAsync<Product>(new object[] { id }, cancellationToken);
            if (order == null)
            {
                _logger.LogWarning(LogEvents.ProductNotFoundEvent, "Product with id {ProductId} was not found", id);

                return NotFound(id);
            }

            _db.Entry(order).State = EntityState.Deleted;
            await _db.SaveChangesAsync(cancellationToken);

            return Ok(id);
        }
    }
}