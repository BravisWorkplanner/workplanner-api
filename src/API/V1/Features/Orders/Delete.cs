using APIEndpoints;
using Domain;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace API.V1.Features.Orders
{
    public class Delete : BaseAsyncEndpoint.WithRequest<int>.WithResponse<int>
    {
        private readonly IAsyncRepository<Order> _repository;
        private readonly ILogger _logger;

        public Delete(IAsyncRepository<Order> repository, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Delete>();
            _repository = repository;
        }

        [HttpDelete("api/v1/orders/{id}", Name = "DeleteOrder")]
        [SwaggerOperation(Summary = "Delete a new Order", Description = "Delete a new Order", OperationId = "Order.Delete", Tags = new[] {"Orders"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        public override async Task<ActionResult<int>> HandleAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.OrderGetEvent, "Getting order with id {OrderId}", id);

            var order = await _repository.GetByIdAsync(id, cancellationToken);
            if (order == null)
            {
                _logger.LogWarning(LogEvents.OrderNotFoundEvent, "Order with id {OrderId} was not found", id);

                return NotFound(id);
            }

            _logger.LogInformation(LogEvents.OrderDeleteEvent, "Deleting order with id {OrderId}", id);

            await _repository.DeleteAsync(order, cancellationToken);

            return Ok(id);
        }
    }
}