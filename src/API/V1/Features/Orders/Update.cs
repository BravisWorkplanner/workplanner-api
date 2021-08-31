using API.V1.Features.Orders.Request;
using APIEndpoints;
using AutoMapper;
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
    public class Update : BaseAsyncEndpoint.WithRequest<OrderUpdateRequest>.WithResponse<int>
    {
        private readonly IAsyncRepository<Order> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public Update(IAsyncRepository<Order> repository, ILoggerFactory loggerFactory, IMapper mapper)
        {
            _logger = loggerFactory.CreateLogger<Delete>();
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPut("api/v1/orders", Name = "UpdateOrder")]
        [SwaggerOperation(Summary = "Update an Order", Description = "Update an Order", OperationId = "Order.Update", Tags = new[] {"Orders"})]
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK)]
        public override async Task<ActionResult<int>> HandleAsync(
            [FromBody] OrderUpdateRequest request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(LogEvents.OrderGetEvent, "Getting order with id {OrderId}", request.OrderId);

            var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                _logger.LogWarning(LogEvents.OrderNotFoundEvent, "Order with id {OrderId} was not found", request.OrderId);

                return NotFound(request.OrderId);
            }

            // TODO: find out better way to handle persisting object number value from fetched order
            var updatedOrder = _mapper.Map<Order>(request);
            updatedOrder.ObjectNumber = order.ObjectNumber;

            _logger.LogInformation(LogEvents.OrderUpdateEvent, "Updating order with id {OrderId}", request.OrderId);

            await _repository.UpdateAsync(order, cancellationToken);

            return Ok(request.OrderId);
        }
    }
}