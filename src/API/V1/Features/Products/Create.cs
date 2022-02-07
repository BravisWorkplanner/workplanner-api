using System.Net;
using System.Threading;
using System.Threading.Tasks;
using API.V1.Features.Products.Request;
using APIEndpoints;
using Domain.Entities;
using Infrastructure.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace API.V1.Features.Products
{
    public class Create : BaseAsyncEndpoint.WithRequest<ProductCreateRequest>.WithResponse<int>
    {
        private readonly AppDbContext _db;

        public Create(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("api/v1/products", Name = "CreateProduct")]
        [SwaggerOperation(
            Summary = "Create a product",
            Description = "Create a product",
            OperationId = "Product.Create",
            Tags = new[] { "Products" })]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] ProductCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var product = new Product
            {
                Description = request.Description,
                Type = request.Type,
            };

            _db.Entry(product).State = EntityState.Added;

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(product.Id);
        }
    }
}